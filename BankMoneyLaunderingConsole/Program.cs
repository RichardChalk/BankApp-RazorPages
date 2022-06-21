using Bank.Models;
using Bank.Services;
using BankMoneyLaunderingConsole;
using BankMoneyLaunderingConsole.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
        services

        // Add Bank Db and Services to the container.
        .AddDbContext<JAFU20BankContext>(options =>
        options.UseSqlServer(
        context.Configuration.GetConnectionString("DefaultConnection")))
            .AddTransient<ICustomerService, CustomerService>()
            .AddTransient<IAccountService, AccountService>()
            .AddTransient<ICountryDataService, CountryDataService>()
            .AddTransient<ILaunderingService, LaunderingService>()
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddTransient<Application>())
    .Build();




using (var scope = host.Services.CreateScope())
{
    scope.ServiceProvider.GetService<Application>().Run();
}

host.Run();





