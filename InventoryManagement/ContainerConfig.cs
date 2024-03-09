using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models;
using InventoryManagement.Interface;
using InventoryManagement.Implementation;

namespace InventoryManagement
{
    public static class ContainerConfig
    {
        /// <summary>
        /// get the service collection
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public static async Task<ServiceProvider> GetServiceCollection()
        {
            var builder = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            var configuration = builder.Build();
            try
            {
                var connectionstring = configuration.GetConnectionString("sqlserver");
                var serviceProvider = new ServiceCollection()
                    .AddSingleton<IConfiguration>(configuration)
                    .AddSingleton<IWorkFlow, InventoryWorkFlow>()
                    .AddDbContext<InventoryManagementContext>(options =>
                    {
                        options.UseSqlServer(connectionstring);
                    })
                    .BuildServiceProvider();

                return serviceProvider;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in GetServiceCollection", ex);
            }
        }
    }
}
