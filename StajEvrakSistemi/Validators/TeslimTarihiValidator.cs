namespace StajEvrakSistemi.Validators;

// belge staj başlangıcından en az 30 gün önce teslim edilmeli (doküman 6.3)
public class TeslimTarihiValidator : BaseValidator
{
    public override ValidationResult Validate(ValidationContext context)
    {
        if (context.StajBaslangic == null)
            return ValidationResult.Ok(); // controller söylüyor

        var enGec = context.StajBaslangic.Value.Date.AddDays(-30);
        if (context.TeslimTarihi.Date > enGec)
        {
            // uygun değilse önerilen tarihi gösteriyoruz
            var enErken = context.TeslimTarihi.Date.AddDays(30);
            return ValidationResult.Fail(
                $"Belge staj başlangıcından en az 30 gün önce teslim edilmelidir. " +
                $"Bu başlangıç için en geç teslim tarihi {enGec:dd.MM.yyyy}. " +
                $"Bugün teslim ediyorsanız staj en erken {enErken:dd.MM.yyyy} tarihinde başlayabilir.");
        }
        return ValidationResult.Ok();
    }
}