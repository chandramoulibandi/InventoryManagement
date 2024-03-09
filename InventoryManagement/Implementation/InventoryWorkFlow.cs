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
                List<ProductBundle> productBundles = _dbContext.ProductBundles.Where(p=>p.ParentBundleId==bundle.BundleId).ToList();
                foreach(ProductBundle productBundle in productBundles)
                {
                    int MaxValue = 0;
                    List<ProductSparePart> productSpareParts = _dbContext.ProductSpareParts.Where(p => p.BundleId == productBundle.BundleId).ToList();

                    int MinValue = productSpareParts.Min(p => p.TotalInventoty).Value;
                    //foreach(ProductSparePart productSparePart in productSpareParts)
                    //{

                    //}
                }
            }
            Console.WriteLine("Recalculated Availabiliy");
            ShowSummary();
        }

        public void ShowSummary()
        {
            List<ProductBundle> bundles = _dbContext.ProductBundles.Where(p => p.IsFinishedProduct == true).ToList();
            foreach (ProductBundle bundle in bundles)
            {
                Console.WriteLine("No of Product can be Created" + bundle.MaxUnitsToBuild);
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
                    //save the finishible product
                }

            }
            else
            {
                Console.WriteLine("Enter the  Name");
                string name = Console.ReadLine() ?? string.Empty;
                Console.WriteLine("Select Parent Node");

                List<ProductBundle> bundles = _dbContext.ProductBundles.ToList();
                foreach (ProductBundle bundle in bundles)
                {
                    Console.WriteLine(bundle.BundleId + ")" + bundle.MaxUnitsToBuild);
                }

                int selectedParentNode = Convert.ToInt32(Console.ReadLine());
                ProductBundle? parentBundle = bundles.Where(p => p.BundleId == selectedParentNode).FirstOrDefault();
                Console.WriteLine("Enter the Units in Bundle");
                int unitsInBundle = Convert.ToInt32(Console.ReadLine());

                bool IsBunbleProduct = IsBunbleOrSpareNode();
                if(IsBunbleProduct) 
                {
                    ProductBundle productBundle = new ProductBundle()
                    {
                        BundleName = name,
                        UnitsInBundle = 1,
                        IsFinishedProduct = true,
                        MaxUnitsToBuild = 0,
                        ParentBundleId = parentBundle?.BundleId
                    };
                    _dbContext.ProductBundles.Add(productBundle);
                    _dbContext.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Enter the Total Inventory");

                    int Inventory = Convert.ToInt32(Console.ReadLine());
                    //save the Sparepart Node
                    ProductSparePart productSparePart = new ProductSparePart()
                    {
                        SparePartName= name,
                        TotalInventoty=Inventory,
                        BalanceInventory=0,
                        BundleId= parentBundle?.BundleId
                    };
                }
            }

        }

        private bool IsBunbleOrSpareNode()
        {
            Console.WriteLine("Select 1 for Bundle or 2 for Spare Item");
            
            switch(Console.ReadLine()) 
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

    }
}
