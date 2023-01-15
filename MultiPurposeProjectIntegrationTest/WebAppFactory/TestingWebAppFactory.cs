using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MultiPurposeProject.Helpers;
using Microsoft.Extensions.DependencyInjection;

public class TestingWebAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<DataContext>));
           
            // Remove the NpgSql context
            if (descriptor != null)
                services.Remove(descriptor);

            // Add a Inmemory context
            services.AddDbContext<DataContext>((container, options) =>
            {
                //options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                options.UseInMemoryDatabase("Testing");
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            using (var appContext = scope.ServiceProvider.GetRequiredService<DataContext>())
            {
                try
                {
                    appContext.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    //Log errors or do anything you think it's needed
                    throw;
                }
            }
        });
    }
}