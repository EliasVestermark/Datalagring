using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class BookingCatalogContext(DbContextOptions<BookingCatalogContext> options) : DbContext(options)
{
    public virtual DbSet<TimeEntity> Times { get; set; }
    public virtual DbSet<StatusEntity> Statuses { get; set; }
    public virtual DbSet<ParticipantsEntity> Participants { get; set; }
    public virtual DbSet<LocationEntity> Locations { get; set; }
    public virtual DbSet<ClientEntity> Clients { get; set; }
    public virtual DbSet<BookingEntity> Boookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientEntity>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<StatusEntity>()
            .HasIndex(x => x.StatusText)
            .IsUnique();

        modelBuilder.Entity<ParticipantsEntity>()
            .HasIndex(x => x.Amount)
            .IsUnique();
    }
}
