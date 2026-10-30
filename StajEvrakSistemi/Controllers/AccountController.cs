using StajEvrakSistemi.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StajEvrakSistemi.Data;
using StajEvrakSistemi.Models;
using System.Security.Claims;

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
            LogKaydet(model.Email, false);
            ModelState.AddModelError("", "E-posta veya şifre hatalı.");
            return View(model);
        }

        // 5 hatadan sonra hesap kilitli
        if (user.IsLocked())
        {
            LogKaydet(model.Email, false);
            ModelState.AddModelError("", "Çok fazla hatalı deneme, hesap geçici olarak kilitlendi.");
            return View(model);
        }

        if (!user.CheckPassword(model.Password))
        {
            user.RegisterFailedLogin();
            _db.SaveChanges();
            LogKaydet(model.Email, false);
            ModelState.AddModelError("", "E-posta veya şifre hatalı.");
            return View(model);
        }

        user.ResetFailedLogins();
        _db.SaveChanges();
        LogKaydet(model.Email, true);

        // rol bilgisini cookie'nin içine koyuyoruz
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };
        var kimlik = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(kimlik));

        // admin kendi paneline, öğrenci kendi paneline gidiyor
        if (user.Role == "ROLE_ADMIN")
            return RedirectToAction("Index", "Admin");

        return RedirectToAction("Index", "Student");
    } 

    // her denemeyi tabloya yazıyor, başarılı ve başarısız ayrımı Success alanında
    private void LogKaydet(string email, bool basarili)
    {
        _db.LoginAttempts.Add(new LoginAttempt
        {
            Email = email ?? "",
            Success = basarili,
            AttemptedAt = DateTime.UtcNow,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
        });
        _db.SaveChanges();
    }

    // çıkış yapınca cookie siliniyor
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");

    }
    // yetkisiz erişimde çalışıyor, 403 kodu döndürüp bilgi sayfasını gösteriyor
    public IActionResult AccessDenied()
    {
        Response.StatusCode = 403;
        return View();
    }
}