using Microsoft.EntityFrameworkCore;
using StajEvrakSistemi.Models.Entities;

namespace StajEvrakSistemi.Data;

// veritabanıyla konuşan sınıf, DbSet'ler tablolara karşılık geliyor
public class AppDbContext : DbContext
{
    // ayarlar (hangi veritabanı vs.) Program.cs'ten buraya geliyor
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<AdminUser> Admins => Set<AdminUser>();
}
