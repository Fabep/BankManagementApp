using AutoMapper;
using InfrastructureLibrary.Infrastructure.AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols;
using ModelLibrary.Models;
using ServiceLibrary.Services;
using SuspiciousMoney;



IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) => services
    .AddDbContext<BankAppDataContext>(options => options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection"))));

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile<AutoMapperProfile>();
});

IMapper mapper = mappingConfig.CreateMapper();

hostBuilder.ConfigureServices(services =>
{
    services.AddSingleton(mapper);
    services.AddTransient<ICustomerService, CustomerService>();
    services.AddTransient<ICountryService, CountryService>();
    services.AddTransient<IAccountService, AccountService>();
    services.AddTransient<ITransactionService, TransactionService>();
    services.AddTransient<Application>();
});

var app = hostBuilder.Build();
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<Application>().Run();
}
app.Run();
