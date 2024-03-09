
using InventoryManagement;
using InventoryManagement.Interface;
using InventoryManagement.Models;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;

public class Program
{

    public static async Task<int> Main(string[] args)
    {
        ServiceProvider serviceProvider = await ContainerConfig.GetServiceCollection().ConfigureAwait(false);
        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
        IWorkFlow workFlow = serviceProvider.GetRequiredService<IWorkFlow>();
        InventoryManagementContext context = serviceProvider.GetRequiredService<InventoryManagementContext>();

        int result = workFlow.MainMenu();

        while (result != 0)
        {
            result = workFlow.MainMenu();
        }
        return result;
    }

    


}


