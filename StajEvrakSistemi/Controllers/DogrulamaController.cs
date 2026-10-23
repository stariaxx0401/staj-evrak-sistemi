using Microsoft.AspNetCore.Mvc;
using StajEvrakSistemi.Models;
using StajEvrakSistemi.Validators;

namespace StajEvrakSistemi.Controllers;

public class DogrulamaController: Controller

{
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
    public IActionResult TestHepsi(string tc, string uyruk = "T.C.", string tel = "", string eposta = "")
    {
        var ctx = new ValidationContext
        {
            TcKimlikNo = tc ?? "",
            Uyruk = uyruk,
            Telefon = tel,
            Eposta = eposta
        };

        var dogrulayicilar = new List<BaseValidator>
    {
        new TcKimlikValidator(),
        new TelefonValidator(),
        new EpostaValidator()
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