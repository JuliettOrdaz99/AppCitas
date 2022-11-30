<<<<<<< HEAD
using AppCitas.Service.Extensions;
using AppCitas.Service.Middleware;
using Microsoft.OpenApi.Models;

namespace AppCitas;

public class Startup
{
    private readonly IConfiguration _config;

=======
using AppCitas.Service.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace AppCitas;
public class Startup
{
    private readonly IConfiguration _config;
>>>>>>> dcf52db73f10a35af4078c97974caf8cdc00ee1f
    public Startup(IConfiguration config)
    {
        _config = config;
    }
<<<<<<< HEAD

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplicationServices(_config);
        services.AddControllers();
        services.AddCors();
        services.AddIdentityServices(_config);
=======
    public IConfiguration Configuration { get; }
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(
                _config.GetConnectionString("DefaultConnection")
            );
        });
        services.AddControllers();
        services.AddCors();
>>>>>>> dcf52db73f10a35af4078c97974caf8cdc00ee1f

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
        });
    }
<<<<<<< HEAD

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionMiddleware>();

=======
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
        }
>>>>>>> dcf52db73f10a35af4078c97974caf8cdc00ee1f
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors(p => p.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

<<<<<<< HEAD
        app.UseAuthentication();

=======
>>>>>>> dcf52db73f10a35af4078c97974caf8cdc00ee1f
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> dcf52db73f10a35af4078c97974caf8cdc00ee1f
