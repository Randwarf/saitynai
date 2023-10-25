using API.Auth;
using API.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class AuthDbSeeder
    {
        private readonly UserManager<GameUser> _usermanager;
        private readonly RoleManager<IdentityRole> _rolemanager;

        public AuthDbSeeder(UserManager<GameUser> usermanager, RoleManager<IdentityRole> rolemanager)
        {
            _usermanager = usermanager;
            _rolemanager = rolemanager;
        }

        public async Task SeedAsync()
        {
            await AddDefaultRoles();
            await AddAdminUser();
        }

        private async Task AddAdminUser()
        {
            var admin = new GameUser
            {
                UserName = "MatVai2",
                Email = "matvai2@ktu.lt"
            };

            var existingAdminUser = await _usermanager.FindByNameAsync(admin.UserName);
            if (existingAdminUser == null) 
            {
                var createAdminResult = await _usermanager.CreateAsync(admin, "ABC-abc-123");
                if (createAdminResult.Succeeded)
                    await _usermanager.AddToRolesAsync(admin, GameRoles.All);
            }
        }

        private async Task AddDefaultRoles()
        {
            foreach (var role in GameRoles.All)
            {
                var roleExists = await _rolemanager.RoleExistsAsync(role);
                if (!roleExists)
                    await _rolemanager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
