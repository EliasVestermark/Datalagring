using DatalagringUppgift.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatalagringUppgift.Contexts;

public class BookingCatalogContext : DbContext
{
    public BookingCatalogContext()
    {
    }

    public BookingCatalogContext(DbContextOptions<BookingCatalogContext> options) : base(options)
    {
    }

    public virtual DbSet<TimeEntity> Times { get; set; }
    public virtual DbSet<StatusEntity> Statuses { get; set; }
    public virtual DbSet<ParticipantsEntity> Participants { get; set; }
    public virtual DbSet<LocationEntity> Locations { get; set; }
    public virtual DbSet<ClientEntity> Clients { get; set; }
    public virtual DbSet<BookingEntity> Boookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StatusEntity>()
            .HasIndex(x => x.StatusText)
            .IsUnique();

        modelBuilder.Entity<ClientEntity>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<ParticipantsEntity>()
            .HasIndex(x => x.Amount)
            .IsUnique();

        modelBuilder.Entity<StatusEntity>()
            .HasIndex(x => x.StatusText)
            .IsUnique();
    }
}
