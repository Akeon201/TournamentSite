using Microsoft.AspNetCore.Identity;
using Azure.Storage.Blobs;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using tourney_app.Data;
using Microsoft.Extensions.DependencyInjection;

public class Program
{

    public static async Task Main(string[] args)
    {



        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Configuration.AddJsonFile("appsettings.json"); // Load configuration from appsettings.json

        var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection") ??
            throw new InvalidOperationException("Connection string 'PostgreSQLConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddRazorPages();

        // Register BlobServiceClient
        // Register BlobServiceClient
        builder.Services.AddSingleton(provider =>
        {
            var config = provider.GetRequiredService<IConfiguration>();
            var storageAccountUri = config["StorageAccount:Uri"];

            if (string.IsNullOrWhiteSpace(storageAccountUri))
            {
                throw new InvalidOperationException("StorageAccount:Uri not found in configuration.");
            }

            var connectionString = config["AzureBlobStorageConnection"];

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("AzureBlobStorageConnection not found in configuration.");
            }

            var blobServiceClient = new BlobServiceClient(connectionString);
            return blobServiceClient;
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthorization();
        app.MapRazorPages();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "Owner", "Captain", "Player", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            }

        }

        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string email = "myff3drvmble@gmail.com";
            string password = "fm?fn5@3gA,K)jJ";


            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser();
                user.UserName = email;
                user.Email = email;


                userManager.CreateAsync(user, password);

                userManager.AddToRoleAsync(user, "Admin");
            }
        }

        app.Run();

    }
}
