namespace Feedback_System.Roles;


public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }  // Kullanıcının tam adı
    public DateTime CreatedAt { get; set; } = DateTime.Now;  // Kullanıcının oluşturulma tarihi
}
