using Microsoft.AspNetCore.Identity;
using Azure.Storage.Blobs;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using tourney_app.Data;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json"); // Load configuration from appsettings.json

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection") ??
    throw new InvalidOperationException("Connection string 'PostgreSQLConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
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

app.Run();
