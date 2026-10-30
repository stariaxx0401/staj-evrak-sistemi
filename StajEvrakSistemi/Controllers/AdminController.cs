using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StajEvrakSistemi.Controllers;

// sadece admin rolü girebilir, giriş yapmayan login'e, yetkisiz olan 403'e gidiyor
[Authorize(Roles = "ROLE_ADMIN")]

// admin paneli, sadece ROLE_ADMIN girsin kısmını backend sonra ekleyecek
public class AdminController : Controller
{
    public IActionResult Index() => View();

    // veri seti yükleme sayfası
    public IActionResult VeriSeti() => View();

    // tamamlanan başvuruların listesi
    public IActionResult Tamamlandi() => View();
}