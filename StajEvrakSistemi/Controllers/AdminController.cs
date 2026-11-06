using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajEvrakSistemi.Data;
using StajEvrakSistemi.Models.Entities;

namespace StajEvrakSistemi.Controllers;

// admin paneli, sadece admin rolü girebilir
[Authorize(Roles = "ROLE_ADMIN")]
public class AdminController : Controller
{
    private readonly AppDbContext _db;

    // veritabanı Program.cs'teki ayardan buraya otomatik geliyor
    public AdminController(AppDbContext db)
    {
        _db = db;
    }
     
    public IActionResult Index() => View();

    // veri seti yükleme sayfası
    [HttpGet]
    public IActionResult VeriSeti() => View();

    // yüklenen CSV'yi okuyup öğrencileri veritabanına kaydediyor
    [HttpPost]
    public IActionResult VeriSeti(IFormFile dosya)
    {
        if (dosya == null || dosya.Length == 0)
        {
            TempData["Mesaj"] = "Lütfen bir dosya seçin.";
            return RedirectToAction("VeriSeti");
        }

        // sunucuda da uzantıyı kontrol ediyoruz, sadece tarayıcıya güvenmiyoruz
        if (Path.GetExtension(dosya.FileName).ToLower() != ".csv")
        {
            TempData["Mesaj"] = "Sadece .csv dosyası yüklenebilir.";
            return RedirectToAction("VeriSeti");
        }

        var tarihFormatlari = new[] { "dd.MM.yyyy", "yyyy-MM-dd", "dd/MM/yyyy" };
        var gorulenler = new HashSet<string>(); // aynı dosyada tekrar eden kayıtlar için
        int eklenen = 0, atlanan = 0;
        var hatalar = new List<string>();

        using var okuyucu = new StreamReader(dosya.OpenReadStream());

        // ilk satır başlık, ayırıcının ; mü , mü olduğunu buradan anlıyoruz
        var baslik = okuyucu.ReadLine();
        if (baslik == null)
        {
            TempData["Mesaj"] = "Dosya boş.";
            return RedirectToAction("VeriSeti");
        }
        char ayirici = baslik.Contains(';') ? ';' : ',';

        string? satirMetni;
        int satir = 1;

        while ((satirMetni = okuyucu.ReadLine()) != null)
        {
            satir++;
            if (string.IsNullOrWhiteSpace(satirMetni))
                continue;

            // satırı sütunlara ayırıyoruz, sıra dokümandaki veri seti sırası
            var a = satirMetni.Split(ayirici);
            if (a.Length < 12)
            {
                hatalar.Add($"{satir}. satır eksik sütun");
                continue;
            }

            var adSoyad = a[0].Trim();
            var tc = a[1].Trim();
            var uyruk = a[2].Trim();
            var ogrNo = a[3].Trim();
            var dogumMetni = a[4].Trim();
            var adres = a[5].Trim();
            var bolum = a[6].Trim();
            var teslimYili = a[7].Trim();
            var oncekiGunMetni = a[8].Trim();
            var eposta = a[9].Trim();
            var telefon = a[10].Trim();
            var saglik = a[11].Trim();

            // giriş için en az bunlar lazım
            if (eposta == "" || ogrNo == "" || adSoyad == "")
            {
                hatalar.Add($"{satir}. satır eksik");
                continue;
            }

            // aynı öğrenci veritabanında ya da dosyada zaten varsa atla
            if (!gorulenler.Add(ogrNo) ||
                _db.Students.Any(s => s.OgrenciNo == ogrNo || s.Email == eposta))
            {
                atlanan++;
                continue;
            }

            DateTime.TryParseExact(dogumMetni, tarihFormatlari, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var dogum);
            int.TryParse(oncekiGunMetni, out var oncekiGun);

            // ilk şifre geçici olarak öğrenci numarası (hocalara sorulacak)
            var ogrenci = new Student(eposta, ogrNo)
            {
                AdSoyad = adSoyad,
                TcKimlikNo = tc,
                Uyruk = uyruk,
                OgrenciNo = ogrNo,
                DogumTarihi = dogum,
                Adres = adres,
                Bolum = bolum,
                Ek16TeslimYili = teslimYili,
                OncekiStajGunSayisi = oncekiGun,
                Telefon = telefon,
                SaglikYardimiStatusu = saglik
            };

            _db.Students.Add(ogrenci);
            eklenen++;
        }

        _db.SaveChanges();

        var mesaj = $"{eklenen} öğrenci eklendi, {atlanan} kayıt zaten vardı.";
        if (hatalar.Count > 0)
            mesaj += " Sorunlu satırlar: " + string.Join(", ", hatalar);

        TempData["Mesaj"] = mesaj;
        return RedirectToAction("VeriSeti");
    }

    // tüm başvurular (Tamamlandı olanlar ayrı sekmede), isteğe bağlı durum ve arama filtresi
    public IActionResult Basvurular(string? durum, string? ara)
    {
        var sorgu = _db.Documents.Include(d => d.Student)
            .Where(d => d.Status != DocumentStatus.Tamamlandi);

        if (!string.IsNullOrWhiteSpace(durum) && Enum.TryParse<DocumentStatus>(durum, out var secilen))
            sorgu = sorgu.Where(d => d.Status == secilen);

        if (!string.IsNullOrWhiteSpace(ara))
            sorgu = sorgu.Where(d => d.OgrenciAdSoyad.Contains(ara) || d.TcKimlikNo.Contains(ara));

        ViewBag.Ara = ara;
        return View(sorgu.OrderByDescending(d => d.UploadedAt).ToList());
    }

    // tamamlanan başvurular, ayrı sekmede listeleniyor
    public IActionResult Tamamlandi()
    {
        var liste = _db.Documents.Include(d => d.Student)
            .Where(d => d.Status == DocumentStatus.Tamamlandi)
            .OrderByDescending(d => d.CompletedAt)
            .ToList();
        return View(liste);
    }

    // bir belgenin kontrol raporunu admin görüyor
    public IActionResult Detay(int id)
    {
        var rapor = _db.ValidationReports
            .Include(r => r.Issues)
            .Include(r => r.Document)
            .Where(r => r.DocumentId == id)
            .OrderByDescending(r => r.Id)
            .FirstOrDefault();

        if (rapor == null)
            return NotFound();

        return View(rapor);
    }

    // Onaylandı durumundaki belgeyi Tamamlandı yapıyor
    [HttpPost]
    public IActionResult Tamamla(int id)
    {
        var belge = _db.Documents.FirstOrDefault(d => d.Id == id);
        if (belge == null)
            return NotFound();

        // sadece onaylanmış belge tamamlanabilir
        if (belge.Status != DocumentStatus.Onaylandi)
        {
            TempData["Mesaj"] = "Sadece Onaylandı durumundaki belge tamamlanabilir.";
            return RedirectToAction("Basvurular");
        }

        belge.ChangeStatus(DocumentStatus.Tamamlandi);
        _db.SaveChanges();

        TempData["Mesaj"] = "Belge Tamamlandı durumuna alındı.";
        return RedirectToAction("Tamamlandi");
    }

}