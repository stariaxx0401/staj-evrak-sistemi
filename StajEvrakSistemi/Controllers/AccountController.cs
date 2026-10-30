using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StajEvrakSistemi.Data;
using StajEvrakSistemi.Models;

namespace StajEvrakSistemi.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _db;

    // veritabanı Program.cs'teki ayardan buraya otomatik geliyor
    public AccountController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == model.Email);

        // kullanıcı yoksa hangisinin yanlış olduğunu söylemiyoruz
        if (user == null)
        {
            ModelState.AddModelError("", "E-posta veya şifre hatalı.");
            return View(model);
        }

        // 5 hatadan sonra hesap kilitli
        if (user.IsLocked())
        {
            ModelState.AddModelError("", "Çok fazla hatalı deneme, hesap geçici olarak kilitlendi.");
            return View(model);
        }

        if (!user.CheckPassword(model.Password))
        {
            user.RegisterFailedLogin();
            _db.SaveChanges();
            ModelState.AddModelError("", "E-posta veya şifre hatalı.");
            return View(model);
        }

        user.ResetFailedLogins();
        _db.SaveChanges();

        // rol bilgisini cookie'nin içine koyuyoruz
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var kimlik = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(kimlik));

        return RedirectToAction("Index", "Home");
    }

    // çıkış yapınca cookie siliniyor
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}