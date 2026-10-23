using System.Text.RegularExpressions;

namespace StajEvrakSistemi.Validators;

// e-posta kontrolü (ad@alanadi.uzanti şeklinde olmalı)
public class EpostaValidator : BaseValidator
{
    public override ValidationResult Validate(ValidationContext context)
    {
        var eposta = context.Eposta;

        if (string.IsNullOrWhiteSpace(eposta))
            return ValidationResult.Fail("E-posta boş olamaz.");

        // @ öncesi bir şey, @ sonrası bir şey, nokta ve uzantı olmalı
        if (!Regex.IsMatch(eposta, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return ValidationResult.Fail("Geçerli bir e-posta formatı giriniz (örn. ad@alanadi.uzanti).");

        return ValidationResult.Ok();
    }
}