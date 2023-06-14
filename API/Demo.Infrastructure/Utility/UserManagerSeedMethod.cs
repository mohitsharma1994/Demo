using Demo.Infrastructure.Entity;
using Microsoft.AspNetCore.Identity;

namespace Demo.Infrastructure.Utility
{
    public static class UserManagerSeedMethod
    {
        public static async Task InsertDataInUser(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    FirstName="Test",
                    LastName="Test",
                    Email="Test@gmail.com",
                    UserName = "Test@gmail.com",
                    PhoneNumber="12364479"
                };

               await userManager.CreateAsync(user,"Admin@123");
            }
        }
    }
}
