using StajEvrakSistemi.Validators;

namespace StajEvrakSistemi.Validators;

// TC ve yabancı kimlik no kontrolü
public class TcKimlikValidator : BaseValidator
{
    // BaseValidator'daki Validate'i burada kendi kuralımıza göre yazdık
    public override ValidationResult Validate(ValidationContext context)
    {
        var no = context.TcKimlikNo;

        // 11 hane ve sadece rakam olmalı
        if (no.Length != 11 || !no.All(char.IsDigit))
            return ValidationResult.Fail("Kimlik numarası 11 haneli ve sadece rakam olmalıdır.");

        // uyruğa göre kural değişiyor (dokümanda 6.2)
        if (context.Uyruk == "T.C.")
        {
            if (no[0] == '0')
                return ValidationResult.Fail("T.C. kimlik numarasının ilk hanesi 0 olamaz.");
        }
        else if (!no.StartsWith("99"))
        {
            return ValidationResult.Fail("Yabancı kimlik numarası 99 ile başlamalıdır.");
        }

        return ValidationResult.Ok(); // hepsi geçti
    }
}