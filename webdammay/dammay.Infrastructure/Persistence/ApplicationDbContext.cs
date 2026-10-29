using Microsoft.EntityFrameworkCore;
using webdammay.dammay.Application.Command.Interfaces;
using webdammay.dammay.Domain.Entities;
using System.Reflection.Emit;

namespace webdammay.dammay.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<StoredFile> StoredFiles => Set<StoredFile>();

    
    public DbSet<User> Users => Set<User>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StoredFile>(entity =>
        {
            entity.ToTable("StoredFiles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.FileSize).IsRequired();
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.IsShared).HasDefaultValue(false);
            entity.Property(e => e.ShareToken).HasMaxLength(64);
        });

    }
}