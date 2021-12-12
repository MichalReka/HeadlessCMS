using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.UserRoles.Any())
            {
                return;   // DB has been seeded
            }

            var userRoles = new UserRole[]
            {
                new UserRole{
                    Name = "Admin",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                }
            };

            var users = new List<User>();
            users.Add(new User {
                Name = "Admin",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Password = "Admin",
            });

            foreach (UserRole userRole in userRoles)
            {
                context.UserRoles.Add(userRole);
            }

            foreach (User user in users)
            {
                context.Users.Add(user);
            }

            context.SaveChanges();
        }
    }
}