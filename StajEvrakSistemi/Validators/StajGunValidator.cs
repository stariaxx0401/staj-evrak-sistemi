namespace StajEvrakSistemi.Validators;

// hesaplanan staj günü ile belgedeki gün sayısı uyuşmalı (doküman 6.3)
public class StajGunValidator : BaseValidator
{
    public override ValidationResult Validate(ValidationContext context)
    {
        // tarihler eksik ya da ters ise o hatayı controller söylüyor
        if (context.StajBaslangic == null || context.StajBitis == null
            || context.StajBitis < context.StajBaslangic)
            return ValidationResult.Ok();

        if (context.HaftalikGunler.Count == 0)
            return ValidationResult.Fail("Haftalık staj günleri seçilmelidir.");

        int hesaplanan = StajGunHesaplayici.Hesapla(
            context.StajBaslangic.Value, context.StajBitis.Value, context.HaftalikGunler);

        if (hesaplanan != context.BelgedekiGunSayisi)
            return ValidationResult.Fail(
                $"Belgedeki staj gün sayısı ({context.BelgedekiGunSayisi}) ile hesaplanan gün sayısı " +
                $"({hesaplanan}) uyuşmuyor. Seçilen günler ve resmi tatiller dikkate alınmıştır.");

        return ValidationResult.Ok();
    }
}