using System;
using System.Collections.Generic;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class ProductCatalogContext : DbContext
{
    public ProductCatalogContext()
    {
    }

    public ProductCatalogContext(DbContextOptions<ProductCatalogContext> options) : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Ingridient> Ingridients { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0BDBF1182A");

            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Ingridient>(entity =>
        {
            entity.HasKey(e => e.IngridientId).HasName("PK__Ingridie__17B0EDEB4E55B215");

            entity.HasIndex(e => e.Name, "UQ__Ingridie__737584F6DE613E82").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD9AF759CB");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__4F7CD00D");

            entity.HasMany(d => d.Ingridients).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductIngridient",
                    r => r.HasOne<Ingridient>().WithMany()
                        .HasForeignKey("IngridientId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ProductIn__Ingri__534D60F1"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__ProductIn__Produ__52593CB8"),
                    j =>
                    {
                        j.HasKey("ProductId", "IngridientId").HasName("PK__ProductI__1577C813E634878E");
                        j.ToTable("ProductIngridients");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
