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

            foreach (UserRole userRole in userRoles)
            {
                context.UserRoles.Add(userRole);
            }

            context.SaveChanges();
        }
    }
}