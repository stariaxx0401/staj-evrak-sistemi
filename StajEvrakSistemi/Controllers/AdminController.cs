using Microsoft.AspNetCore.Mvc;

namespace StajEvrakSistemi.Controllers;

// admin paneli, sadece ROLE_ADMIN girsin kısmını backend sonra ekleyecek
public class AdminController : Controller
{
    public IActionResult Index() => View();

    // veri seti yükleme sayfası
    public IActionResult VeriSeti() => View();

    // tamamlanan başvuruların listesi
    public IActionResult Tamamlandi() => View();
}