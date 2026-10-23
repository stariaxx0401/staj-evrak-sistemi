namespace StajEvrakSistemi.Models.Entities;

// admin kullanıcı, kendine özel alanı yok sadece rolü farklı
public class AdminUser : AppUser
{
    // EF Core için
    private AdminUser() { }

    public AdminUser(string email, string password) : base(email, password)
    {
        Role = "ROLE_ADMIN";
    }
}