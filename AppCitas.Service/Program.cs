<<<<<<< HEAD
using AppCitas.Service.Data;
using Microsoft.EntityFrameworkCore;

=======
>>>>>>> dcf52db73f10a35af4078c97974caf8cdc00ee1f
namespace AppCitas;

public class Program
{
<<<<<<< HEAD
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync();
            await Seed.SeedUsers(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred during migration/seeding");
        }

        await host.RunAsync();
=======
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
>>>>>>> dcf52db73f10a35af4078c97974caf8cdc00ee1f
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
<<<<<<< HEAD
}
=======
}
>>>>>>> dcf52db73f10a35af4078c97974caf8cdc00ee1f
