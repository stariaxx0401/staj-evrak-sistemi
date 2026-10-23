using Microsoft.AspNetCore.Mvc;
using StajEvrakSistemi.Models;

namespace StajEvrakSistemi.Controllers;

public class ArayuzController : Controller
{
    public IActionResult Index()
    {
        var ogrenci = new ArayuzOgrenci
        {
            Ad = "Ali Veli",
            OgrenciNo = "123456"
        };
        return View(ogrenci);
    }
}

