using Microsoft.AspNetCore.Mvc;
using StajEvrakSistemi.Models;

namespace StajEvrakSistemi.Controllers;

public class AccountController : Controller
{
    // sayfa ilk açıldığında boş formu gösteriyor
    // giriş yapma kısmını (HttpPost) backend kişisi ekleyecek
    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }
}