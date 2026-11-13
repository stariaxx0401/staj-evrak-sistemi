# Staj Evrak Doğrulama ve Yönetim Sistemi

Programlama Laboratuvarı III, 2026-2027 Güz dönemi projesi.

## Ekip
- Serhat Türmüş (Backend)
- Abdullah Aydın (Doğrulama)
- Furkan Yıldız (Arayüz)

## Kullanılan teknolojiler
ASP.NET Core MVC, Entity Framework Core, SQLite, BCrypt.

## Nasıl çalıştırılır
1. Repoyu klonlayın.
2. `StajEvrakSistemi` klasöründe `dotnet ef database update` komutunu çalıştırın.
3. Projeyi Visual Studio'dan ya da `dotnet run` ile başlatın.

## Test kullanıcıları
- Admin: admin@test.com / Admin123!
- Öğrenci: ogrenci@test.com / Ogrenci123!
- Admin veri seti yüklerse öğrenciler e-postalarıyla, ilk şifre olarak öğrenci numarasıyla giriş yapar.

## Vize kapsamında yapılanlar
- Giriş, şifre hash'leme, 5 hatalı denemede kilitleme, giriş logu
- Rol bazlı yetkilendirme ve 403 sayfası
- Admin CSV veri seti yükleme
- Öğrenci belge yükleme ve kural kontrolleri
- Başvuru listesi, filtreleme, Tamamla ve Tamamlananlar
- Diyagramlar ve rapor (docs klasörü)

## Final için kalanlar
OCR, Google Form otomatik doldurma, Ek-17 üretimi.