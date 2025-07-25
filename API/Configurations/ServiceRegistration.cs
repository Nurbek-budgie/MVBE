using System.Text;
using System.Text.Json.Serialization;
using AutoMapper;
using BLL.Configurations;
using BLL.Interfaces.Identity;
using BLL.Services;
using DAL.EF;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredUniqueChars = 0;
            options.User.RequireUniqueEmail = true;
            // Customize as needed
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
        
        var jwtSettings = configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
        
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
                policy.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });
        return services;
    }
}