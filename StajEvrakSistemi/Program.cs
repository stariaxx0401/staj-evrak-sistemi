using Microsoft.AspNetCore.Authentication.Cookies;
using StajEvrakSistemi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using StajEvrakSistemi.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// veritabanı olarak SQLite kullanıyoruz, staj.db dosyası proje klasöründe oluşacak
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=staj.db"));

// giriş için cookie kullanıyoruz, giriş yapmayan kişi login sayfasına gidiyor
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/Account/Login");

var app = builder.Build();

// veritabanında hiç kullanıcı yoksa test admini ve test öğrencisi ekliyoruz (sadece geliştirme için)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Admins.Any())
        db.Admins.Add(new AdminUser("admin@test.com", "Admin123!"));

    if (!db.Students.Any())
        db.Students.Add(new Student("ogrenci@test.com", "Ogrenci123!"));

    db.SaveChanges();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
