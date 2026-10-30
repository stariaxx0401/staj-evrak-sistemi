namespace StajEvrakSistemi.Validators;

// mühendis sayısı kuralı (doküman 6.5)
public class MuhendisValidator : BaseValidator
{
    public override ValidationResult Validate(ValidationContext context)
    {
        // en az 1 mühendis olmalı
        if (context.MuhendisSayisi < 1)
            return ValidationResult.Fail("Mühendis sayısı en az 1 olmalıdır.");

        // 5'in altındaysa mezun olduğu bölüm sorulacak, boşsa uyarı çıkıyor
        // (uyarı kutusunu arayüzde Kişi 3 gösterecek)
        if (context.MuhendisSayisi < 5 && string.IsNullOrWhiteSpace(context.MuhendisBolumu))
            return ValidationResult.Fail("Mühendis sayısı 5'in altında, mühendisin mezun olduğu bölümü giriniz.");

        return ValidationResult.Ok();
    }
}