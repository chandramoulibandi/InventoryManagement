using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Models;

public partial class InventoryManagementContext : DbContext
{
    public InventoryManagementContext()
    {
    }

    public InventoryManagementContext(DbContextOptions<InventoryManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ProductBundle> ProductBundles { get; set; }

    public virtual DbSet<ProductSparePart> ProductSpareParts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductBundle>(entity =>
        {
            entity.HasKey(e => e.BundleId).HasName("PK__ProductB__42003BB18EE46CA1");

            entity.ToTable("ProductBundle");

            entity.Property(e => e.BundleId).HasColumnName("BundleID");
            entity.Property(e => e.BundleName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ParentBundleId).HasColumnName("ParentBundleID");

            entity.HasOne(d => d.ParentBundle).WithMany(p => p.InverseParentBundle)
                .HasForeignKey(d => d.ParentBundleId)
                .HasConstraintName("FK__ProductBu__Paren__37A5467C");
        });

        modelBuilder.Entity<ProductSparePart>(entity =>
        {
            entity.HasKey(e => e.SparePartId).HasName("PK__ProductS__F5BA41F2108D8BC7");

            entity.Property(e => e.SparePartId).HasColumnName("SparePartID");
            entity.Property(e => e.BundleId).HasColumnName("BundleID");
            entity.Property(e => e.SparePartName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Bundle).WithMany(p => p.ProductSpareParts)
                .HasForeignKey(d => d.BundleId)
                .HasConstraintName("FK__ProductSp__Bundl__3A81B327");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
