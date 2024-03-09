
using InventoryManagement;
using InventoryManagement.Interface;
using InventoryManagement.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{

    public static async void Main(string[] args)
    {
        ServiceProvider serviceProvider = await ContainerConfig.GetServiceCollection().ConfigureAwait(false);
        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
        IWorkFlow workFlow = serviceProvider.GetRequiredService<IWorkFlow>();
        InventoryManagementContext context = serviceProvider.GetRequiredService<InventoryManagementContext>();

        


    }
    private static bool MainMenu(IWorkFlow workFlow)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Update Nodes");
            Console.WriteLine("2) Show Inventory Summary");

            switch (Console.ReadLine())
            {
                case "1":
                    workFlow.UpdateNodes();
                    return true;
                case "2":
                    workFlow.ShowInventorySummary();
                    return true;
                default:
                    return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}


