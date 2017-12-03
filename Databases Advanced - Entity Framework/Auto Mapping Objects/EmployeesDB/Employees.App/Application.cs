namespace Employees.App
{
    using System;
    using AutoMapper;    
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Employees.Data;    
    using Employees.Services;
    using Employees.Data.Configurations;
    using Employees.App.Core;

    public class Application
    {
        public static void Main()
        {
            var serviceProdiver = ConfigureServices();
            var engine = new Engine(serviceProdiver);
            engine.Run();
        }

        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<EmployeesContext>(opt => 
            opt.UseSqlServer(Configuration.ConnectionString));

            serviceCollection.AddTransient<EmployeeService>();

            serviceCollection.AddAutoMapper(cfg => cfg.AddProfile<AutomapperProfile>());

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}