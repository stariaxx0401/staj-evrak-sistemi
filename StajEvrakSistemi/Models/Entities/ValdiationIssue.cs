namespace StajEvrakSistemi.Models.Entities;

// raporun içindeki tek bir hata (hangi alan, ne sorun)
public class ValidationIssue
{
    public int Id { get; set; }
    public int ReportId { get; set; }
    public string FieldName { get; set; } = "";
    public string Message { get; set; } = "";
}