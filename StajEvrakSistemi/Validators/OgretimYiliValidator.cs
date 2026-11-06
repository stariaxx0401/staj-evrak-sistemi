using System.Text.RegularExpressions;

namespace StajEvrakSistemi.Validators;

// öğretim yılı ardışık yıl olmalı ve staj tarihleri bu yılın içinde kalmalı (doküman 6.3)
public class OgretimYiliValidator : BaseValidator
{
    public override ValidationResult Validate(ValidationContext context)
    {
        // örn. 2025-2026 Bahar
        var m = Regex.Match(context.OgretimYili.Trim(), @"^(\d{4})-(\d{4})\s+(Güz|Bahar)$", RegexOptions.IgnoreCase);
        if (!m.Success)
            return ValidationResult.Fail("Öğretim yılı '2025-2026 Bahar' biçiminde olmalıdır.");

        int y1 = int.Parse(m.Groups[1].Value);
        int y2 = int.Parse(m.Groups[2].Value);
        if (y2 != y1 + 1)
            return ValidationResult.Fail("Öğretim yılındaki yıllar ardışık olmalıdır (örn. 2025-2026).");

        // tarihler boşsa o hatayı controller söylüyor
        if (context.StajBaslangic == null || context.StajBitis == null)
            return ValidationResult.Ok();

        // öğretim yılı 1 Eylül - 31 Ağustos kabul edildi (varsayım, hocaya sorulabilir)
        var bas = new DateTime(y1, 9, 1);
        var bit = new DateTime(y2, 8, 31);
        if (context.StajBaslangic < bas || context.StajBitis > bit)
            return ValidationResult.Fail($"Staj tarihleri {y1}-{y2} öğretim yılı ({bas:dd.MM.yyyy} - {bit:dd.MM.yyyy}) içinde olmalıdır.");

        return ValidationResult.Ok();
    }
}