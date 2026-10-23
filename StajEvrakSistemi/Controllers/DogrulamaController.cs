using Microsoft.AspNetCore.Mvc;
using StajEvrakSistemi.Models;

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
}