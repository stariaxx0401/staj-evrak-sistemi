using System.Text.RegularExpressions;

namespace StajEvrakSistemi.Validators;

// telefon kontrolü, +90 xxx xxx xx xx formatında olmalı (doküman 6.1)
public class TelefonValidator : BaseValidator
{
    public override ValidationResult Validate(ValidationContext context)
    {
        var tel = context.Telefon;

        // e-posta ile telefonun ikisi de zorunlu
        if (string.IsNullOrWhiteSpace(tel))
            return ValidationResult.Fail("Telefon numarası boş olamaz.");

        // \d{3} = 3 rakam demek
        if (!Regex.IsMatch(tel, @"^\+90 \d{3} \d{3} \d{2} \d{2}$"))
            return ValidationResult.Fail("Telefon +90 xxx xxx xx xx formatında olmalıdır.");

        return ValidationResult.Ok();
    }
}