using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using Talabat.Api.Errors;
using Talabat.Api.Extensions;
using Talabat.Api.Helpers;
using Talabat.Api.MiddleWares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;

using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
//identity
builder.Services.AddDbContext<IdentityContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Identity"));
});

// service identity
builder.Services.AddIdentityService(builder.Configuration);

// Extension for services(repository,automapper,validation error)
builder.Services.AddApplicationServices();

//redis
builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
{
    var connection=builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(connection);
});
builder.Services.AddScoped(typeof(IBasketRepository),typeof(BasketRepository));
builder.Services.AddCors(option=>
{
    option.AddPolicy("mypolicy", option =>
{
    option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});
    });


// update database
var app = builder.Build();
var scope=app.Services.CreateScope();
var services=scope.ServiceProvider;
var LoggerFactory=services.GetRequiredService<ILoggerFactory>();
try
{
    var dbcontext = services.GetRequiredService<StoreContext>();
    await dbcontext.Database.MigrateAsync();//Update database
    
    await StoreContextSeed.SeedAsync(dbcontext);

    var identitydbcontext = services.GetRequiredService<IdentityContext>();
    await identitydbcontext.Database.MigrateAsync();//Update database
    var usermanager = services.GetRequiredService<UserManager<AppUser>>();
    await AppIdentitySeed.UserSeedAsync(usermanager);
    var rolemanager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await AppIdentitySeed.RoleSeedAsync(rolemanager);
}
catch (Exception ex)
{
    var Logger = LoggerFactory.CreateLogger<Program>();
    Logger.LogError(ex, "an error occurred during migration");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// middle ware of handling server error
app.UseMiddleware<ExceptionMiddleWare>();

//middle ware for redirection
app.UseStatusCodePagesWithRedirects("errors/{0}");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("mypolicy");
app.MapControllers();

app.Run();
