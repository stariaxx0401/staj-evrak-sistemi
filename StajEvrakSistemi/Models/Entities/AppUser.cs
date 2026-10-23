namespace StajEvrakSistemi.Models.Entities;

// giriş yapan herkesin ortak sınıfı, Student ve AdminUser bundan türüyor
public abstract class AppUser
{
    public int Id { get; private set; }
    public string Email { get; private set; } = "";

    // şifreyi düz tutmuyoruz hash'li tutuyoruz, private set ile dışarıdan değiştirilemiyor
    public string PasswordHash { get; private set; } = "";

    // ROLE_STUDENT ya da ROLE_ADMIN, alt sınıflar belirliyor
    public string Role { get; protected set; } = "";

    public int FailedLoginCount { get; private set; }
    public DateTime? LockedUntil { get; private set; }

    // EF Core veritabanından okurken bunu kullanıyor
    protected AppUser() { }

    protected AppUser(string email, string password)
    {
        Email = email;
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
    }

    // girilen şifre hash ile uyuşuyor mu
    public bool CheckPassword(string password)
        => BCrypt.Net.BCrypt.Verify(password, PasswordHash);

    // kilit süresi bitmediyse hesap hala kilitli
    public bool IsLocked()
        => LockedUntil.HasValue && LockedUntil > DateTime.UtcNow;

    // hatalı girişi say, 5 olunca kilitle
    public void RegisterFailedLogin()
    {
        FailedLoginCount++;
        if (FailedLoginCount >= 5)
            LockedUntil = DateTime.UtcNow.AddMinutes(15); // süreyi biz belirledik, hocaya sor
    }

    // giriş başarılıysa sayacı sıfırla
    public void ResetFailedLogins()
    {
        FailedLoginCount = 0;
        LockedUntil = null;
    }
}