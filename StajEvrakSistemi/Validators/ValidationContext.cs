namespace StajEvrakSistemi.Validators;

// doğrulayıcılara verilecek bilgileri taşıyor
// yeni kontroller eklendikçe buraya alan eklenecek
public class ValidationContext
{
    public string TcKimlikNo { get; set; } = "";
    public string Uyruk { get; set; } = "T.C.";
}