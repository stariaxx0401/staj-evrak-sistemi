namespace StajEvrakSistemi.Models.Entities;

// belgenin akıştaki durumları (doküman bölüm 5)
public enum DocumentStatus
{
    Yuklendi,
    Inceleniyor,
    HataliEksik,
    Onaylandi,
    Tamamlandi
}