using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class ProductBundle
{
    public int BundleId { get; set; }

    public string BundleName { get; set; } = null!;

    public int? UnitsInBundle { get; set; }

    public bool IsFinishedProduct { get; set; }

    public int? MaxUnitsToBuild { get; set; }

    public int? ParentBundleId { get; set; }

    public virtual ICollection<ProductBundle> InverseParentBundle { get; set; } = new List<ProductBundle>();

    public virtual ProductBundle? ParentBundle { get; set; }

    public virtual ICollection<ProductSparePart> ProductSpareParts { get; set; } = new List<ProductSparePart>();

    public virtual ICollection<ProductSubBundle> ProductSubBundles { get; set; } = new List<ProductSubBundle>();
}
