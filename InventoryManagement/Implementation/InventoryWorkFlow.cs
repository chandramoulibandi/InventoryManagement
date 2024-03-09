using InventoryManagement.Interface;
using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Implementation
{
    public class InventoryWorkFlow : IWorkFlow
    {
        private readonly InventoryManagementContext _dbContext;
        public InventoryWorkFlow(InventoryManagementContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int MainMenu()
        {

            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Create Nodes");
            Console.WriteLine("2) Show Inventory Summary");
            Console.WriteLine("3) Exit");

            switch (Console.ReadLine())
            {
                case "1":
                    UpdateNodes();
                    return 1;
                case "2":
                    ShowInventorySummary();
                    return 1;
                case "3":
                    return 0;
                default:
                    return 0;
            }
        }
        public void ShowInventorySummary()
        {

            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Recalculate Availability");
            Console.WriteLine("2) Show Inventory Summary");

            switch (Console.ReadLine())
            {
                case "1":
                    RecalculateAvailability();
                    return;
                case "2":
                    ShowSummary();
                    return;
                default:
                    return;


            }
        }
        public void RecalculateAvailability()
        {
            List<ProductBundle> bundles = _dbContext.ProductBundles.Where(p => p.IsFinishedProduct).ToList();
            foreach (ProductBundle bundle in bundles)
            {
                List<ProductBundle> productBundles = _dbContext.ProductBundles.Where(p => p.ParentBundleId == bundle.BundleId).ToList();
                int MinValue = 0;
                foreach (ProductBundle productBundle in productBundles)
                {
                    List<ProductSubBundle> productSubBundles = _dbContext.ProductSubBundles.Where(p => p.BundleId == productBundle.BundleId).ToList();

                    foreach (ProductSubBundle productSubBundle in productSubBundles)
                    {
                        ProductSparePart? productSpare = _dbContext.ProductSpareParts.Where(p => p.SubBundleId == productSubBundle.SubBundleId).FirstOrDefault();
                        if (productSpare != null)
                        {
                            if (MinValue != 0) MinValue = (productSpare.TotalInventoty < MinValue) ? productSpare.TotalInventoty ?? 0 : MinValue;
                            else MinValue = productSpare.TotalInventoty ?? 0;
                        }
                    }

                    List<ProductSubBundle> productSubBundles2 = _dbContext.ProductSubBundles.Where(p => p.ParentSubBundleId == productBundle.BundleId).ToList();

                    foreach (ProductSubBundle productSubBundle in productSubBundles2)
                    {
                        ProductSparePart? productSpare = _dbContext.ProductSpareParts.Where(p => p.SubBundleId == productSubBundle.SubBundleId).FirstOrDefault();
                        if (productSpare != null)
                        {
                            if (MinValue != 0) MinValue = (productSpare.TotalInventoty < MinValue) ? productSpare.TotalInventoty ?? 0 : MinValue;
                            else MinValue = productSpare.TotalInventoty ?? 0;
                        }
                    }


                }
                List<int> units = new List<int>();
                foreach (ProductBundle productBundle in productBundles)
                {
                    units.Add(MinValue / productBundle.UnitsInBundle ?? 0);
                }
                int minAvailableUnits = units.Min();
                bundle.MaxUnitsToBuild = minAvailableUnits;
                _dbContext.ProductBundles.Update(bundle);
                _dbContext.SaveChanges();

            }
            Console.WriteLine("Recalculated the Max Units can be build");
            ShowSummary();
        }

        public void ShowSummary()
        {
            List<ProductBundle> bundles = _dbContext.ProductBundles.Where(p => p.IsFinishedProduct == true).ToList();
            foreach (ProductBundle bundle in bundles)
            {
                Console.WriteLine("No of product can be build from available inventory" + bundle.MaxUnitsToBuild);
            }
        }

        public void UpdateNodes()
        {

            bool IsFinishible = IsFinishibleProduct();
            if (IsFinishible)
            {
                Console.WriteLine("Enter the Bundle Name");
                string bundleName = Console.ReadLine() ?? string.Empty;
                if (!string.IsNullOrEmpty(bundleName))
                {
                    ProductBundle productBundle = new ProductBundle()
                    {
                        BundleName = bundleName,
                        UnitsInBundle = 1,
                        IsFinishedProduct = true,
                        MaxUnitsToBuild = 0,
                        ParentBundleId = null
                    };
                    _dbContext.ProductBundles.Add(productBundle);
                    _dbContext.SaveChanges();
                    Console.WriteLine("Node created successfully");
                }

            }
            else
            {
                Console.WriteLine("Enter the  Name");
                string name = Console.ReadLine() ?? string.Empty;


                bool IsBunbleProduct = IsBunbleOrSpareNode();
                if (IsBunbleProduct)
                {
                    bool IsParentNaodeidSubBundleNode = IsParentNaodeidSubBundle();

                    if (IsParentNaodeidSubBundleNode)
                    {
                        Console.WriteLine("Select Parent Node");

                        List<ProductSubBundle> bundles = _dbContext.ProductSubBundles.ToList();
                        foreach (ProductSubBundle bundle in bundles)
                        {
                            Console.WriteLine(bundle.SubBundleId + ")" + bundle.SubBundleName);
                        }
                        int selectedParentNode = Convert.ToInt32(Console.ReadLine());
                        ProductSubBundle? parentBundle = bundles.Where(p => p.SubBundleId == selectedParentNode).FirstOrDefault();
                        ProductSubBundle productSubBundle = new ProductSubBundle()
                        {
                            SubBundleName = name,
                            ParentSubBundleId = selectedParentNode,
                        };
                        _dbContext.ProductSubBundles.Add(productSubBundle);
                    }
                    else
                    {
                        Console.WriteLine("Select Parent Node");

                        List<ProductBundle> bundles = _dbContext.ProductBundles.ToList();
                        foreach (ProductBundle bundle in bundles)
                        {
                            Console.WriteLine(bundle.BundleId + ")" + bundle.BundleName);
                        }

                        int selectedParentNode = Convert.ToInt32(Console.ReadLine());
                        ProductBundle? parentBundle = bundles.Where(p => p.BundleId == selectedParentNode).FirstOrDefault();

                        if (parentBundle != null && parentBundle.IsFinishedProduct)
                        {

                            Console.WriteLine("Enter the Units in Bundle");
                            int unitsInBundle = Convert.ToInt32(Console.ReadLine());


                            ProductBundle productBundle = new ProductBundle()
                            {
                                BundleName = name,
                                UnitsInBundle = unitsInBundle,
                                IsFinishedProduct = false,
                                MaxUnitsToBuild = 0,
                                ParentBundleId = parentBundle?.BundleId
                            };
                            _dbContext.ProductBundles.Add(productBundle);
                        }
                        else
                        {
                            ProductSubBundle productSubBundle = new ProductSubBundle()
                            {
                                SubBundleName = name,
                                BundleId = selectedParentNode,
                            };
                            _dbContext.ProductSubBundles.Add(productSubBundle);

                        }
                    }


                    _dbContext.SaveChanges();
                    Console.WriteLine("Node created successfully");
                }
                else
                {
                    int ParentID = 0;
                    bool IsMainBundle = IsParentNodeFromMainBundle();
                    if (IsMainBundle)
                    {
                        Console.WriteLine("Select Parent Node");

                        List<ProductBundle> bundles = _dbContext.ProductBundles.ToList();
                        foreach (ProductBundle bundle in bundles)
                        {
                            Console.WriteLine(bundle.BundleId + ")" + bundle.BundleName);
                        }

                        ParentID = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter the Total Inventory");

                        int Inventory = Convert.ToInt32(Console.ReadLine());
                        //save the Sparepart Node
                        ProductSparePart productSparePart = new ProductSparePart()
                        {
                            SparePartName = name,
                            TotalInventoty = Inventory,
                            BalanceInventory = 0,
                            BundleId = ParentID
                        };
                        _dbContext.ProductSpareParts.Add(productSparePart);
                    }
                    else
                    {
                        Console.WriteLine("Select Parent Node");
                        List<ProductSubBundle> bundles = _dbContext.ProductSubBundles.ToList();
                        foreach (ProductSubBundle bundle in bundles)
                        {
                            Console.WriteLine(bundle.SubBundleId + ")" + bundle.SubBundleName);
                        }

                        ParentID = Convert.ToInt32(Console.ReadLine());
                        //ProductSubBundle? parentSubBundle = bundles.Where(p => p.BundleId == selectedParentNode).FirstOrDefault();
                        Console.WriteLine("Enter the Total Inventory");

                        int Inventory = Convert.ToInt32(Console.ReadLine());
                        //save the Sparepart Node
                        ProductSparePart productSparePart = new ProductSparePart()
                        {
                            SparePartName = name,
                            TotalInventoty = Inventory,
                            BalanceInventory = 0,
                            SubBundleId = ParentID
                        };
                        _dbContext.ProductSpareParts.Add(productSparePart);
                    }



                }
                _dbContext.SaveChanges();
                Console.WriteLine("Node created successfully");
            }

        }

        private bool IsBunbleOrSpareNode()
        {
            Console.WriteLine("Select 1 for Bundle or 2 for Spare Item");

            switch (Console.ReadLine())
            {
                case "1":
                    return true;
                case "2":
                    return false;

            }
            return false;
        }
        private bool IsFinishibleProduct()
        {
            Console.WriteLine("Select 1 for Finishible Product or 2 for Non Finishible Product");

            switch (Console.ReadLine())
            {
                case "1":
                    return true;
                case "2":
                    return false;

            }
            return false;
        }
        private bool IsParentNaodeidSubBundle()
        {
            Console.WriteLine("Select 1 if parent node is sub bundle or 2 if parent node is main bundle");

            switch (Console.ReadLine())
            {
                case "1":
                    return true;
                case "2":
                    return false;

            }
            return false;
        }
        private bool IsParentNodeFromMainBundle()
        {
            Console.WriteLine("Select 1 if parent node is Main Bundle or 2 if parent node is From Sub Bundle");

            switch (Console.ReadLine())
            {
                case "1":
                    return true;
                case "2":
                    return false;

            }
            return false;
        }

    }
}
