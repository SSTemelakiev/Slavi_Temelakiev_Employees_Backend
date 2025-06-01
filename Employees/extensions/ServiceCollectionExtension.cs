using Employees.Services;

namespace Employees.extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
        services.AddOpenApi();
        services.AddControllers();
        services.AddScoped<IEmployeesService, EmployeesService>();
        
        return services;
    }
}