using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajEvrakSistemi.Data;
using StajEvrakSistemi.Models;
using StajEvrakSistemi.Models.Entities;
using StajEvrakSistemi.Validators;

namespace StajEvrakSistemi.Controllers;

// öğrenci ekranları, sadece öğrenci rolü girebilir
[Authorize(Roles = "ROLE_STUDENT")]
public class StudentController : Controller
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    // veritabanı ve klasör bilgisi otomatik geliyor
    public StudentController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult EvrakYukle() => View(new EvrakYukleViewModel());

    [HttpPost]
    public IActionResult EvrakYukle(EvrakYukleViewModel model)
    {
        // giriş yapan öğrenciyi e-postasından buluyoruz
        var ogrenci = _db.Students.FirstOrDefault(s => s.Email == User.Identity!.Name);
        if (ogrenci == null)
            return Forbid();

        // dosya seçilmiş mi
        if (model.Dosya == null || model.Dosya.Length == 0)
        {
            ModelState.AddModelError("", "Lütfen bir dosya seçin.");
            return View(model);
        }

        // sunucuda da formatı kontrol ediyoruz, sadece tarayıcıya güvenmiyoruz
        var izinli = new[] { ".doc", ".docx", ".pdf", ".jpg", ".jpeg", ".png" };
        var uzanti = Path.GetExtension(model.Dosya.FileName).ToLower();
        if (!izinli.Contains(uzanti))
        {
            ModelState.AddModelError("", "Sadece Word (.doc/.docx), PDF, JPEG veya PNG yüklenebilir.");
            return View(model);
        }

        // kontrol sonucunda çıkan sorunlar (alan adı, mesaj)
        var sorunlar = new List<(string Alan, string Mesaj)>();

        // belgedeki öğrenci bilgileri veritabanındaki kayıtla uyuşuyor mu (doküman 4.3)
        if (Farkli(model.AdSoyad, ogrenci.AdSoyad))
            sorunlar.Add(("Ad Soyad", "Belgedeki ad soyad sistemdeki kayıtla uyuşmuyor."));
        if (Farkli(model.TcKimlikNo, ogrenci.TcKimlikNo))
            sorunlar.Add(("TC Kimlik No", "Belgedeki kimlik numarası sistemdeki kayıtla uyuşmuyor."));
        if (Farkli(model.Uyruk, ogrenci.Uyruk))
            sorunlar.Add(("Uyruk", "Belgedeki uyruk sistemdeki kayıtla uyuşmuyor."));

        // doğrulayıcılara verilecek bilgiler
        var ctx = new ValidationContext
        {
            TcKimlikNo = model.TcKimlikNo ?? "",
            Uyruk = model.Uyruk ?? "T.C.",
            Telefon = model.Telefon ?? "",
            Eposta = model.Eposta ?? "",
            Adres = model.Adres ?? "",
            MuhendisSayisi = model.MuhendisSayisi ?? 0,
            MuhendisBolumu = model.MuhendisBolumu ?? ""
        };

        // hepsi BaseValidator, döngüde aynı Validate çağrılıyor (çok biçimlilik)
        var kontroller = new List<(string Alan, BaseValidator Dogrulayici)>
        {
            ("TC Kimlik No", new TcKimlikValidator()),
            ("Telefon", new TelefonValidator()),
            ("E-posta", new EpostaValidator()),
            ("Adres", new AdresValidator()),
            ("Mühendis", new MuhendisValidator())
        };

        foreach (var (alan, dogrulayici) in kontroller)
        {
            var sonuc = dogrulayici.Validate(ctx);
            if (!sonuc.IsValid)
                sorunlar.Add((alan, sonuc.Message!));
        }

        // basit tarih kontrolleri (30 gün ve staj günü kuralları sonra gelecek)
        if (string.IsNullOrWhiteSpace(model.OgretimYili))
            sorunlar.Add(("Öğretim yılı", "Öğretim yılı girilmelidir (örn. 2025-2026 Bahar)."));
        if (model.StajBaslangic == null || model.StajBitis == null)
            sorunlar.Add(("Staj tarihleri", "Staj başlangıç ve bitiş tarihi girilmelidir."));
        else if (model.StajBitis < model.StajBaslangic)
            sorunlar.Add(("Staj tarihleri", "Bitiş tarihi başlangıç tarihinden önce olamaz."));

        // dosyayı kaydediyoruz, adı rastgele olsun ki kimse tahmin edemesin
        var klasor = Path.Combine(_env.ContentRootPath, "Uploads");
        Directory.CreateDirectory(klasor);
        var kayitAdi = Guid.NewGuid() + uzanti;
        using (var akim = new FileStream(Path.Combine(klasor, kayitAdi), FileMode.Create))
        {
            model.Dosya.CopyTo(akim);
        }

        var belge = new InternshipDocument
        {
            StudentId = ogrenci.Id,
            FilePath = kayitAdi,
            FileType = uzanti,
            OgrenciAdSoyad = model.AdSoyad ?? "",
            TcKimlikNo = model.TcKimlikNo ?? "",
            Uyruk = model.Uyruk ?? "T.C.",
            OgretimYili = model.OgretimYili ?? "",
            StajBaslangic = model.StajBaslangic,
            StajBitis = model.StajBitis,
            Telefon = model.Telefon ?? "",
            Eposta = model.Eposta ?? "",
            Adres = model.Adres ?? "",
            MuhendisSayisi = model.MuhendisSayisi ?? 0,
            MuhendisBolumu = model.MuhendisBolumu ?? ""
        };

        // Yüklendi → İnceleniyor
        belge.ChangeStatus(DocumentStatus.Inceleniyor);

        // sorun varsa Hatalı/Eksik, yoksa Onaylandı (Tamamlandı'ya geçiş final aşamasında)
        belge.ChangeStatus(sorunlar.Count == 0 ? DocumentStatus.Onaylandi : DocumentStatus.HataliEksik);

        // rapor ve içindeki hatalar
        var rapor = new ValidationReport { Document = belge };
        foreach (var (alan, mesaj) in sorunlar)
            rapor.Issues.Add(new ValidationIssue { FieldName = alan, Message = mesaj });

        _db.Documents.Add(belge);
        _db.ValidationReports.Add(rapor);
        _db.SaveChanges();

        return RedirectToAction("Rapor", new { id = belge.Id });
    }

    // belgenin kontrol raporunu gösteriyor, öğrenci sadece kendi belgesini görebilir
    public IActionResult Rapor(int id)
    {
        var ogrenci = _db.Students.FirstOrDefault(s => s.Email == User.Identity!.Name);
        if (ogrenci == null)
            return Forbid();

        var rapor = _db.ValidationReports
            .Include(r => r.Issues)
            .Include(r => r.Document)
            .Where(r => r.DocumentId == id && r.Document!.StudentId == ogrenci.Id)
            .OrderByDescending(r => r.Id)
            .FirstOrDefault();

        if (rapor == null)
            return NotFound();

        return View(rapor);
    }

    // iki metni büyük küçük harf ve Türkçe karaktere dikkat ederek karşılaştırıyor
    private static bool Farkli(string? a, string? b)
    {
        return string.Compare((a ?? "").Trim(), (b ?? "").Trim(),
            new CultureInfo("tr-TR"), CompareOptions.IgnoreCase) != 0;
    }
}