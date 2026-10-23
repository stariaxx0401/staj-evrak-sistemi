namespace StajEvrakSistemi.Validators;

// bir kontrolün sonucu: geçti mi, geçmediyse neden
public class ValidationResult
{
    // private set: sonuç sadece aşağıdaki Ok ve Fail ile oluşsun
    public bool IsValid { get; private set; }
    public string? Message { get; private set; }

    // başarılı sonuç
    public static ValidationResult Ok() => new() { IsValid = true };

    // hatalı sonuç, mesajı rapora yazacağız
    public static ValidationResult Fail(string message) => new() { IsValid = false, Message = message };
}