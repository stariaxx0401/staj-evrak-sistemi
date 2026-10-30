namespace StajEvrakSistemi.Validators;

// doğrulayıcılara verilecek bilgileri taşıyor
// yeni kontroller eklendikçe buraya alan eklenecek
public class ValidationContext
{
    public string TcKimlikNo { get; set; } = "";
    public string Uyruk { get; set; } = "T.C.";
    public string Telefon { get; set; } = "";
    public string Eposta { get; set; } = "";
    public string Adres { get; set; } = "";
    public int MuhendisSayisi { get; set; }
    public string MuhendisBolumu { get; set; } = "";
}