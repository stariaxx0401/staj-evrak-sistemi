namespace StajEvrakSistemi.Models;

// giriş formundan gelen bilgileri taşıyor
public class LoginViewModel
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}