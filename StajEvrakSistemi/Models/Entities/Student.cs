namespace StajEvrakSistemi.Models.Entities;

// öğrenci kullanıcı, AppUser'ın bilgilerine ek olarak kendi bilgilerini tutuyor
public class Student : AppUser
{
    // bu alanlar admin'in yüklediği veri setinden gelecek
    public string AdSoyad { get; set; } = "";
    public string TcKimlikNo { get; set; } = "";
    public string Uyruk { get; set; } = "T.C."; // "T.C." ya da yabancı, kimlik kontrolü buna bakıyor
    public string OgrenciNo { get; set; } = "";
    public DateTime DogumTarihi { get; set; }
    public string Adres { get; set; } = "";
    public string Bolum { get; set; } = "";
    public int OncekiStajGunSayisi { get; set; }
    public string Telefon { get; set; } = "";
    public string Ek16TeslimYili { get; set; } = "";     
    public string SaglikYardimiStatusu { get; set; } = "";

    // EF Core için, dışarıdan çağrılmasın diye private
    private Student() { }

    // base ile AppUser'ın yapıcısını çağırıyoruz, rolü de burada sabitliyoruz
    public Student(string email, string password) : base(email, password)
    {
        Role = "ROLE_STUDENT";
    }
}