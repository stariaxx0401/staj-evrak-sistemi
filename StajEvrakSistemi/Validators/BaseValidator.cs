namespace StajEvrakSistemi.Validators;

// tüm doğrulayıcıların atası (soyutlama)
// Validate'in içi yok, her alt sınıf kendi kuralını yazıyor
public abstract class BaseValidator
{
    public abstract ValidationResult Validate(ValidationContext context);
}