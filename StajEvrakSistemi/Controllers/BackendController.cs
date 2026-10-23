using Microsoft.AspNetCore.Mvc;
using StajEvrakSistemi.Models;

namespace StajEvrakSistemi.Controllers;

public class  BackendController : Controller
{
    public IActionResult Index()
    {
        var ogrenci = new BackendOgrenci
        {
            Ad = "Ali Veli",
            OgrenciNo = "123456"
        };
        return View(ogrenci);
    }
}