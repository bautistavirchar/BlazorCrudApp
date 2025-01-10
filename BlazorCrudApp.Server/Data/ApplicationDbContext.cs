using BlazorCrudApp.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorCrudApp.Server.Data;

//public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
//{
//	public ApplicationDbContext(DbContextOptions options) : base(options) { }
//	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//	{
//		optionsBuilder.ConfigureWarnings(warnings =>
//			warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
//	}
//}

public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Personal> Personals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Personal>(entity =>
        {
            entity.ToTable("Personal");

            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateDeleted).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
        });

        base.OnModelCreating(modelBuilder);
    }
}
