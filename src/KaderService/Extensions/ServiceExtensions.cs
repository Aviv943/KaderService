using System.Text;
using KaderService.Services.Data;
using KaderService.Services.Models;
using KaderService.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace KaderService.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddMyServices(this IServiceCollection services)
        {
            services.AddTransient<PostsService>();
            services.AddTransient<CommentsService>();
            services.AddTransient<GroupsService>();
            services.AddTransient<UsersService>();
        }

        public static void AddMyAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                // Adding Jwt Bearer  
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = config["JWT:ValidAudience"],
                        ValidIssuer = config["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]))
                    };
                });
        }

        public static void AddMyAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("GroupManager", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new GroupManagerRequirement());
                });

                options.AddPolicy("GroupMember", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new GroupMemberRequirement());
                });
            });

            services.AddScoped<IAuthorizationHandler, GroupManagerHandler>();
            services.AddScoped<IAuthorizationHandler, GroupMemberHandler>();
            services.AddHttpContextAccessor();
        }

        public static void AddMySwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Kader",
                    Description = "Kader Authorization"
                });

                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        public static void AddMyDb(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<KaderContext>(options =>
                options.UseSqlServer(config.GetConnectionString("KaderContext")));
        }

        public static void AddMyIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<KaderContext>()
                .AddDefaultTokenProviders(); ;
        }
    }
}
