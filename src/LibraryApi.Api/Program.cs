using LibraryApi.Application.Services;
using LibraryApi.Domain.Repositories;
using LibraryApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Threading.Tasks;

namespace LibraryApi.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // serilog config
            builder.Host.UseSerilog((context, config) =>
            {
                config.WriteTo.Console()
                .WriteTo.File("logs/library-.log", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Information();
            });

            builder.Services.AddControllers().AddNewtonsoftJson(options => 
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            builder.Services.AddDbContext<LibraryContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
                options.LogTo(Log.Logger.Information, LogLevel.Information);
            });
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IBookService, BookService>();

            var app = builder.Build();

            app.UseSerilogRequestLogging();
            app.UseAuthorization();
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
                await context.Database.MigrateAsync();
            }

           app.Run();
        }
    }
}
