using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class ProductSparePart
{
    public int SparePartId { get; set; }

    public string SparePartName { get; set; } = null!;

    public int? TotalInventoty { get; set; }

    public int? BalanceInventory { get; set; }

    public int? BundleId { get; set; }

    public virtual ProductBundle? Bundle { get; set; }
}
