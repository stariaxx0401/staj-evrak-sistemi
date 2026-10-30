using Microsoft.AspNetCore.Mvc;

namespace StajEvrakSistemi.Controllers;

// öğrenci ekranları, sadece ROLE_STUDENT girsin kısmını backend sonra ekleyecek
public class StudentController : Controller
{
    // öğrenci ana sayfası
    public IActionResult Index() => View();

    // evrak yükleme sayfası, yükleme kodunu doğrulama kişisi yazacak
    [HttpGet]
    public IActionResult EvrakYukle() => View();
}