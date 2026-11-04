namespace StajEvrakSistemi.Models.Entities;

// bir belge kontrol edildiğinde üretilen rapor, her yeniden yüklemede yenisi oluşuyor
public class ValidationReport
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public InternshipDocument? Document { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // raporun içindeki hatalar
    public List<ValidationIssue> Issues { get; set; } = new();
}