using LibraryApp.Application.Interfaces;
using LibraryApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryApp.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorApplicationService, AuthorApplicationService>();
        services.AddScoped<ICategoryApplicationService, CategoryApplicationService>();
        services.AddScoped<IBorrowRecordApplicationService, BorrowRecordApplicationService>();
        services.AddScoped<IMemberApplicationService, MemberApplicationService>();

        return services;
    }
}
