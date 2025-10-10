using LibraryApp.Data.Context;
using LibraryApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryApp.Data;

public static class DataServicesRegistration
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<AuthorRepository>();
        services.AddScoped<BookRepository>();
        services.AddScoped<CategoryRepository>();
        services.AddScoped<MemberRepository>();
        services.AddScoped<BorrowRecordRepository>();

        return services;
    }
}
