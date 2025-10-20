using LibraryApp.Data;
using LibraryApp.Application;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataServices(builder.Configuration);

builder.Services.AddApplicationServices();


builder.Services.AddControllers();
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Library Management API",
            Version = "v1",
            Description = "K�t�phane y�netim sistemi REST API - Clean Architecture ile geli�tirilmi�tir",
            Contact = new()
            {
                Name = "Library App Team",
                Email = "support@libraryapp.com"
            }
        };
        return Task.CompletedTask;
    });
});

// CORS yap�land�rmas� (Frontend'den eri�im i�in)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options
        .WithTitle("Library Management Api")
        .WithTheme(ScalarTheme.Purple)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
       
    });
}

app.UseHttpsRedirection();

// CORS middleware
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
