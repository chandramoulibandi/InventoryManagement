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

    public virtual DbSet<ProductSubBundle> ProductSubBundles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductBundle>(entity =>
        {
            entity.HasKey(e => e.BundleId).HasName("PK__ProductB__42003BB1CE2F50C6");

            entity.ToTable("ProductBundle");

            entity.Property(e => e.BundleId).HasColumnName("BundleID");
            entity.Property(e => e.BundleName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ParentBundleId).HasColumnName("ParentBundleID");

            entity.HasOne(d => d.ParentBundle).WithMany(p => p.InverseParentBundle)
                .HasForeignKey(d => d.ParentBundleId)
                .HasConstraintName("FK__ProductBu__Paren__68487DD7");
        });

        modelBuilder.Entity<ProductSparePart>(entity =>
        {
            entity.HasKey(e => e.SparePartId).HasName("PK__ProductS__F5BA41F2D7180C67");

            entity.Property(e => e.SparePartId).HasColumnName("SparePartID");
            entity.Property(e => e.BundleId).HasColumnName("BundleID");
            entity.Property(e => e.SparePartName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SubBundleId).HasColumnName("SubBundleID");

            entity.HasOne(d => d.Bundle).WithMany(p => p.ProductSpareParts)
                .HasForeignKey(d => d.BundleId)
                .HasConstraintName("FK__ProductSp__Bundl__6EF57B66");

            entity.HasOne(d => d.SubBundle).WithMany(p => p.ProductSpareParts)
                .HasForeignKey(d => d.SubBundleId)
                .HasConstraintName("FK__ProductSp__SubBu__6FE99F9F");
        });

        modelBuilder.Entity<ProductSubBundle>(entity =>
        {
            entity.HasKey(e => e.SubBundleId).HasName("PK__ProductS__84E3B08034AACC3D");

            entity.ToTable("ProductSubBundle");

            entity.Property(e => e.BundleId).HasColumnName("BundleID");
            entity.Property(e => e.SubBundleName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Bundle).WithMany(p => p.ProductSubBundles)
                .HasForeignKey(d => d.BundleId)
                .HasConstraintName("FK__ProductSu__Bundl__6B24EA82");

            entity.HasOne(d => d.ParentSubBundle).WithMany(p => p.InverseParentSubBundle)
                .HasForeignKey(d => d.ParentSubBundleId)
                .HasConstraintName("FK__ProductSu__Paren__6C190EBB");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
