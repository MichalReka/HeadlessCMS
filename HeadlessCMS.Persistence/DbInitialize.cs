using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HeadlessCMS.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(ModelBuilder modelBuilder, IPasswordEncryptService passwordEncryptService)
        {
            var userRoles = new UserRole[]
            {
                new UserRole{
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                }
            };
            (string password, byte[] salt) = passwordEncryptService.HashPassword("1234");
            var users = new User[]
            {
                new User {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Password = password,
                    Salt = salt
                }
            };


            modelBuilder.Entity<UserRole>().HasData(userRoles);
            modelBuilder.Entity<User>().HasData(users);
        }
    }
}