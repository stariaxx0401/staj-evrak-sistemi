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
}