using System.Text;
using System.Text.Json.Serialization;
using AutoMapper;
using BLL.Configurations;
using BLL.Interfaces.Identity;
using BLL.Interfaces.Movie;
using BLL.Interfaces.Reservation;
using BLL.Services;
using BLL.Services.Movie;
using BLL.Services.Reservation;
using DAL.EF;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Movie;
using Repository.Reservation;
using Repository.Service;

namespace API.Configurations;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }); ;
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
        
            // options.MapType<ProjectSortByEnum>(() => new OpenApiSchema
            // {
            //     Type = "string",
            //     Enum = Enum.GetNames(typeof(ProjectSortByEnum))
            //         .Select(name => (IOpenApiAny)new OpenApiString(name))
            //         .ToList()
            // });
        
        });

        // Database Context Configuration
        services.AddDbContext<AppDbContext>
            (options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        
        
        // Add Dependencies
        services.AddDependencyInjection()
            .RegisterJWTAuth(configuration)
            .AddAutoMapperConfiguration()
            .AddCorsConfiguration();

        return services;
    }

    public static IServiceCollection RegisterJWTAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 12;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredUniqueChars = 4;
            options.User.RequireUniqueEmail = true;
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        var jwtSettings = configuration.GetSection("Jwt");
        var keyValue = jwtSettings["Key"];
        if (string.IsNullOrWhiteSpace(keyValue) || Encoding.UTF8.GetByteCount(keyValue) < 32)
            throw new InvalidOperationException(
                "Jwt:Key is not configured or is shorter than 32 bytes. " +
                "Set it via user-secrets (dotnet user-secrets set \"Jwt:Key\" \"...\") or the Jwt__Key env var.");
        var key = Encoding.UTF8.GetBytes(keyValue);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        
        return services;
    }
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddTransient<AuthService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<MovieRepository>();
        services.AddScoped<ITheaterService, TheaterService>();
        services.AddScoped<TheaterRepository>();
        services.AddScoped<IScreenService, ScreenService>();
        services.AddScoped<ScreenRepository>();
        services.AddScoped<IScreeningService, ScreeningService>();
        services.AddScoped<ScreeningRepository>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<ReservationRepository>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IFeaturedMovieService, FeaturedMovieService>();
        services.AddScoped<FeaturedMovieRepository>();
        
        return services;
    }
    
    public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
    {
        services.AddSingleton(_ => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile());
        }).CreateMapper());

        return services;
    }
    
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp", policy =>
                policy.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });
        return services;
    }
}