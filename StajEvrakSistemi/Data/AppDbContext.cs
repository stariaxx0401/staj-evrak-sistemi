using Microsoft.EntityFrameworkCore;
using StajEvrakSistemi.Models.Entities;

namespace StajEvrakSistemi.Data;

// veritabanıyla konuşan sınıf, DbSet'ler tablolara karşılık geliyoor
public class AppDbContext : DbContext
{
    // ayarlar (hangi veritabanı vs.) Program.cs'ten buraya geliyor
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<AdminUser> Admins => Set<AdminUser>();

    public DbSet<LoginAttempt> LoginAttempts => Set<LoginAttempt>();
    public DbSet<InternshipDocument> Documents => Set<InternshipDocument>();
    public DbSet<ValidationReport> ValidationReports => Set<ValidationReport>();
    public DbSet<ValidationIssue> ValidationIssues => Set<ValidationIssue>();
}
