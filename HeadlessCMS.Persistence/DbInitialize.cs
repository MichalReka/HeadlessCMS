using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HeadlessCMS.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(ModelBuilder modelBuilder, IPasswordEncryptService passwordEncryptService)
        {
            (string password, byte[] salt) = passwordEncryptService.HashPassword("1234");
            var users = new User[]
            {
                new User {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Password = password,
                    Role = UserRole.Admin,
                    Salt = salt
                }
            };


            modelBuilder.Entity<User>().HasData(users);
        }
    }
}