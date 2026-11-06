namespace StajEvrakSistemi.Models.Entities;

// öğrencinin yüklediği Ek-16 belgesi
public class InternshipDocument
{
    public int Id { get; set; }

    // belgeyi yükleyen öğrenci
    public int StudentId { get; set; }
    public Student? Student { get; set; }

    public string FilePath { get; set; } = "";
    public string FileType { get; set; } = "";
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    // durum dışarıdan doğrudan değiştirilemesin, sadece ChangeStatus ile değişsin (kapsülleme)
    public DocumentStatus Status { get; private set; } = DocumentStatus.Yuklendi;

    // belgeden okunan ya da formdan girilen alanlar
    public string OgrenciAdSoyad { get; set; } = "";
    public string TcKimlikNo { get; set; } = "";
    public string Uyruk { get; set; } = "T.C.";
    public string OgretimYili { get; set; } = "";   // örn. 2025-2026 Bahar
    public DateTime? StajBaslangic { get; set; }
    public DateTime? StajBitis { get; set; }
    public string Telefon { get; set; } = "";
    public string Eposta { get; set; } = "";
    public string Adres { get; set; } = "";
    public int MuhendisSayisi { get; set; }
    public string MuhendisBolumu { get; set; } = "";
    public string HaftalikGunler { get; set; } = "";
    public int BelgedekiGunSayisi { get; set; }
    public int HesaplananGunSayisi { get; set; }

    // durumu değiştirir, akışa uymayan geçişleri engeller
    public void ChangeStatus(DocumentStatus yeni, bool adminMi = false)
    {
        // tamamlanan belge sadece admin tarafından değiştirilebilir (doküman bölüm 5)
        if (Status == DocumentStatus.Tamamlandi && !adminMi)
            throw new InvalidOperationException("Tamamlandı durumundaki belge değiştirilemez.");

        // admin dışında sadece akıştaki sıraya uygun geçişlere izin var
        if (!adminMi && !GecisGecerliMi(Status, yeni))
            throw new InvalidOperationException($"{Status} durumundan {yeni} durumuna geçilemez.");

        Status = yeni;

        if (yeni == DocumentStatus.Tamamlandi)
            CompletedAt = DateTime.UtcNow;
    }

    // akış: Yüklendi → İnceleniyor → Hatalı/Eksik (→ Yüklendi) ya da Onaylandı → Tamamlandı
    private static bool GecisGecerliMi(DocumentStatus simdiki, DocumentStatus yeni)
    {
        return (simdiki, yeni) switch
        {
            (DocumentStatus.Yuklendi, DocumentStatus.Inceleniyor) => true,
            (DocumentStatus.Inceleniyor, DocumentStatus.HataliEksik) => true,
            (DocumentStatus.Inceleniyor, DocumentStatus.Onaylandi) => true,
            (DocumentStatus.HataliEksik, DocumentStatus.Yuklendi) => true, // düzeltilip yeniden yüklenir
            (DocumentStatus.Onaylandi, DocumentStatus.Tamamlandi) => true,
            _ => false
        };
    }
}