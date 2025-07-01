using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ViharaFund.Application.Contracts;
using ViharaFund.WebAPI.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddFOFWebAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ITenantService, TenantService>();

            // Configure Swagger with JWT support
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Multi-Tenant API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityDefinition("TenantId", new OpenApiSecurityScheme
                {
                    Description = "Tenant ID header. Example: \"X-Tenant-ID: tenant1\"",
                    Name = "X-Tenant-ID",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        },
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "TenantId"
                                }
                            },
                            new string[] {}
                        }
                });
            });

            // Configure JWT Authentication
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = long.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
                o.ValueCountLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddMvc(options =>
            {
                options.MaxModelBindingCollectionSize = int.MaxValue;
            });

            //var allowedOrigins = new List<string>();

            //var allowOrigins = configuration.GetValue<string>("AllowedOrigins")
            //    .Split(",");

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: "CorsPolicy",
            //              builder => builder.WithOrigins(allowOrigins)
            //              .AllowAnyMethod()
            //              .AllowAnyHeader()
            //              .WithExposedHeaders("Content-Disposition")
            //              .SetIsOriginAllowed((x) => true)
            //              .AllowCredentials()
            //             );
            //});

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
