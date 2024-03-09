using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class ProductSubBundle
{
    public int SubBundleId { get; set; }

    public string SubBundleName { get; set; } = null!;

    public int? MaxUnitsToBuild { get; set; }

    public int? BundleId { get; set; }

    public int? ParentSubBundleId { get; set; }

    public virtual ProductBundle? Bundle { get; set; }

    public virtual ICollection<ProductSubBundle> InverseParentSubBundle { get; set; } = new List<ProductSubBundle>();

    public virtual ProductSubBundle? ParentSubBundle { get; set; }

    public virtual ICollection<ProductSparePart> ProductSpareParts { get; set; } = new List<ProductSparePart>();
}
