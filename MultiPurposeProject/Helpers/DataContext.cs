namespace MultiPurposeProject.Helpers;

using Microsoft.EntityFrameworkCore;
using MultiPurposeProject.Entities;
using System.Collections.Generic;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        options.UseNpgsql(connectionString);

    }

    public DbSet<User> Users { get; set; }

}