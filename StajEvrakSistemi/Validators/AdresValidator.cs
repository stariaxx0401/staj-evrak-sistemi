using System.Text.RegularExpressions;

namespace StajEvrakSistemi.Validators;

// adres kontrolü: mahalle, cadde/sokak, bina no, daire no, ilçe/il sırasıyla ve tam olmalı (doküman 6.1)
public class AdresValidator : BaseValidator
{
    public override ValidationResult Validate(ValidationContext context)
    {
        var adres = context.Adres;

        if (string.IsNullOrWhiteSpace(adres))
            return ValidationResult.Fail("Adres boş olamaz.");

        // her parçayı bir öncekinin bittiği yerden sonra arıyoruz,
        // böylece hem eksik var mı hem sıra doğru mu kontrol etmiş oluyoruz
        int konum = 0;

        if (!Bul(adres, @"\bmah", ref konum))
            return ValidationResult.Fail("Adreste mahalle bilgisi eksik.");

        if (!Bul(adres, @"(cad|sok|sk\b|cd\b|bulvar|blv)", ref konum))
            return ValidationResult.Fail("Mahalleden sonra cadde/sokak bilgisi eksik veya sıra yanlış.");

        if (!Bul(adres, @"\bno\s*[:.]?\s*\d+", ref konum))
            return ValidationResult.Fail("Cadde/sokaktan sonra bina no eksik veya sıra yanlış.");

        if (!Bul(adres, @"(daire|\bd)\s*[:.]?\s*\d+", ref konum))
            return ValidationResult.Fail("Bina nodan sonra daire no eksik veya sıra yanlış.");

        // en sonda ilçe/il olmalı (Çankaya/Ankara gibi)
        if (!Bul(adres, @"\p{L}+\s*/\s*\p{L}+", ref konum))
            return ValidationResult.Fail("Daire nodan sonra ilçe/il bilgisi eksik (örn. Çankaya/Ankara).");

        return ValidationResult.Ok();
    }

    // desen adreste konum'dan sonra var mı bakıyor, bulursa konum'u ilerletiyor
    private static bool Bul(string adres, string desen, ref int konum)
    {
        var eslesme = Regex.Match(adres.Substring(konum), desen, RegexOptions.IgnoreCase);
        if (!eslesme.Success)
            return false;

        konum += eslesme.Index + eslesme.Length;
        return true;
    }
}