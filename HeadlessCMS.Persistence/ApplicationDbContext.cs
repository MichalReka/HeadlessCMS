using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HeadlessCMS.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IPasswordEncryptService _passwordEncryptService;
        public ApplicationDbContext(DbContextOptions options, IPasswordEncryptService passwordEncryptService) : base(options)
        {
            _passwordEncryptService = passwordEncryptService;
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Media> Medias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
            .HasOne(p => p.CreatedBy)
            .WithMany(b => b.CreatedArticles)
            .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Media>()
            .HasOne(p => p.CreatedBy)
            .WithMany(b => b.CreatedMedias)
            .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Article>()
            .HasOne(p => p.UpdatedBy)
            .WithMany(b => b.UpdatedArticles)
            .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Media>()
            .HasOne(p => p.UpdatedBy)
            .WithMany(b => b.UpdatedMedias)
            .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Article>().ToTable("Article");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Media>().ToTable("Media");

            DbInitializer.Initialize(modelBuilder, _passwordEncryptService);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}