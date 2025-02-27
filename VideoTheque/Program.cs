using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using VideoTheque.Businesses.AgeRating;
using VideoTheque.Businesses.Emprunt;
using VideoTheque.Businesses.Film;
using VideoTheque.Businesses.Genres;
using VideoTheque.Businesses.Host;
using VideoTheque.Businesses.Personne;
using VideoTheque.Businesses.Support;
using VideoTheque.Context;
using VideoTheque.Core;
using VideoTheque.Repositories.AgeRating;
using VideoTheque.Repositories.Emprunt;
using VideoTheque.Repositories.Film;
using VideoTheque.Repositories.Genres;
using VideoTheque.Repositories.Host;
using VideoTheque.Repositories.PersonneRepository;
using VideoTheque.Repositories.Support;
using VideoTheque.Configurations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Videotheque") ?? "Data Source=Videotheque.db";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Optional: log to console
    .WriteTo.File("Logs/BluRayLogs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Replace default logger with Serilog
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddSqlite<VideothequeDb>(connectionString);

builder.Services.AddScoped(typeof(IGenresRepository), typeof(GenresRepository));
builder.Services.AddScoped(typeof(IGenresBusiness), typeof(GenresBusiness));

builder.Services.AddScoped(typeof(IAgeRatingRepository), typeof(AgeRatingRepository));
builder.Services.AddScoped(typeof(IAgeRatingBusiness), typeof(AgeRatingBusiness));

builder.Services.AddScoped(typeof(IPersonneRepository), typeof(PersonneRepository));
builder.Services.AddScoped(typeof(IPersonneBusiness), typeof(PersonneBusiness));

builder.Services.AddScoped(typeof(IBluRayRepository), typeof(BluRayRepository));
builder.Services.AddScoped(typeof(IBluRayBusiness), typeof(BluRayBusiness));

builder.Services.AddScoped(typeof(ISupportRepository), typeof(SupportRepository));
builder.Services.AddScoped(typeof(ISupportBusiness), typeof(SupportBusiness));

builder.Services.AddScoped(typeof(IHostRepository), typeof(HostRepository));
builder.Services.AddScoped(typeof(IHostBusiness), typeof(HostBusiness));

builder.Services.AddScoped(typeof(IEmpruntRepository), typeof(EmpruntRepository));
builder.Services.AddScoped(typeof(IEmpruntBusiness), typeof(EmpruntBusiness));

MapsterConfig.RegisterMappings();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Vid�oTh�que API",
        Description = "Gestion de sa collection de film.",
        Version = "v1"
    });
});

builder.Services.AddCors(option => option
    .AddDefaultPolicy(builder => builder
        .SetIsOriginAllowed(_=> true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vid�oTh�que API V1");
    });
//}

app.UseRouting();

//app.UseCors(builder => builder
//    .SetIsOriginAllowed(_ => true)
//    .AllowAnyMethod()
//    .AllowAnyHeader()
//    .AllowCredentials()
//    );

app.UseCors();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
