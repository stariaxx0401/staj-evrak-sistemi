namespace StajEvrakSistemi.Models.Entities;

// her giriş denemesinin kaydı (doküman 4.1, başarısız denemeler loglanacak)
public class LoginAttempt
{
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public bool Success { get; set; }          // giriş başarılı mıydı
    public DateTime AttemptedAt { get; set; }
    public string? IpAddress { get; set; }
}