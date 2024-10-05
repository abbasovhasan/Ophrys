namespace Feedback_System.Validations;

public class PasswordValidationConfig
{
    public static void Configure(IdentityOptions options)
    {
        // Şifre gereksinimlerini özelleştiriyoruz
        options.Password.RequireDigit = false; // Rakam gerektirmesin
        options.Password.RequiredLength = 8;   // Minimum 6 karakter olsun
        options.Password.RequireNonAlphanumeric = false; // Özel karakter gerektirmesin
        options.Password.RequireUppercase = false; // Büyük harf gerektirmesin
        options.Password.RequireLowercase = false; // Küçük harf gerektirmesin
    }
}
