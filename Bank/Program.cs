global using Microsoft.AspNetCore.Authorization;

using Bank.Models;
using Bank.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
// Add Bank Db to the container.
builder.Services.AddDbContext<JAFU20BankContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity User
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<JAFU20BankContext>(); 
//builder.Services.AddDbContext<JAFU20BankContext>(options =>
//     options.UseSqlServer("DefaultConnection"));

// Add our own Services to the container
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ICountryDataService, CountryDataService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<DataInitializer>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Response caching
builder.Services.AddResponseCaching();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<DataInitializer>().SeedData();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Response caching
app.UseResponseCaching();

app.MapRazorPages();

app.Run();
