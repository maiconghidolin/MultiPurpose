using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MultiPurposeProject.Authorization;
using MultiPurposeProject.Helpers;
using MultiPurposeProject.Services;
using System;
using System.Data.Common;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;

    
    services.AddDbContext<DataContext>((container, options) =>
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        options.UseNpgsql(connectionString);
    });

    services.AddCors();

    services.AddControllers();

    // configure automapper with all automapper profiles from this assembly
    services.AddAutoMapper(typeof(Program));

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDbConnection"));

    // configure DI for application services

    services.AddSingleton<IMongoDBSettings>(sp => sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);
    
    services.AddSingleton<IMongoClient>(s => new MongoClient(builder.Configuration.GetValue<string>("MongoDbConnection:ConnectionString")));

    services.AddScoped<IJwtUtils, JwtUtils>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IProductService, ProductService>();
}


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Multi Purpose API",
        Description = "An Web API where I will do a lot of stuff to learn"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    using (var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>())
    {

        try
        {
            if (dataContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                dataContext.Database.Migrate();

        }
        catch (Exception ex)
        {
            //Log errors or do anything you think it's needed
            throw;
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}

app.Run();

public partial class Program { }