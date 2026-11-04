namespace StajEvrakSistemi.Models;

// evrak yükleme formundan gelen bilgiler
// OCR olmadığı için belgedeki alanları öğrenci elle giriyor
public class EvrakYukleViewModel
{
    public IFormFile? Dosya { get; set; }

    // belgede yazan öğrenci bilgileri (veritabanıyla karşılaştırılacak)
    public string? AdSoyad { get; set; }
    public string? TcKimlikNo { get; set; }
    public string? Uyruk { get; set; } = "T.C.";

    public string? OgretimYili { get; set; }
    public DateTime? StajBaslangic { get; set; }
    public DateTime? StajBitis { get; set; }

    public string? Telefon { get; set; }
    public string? Eposta { get; set; }
    public string? Adres { get; set; }
    public int? MuhendisSayisi { get; set; }
    public string? MuhendisBolumu { get; set; }
}