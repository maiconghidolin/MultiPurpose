namespace MultiPurposeProject.Helpers;

using Microsoft.EntityFrameworkCore;
using MultiPurposeProject.Entities;
using System.Collections.Generic;

public class DataContext : DbContext
{
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

}