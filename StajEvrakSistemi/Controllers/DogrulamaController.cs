using StajEvrakSistemi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using StajEvrakSistemi.Models;
using StajEvrakSistemi.Validators;

namespace StajEvrakSistemi.Controllers;

public class DogrulamaController: Controller

{
    // geçici test: durum akışı kurallarını deniyoruz, sonra silinecek
    public IActionResult TestDurum()
    {
        var d = new InternshipDocument();
        var sonuc = new List<string> { $"Başlangıç: {d.Status}" };

        d.ChangeStatus(DocumentStatus.Inceleniyor);
        d.ChangeStatus(DocumentStatus.Onaylandi);
        d.ChangeStatus(DocumentStatus.Tamamlandi);
        sonuc.Add($"Akış sonu: {d.Status}");

        // tamamlanmış belgeyi öğrenci değiştirmeye çalışıyor
        try { d.ChangeStatus(DocumentStatus.Yuklendi); }
        catch (InvalidOperationException e) { sonuc.Add("Öğrenci denemesi: " + e.Message); }

        // admin değiştirebiliyor
        d.ChangeStatus(DocumentStatus.Yuklendi, adminMi: true);
        sonuc.Add($"Admin değiştirdi: {d.Status}");

        // akışı atlamaya çalışmak
        var d2 = new InternshipDocument();
        try { d2.ChangeStatus(DocumentStatus.Onaylandi); }
        catch (InvalidOperationException e) { sonuc.Add("Atlama denemesi: " + e.Message); }

        return Content(string.Join("\n", sonuc));
    }
    public IActionResult Index()
    {
        var ogrenci = new DogrulamaOgrenci
        {
            Ad = "Ali Veli",
            OgrenciNo = "123456"
        };
        return View(ogrenci);
    }

    // geçici test sayfası: /Dogrulama/Test?tc=12345678901&uyruk=T.C.
    public IActionResult Test(string tc, string uyruk = "T.C.")
    {
        var dogrulayici = new TcKimlikValidator();
        var sonuc = dogrulayici.Validate(new ValidationContext
        {
            TcKimlikNo = tc ?? "",
            Uyruk = uyruk
        });
        return Content(sonuc.IsValid ? "Geçerli" : sonuc.Message);
    }

    // hepsini tek listede çalıştırıyoruz, her biri kendi Validate'ini çağırıyor (çok biçimlilik)
    public IActionResult TestHepsi(string tc, string uyruk = "T.C.", string tel = "", string eposta = "",
    string adres = "", int muhendis = 0, string bolum = "")
    {
        var ctx = new ValidationContext
        {
            TcKimlikNo = tc ?? "",
            Uyruk = uyruk,
            Telefon = tel,
            Eposta = eposta,
            Adres = adres,
            MuhendisSayisi = muhendis,
            MuhendisBolumu = bolum
        };

        var dogrulayicilar = new List<BaseValidator>
    {
        new TcKimlikValidator(),
        new TelefonValidator(),
        new EpostaValidator(),
        new AdresValidator(),
        new MuhendisValidator()
    };

        var hatalar = new List<string>();
        foreach (var d in dogrulayicilar)
        {
            var sonuc = d.Validate(ctx);
            if (!sonuc.IsValid)
                hatalar.Add(sonuc.Message!);
        }

        return Content(hatalar.Count == 0 ? "Hepsi geçerli" : string.Join("\n", hatalar));
    }
}