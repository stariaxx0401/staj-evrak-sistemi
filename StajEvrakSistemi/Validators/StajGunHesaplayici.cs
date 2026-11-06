namespace StajEvrakSistemi.Validators;

// resmi tatil günleri, 2026-2027 (Diyanet takvimine göre)
// arife yarım günleri çalışma günü sayılıyor
public static class ResmiTatil
{
    private static readonly HashSet<DateTime> Gunler = new()
    {
        // sabit tatiller
        new(2026,1,1), new(2026,4,23), new(2026,5,1), new(2026,5,19), new(2026,7,15), new(2026,8,30), new(2026,10,29),
        new(2027,1,1), new(2027,4,23), new(2027,5,1), new(2027,7,15), new(2027,8,30), new(2027,10,29),
        // 2026 dini bayramlar
        new(2026,3,20), new(2026,3,21), new(2026,3,22),
        new(2026,5,27), new(2026,5,28), new(2026,5,29), new(2026,5,30),
        // 2027 dini bayramlar (19 Mayıs aynı zamanda Kurban Bayramı'nın 4. günü)
        new(2027,3,9), new(2027,3,10), new(2027,3,11),
        new(2027,5,16), new(2027,5,17), new(2027,5,18), new(2027,5,19)
    };

    public static bool TatilMi(DateTime tarih) => Gunler.Contains(tarih.Date);
}

// seçilen haftalık günlere göre aralıktaki staj günlerini sayıyor (doküman 6.3)
public static class StajGunHesaplayici
{
    public static int Hesapla(DateTime baslangic, DateTime bitis, List<DayOfWeek> gunler)
    {
        int sayi = 0;
        for (var gun = baslangic.Date; gun <= bitis.Date; gun = gun.AddDays(1))
        {
            // seçilen günlerden biri ve resmi tatil değilse sayılıyor
            if (gunler.Contains(gun.DayOfWeek) && !ResmiTatil.TatilMi(gun))
                sayi++;
        }
        return sayi;
    }
}