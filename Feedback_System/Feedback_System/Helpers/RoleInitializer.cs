using Microsoft.AspNetCore.Identity;
using Feedback_System.Models;
using System.Threading.Tasks;

namespace Feedback_System.Helpers;

    public static class RoleInitializer
    {
        public static async Task InitializeRolesAndAdmin(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            // Rollerin eklenmesi
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName).ConfigureAwait(false);
                if (!roleExist)
                {
                    // Rolleri oluşturuyoruz
                    await roleManager.CreateAsync(new IdentityRole(roleName)).ConfigureAwait(false);
                }
            }

            // İlk admin kullanıcısının eklenmesi
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail).ConfigureAwait(false);

            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Admin User"
                };

                var createAdmin = await userManager.CreateAsync(admin, "Admin@123").ConfigureAwait(false);

                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin").ConfigureAwait(false);
                }
            }
        }
    }


