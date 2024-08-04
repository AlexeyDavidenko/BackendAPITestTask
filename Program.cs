using BackendAPIDevelopmentTask.Data;
using BackendAPIDevelopmentTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SimpleAuthentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("customSettings.json", optional: false, reloadOnChange: false);
builder.Services.Configure<AppCustomSettings>(builder.Configuration.GetSection("AppCustomSettings"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// ...
// Registers authentication schemes and services using IConfiguration information (see above).
builder.Services.AddSimpleAuthentication(builder.Configuration);

builder.Services.AddSwaggerGen(options =>
{
    // ...
    // Add this line to integrate authentication with Swagger.
    options.AddSimpleAuthentication(builder.Configuration);
});

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});
builder.Services.AddSingleton<AppCustomSettings>();
builder.Services.AddMvcCore();
var app = builder.Build();
new AppCustomSettings(app.Configuration);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//...
// The following middlewares aren't strictly necessary in .NET 7.0, because they are automatically
// added when detecting that the corresponding services have been registered. However, you may
// need to call them explicitly if the default middlewares configuration is not correct for your
// app, for example when you need to use CORS.
// Check https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/middleware
// for more information.
//app.UseAuthentication();
//app.UseAuthorization();
app.UseAuthenticationAndAuthorization();
app.MapControllers();

app.Run();
