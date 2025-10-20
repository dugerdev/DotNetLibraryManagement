using LibraryApp.Data.Context;
using LibraryApp.Data.Repositories;
using LibraryApp.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryApp.Data;

public static class DataServicesRegistration
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext'i kaydet
        services.AddDbContext<LibraryDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // UnitOfWork'ü kaydet
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repository'leri Interface ile kaydet
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IBorrowRecordRepository, BorrowRecordRepository>();

        // Domain Service'leri kaydet
        services.AddScoped<IBorrowingService, BorrowingService>();
        services.AddScoped<IBookAvailabilityService, BookAvailabilityService>();

        return services;
    }
}
