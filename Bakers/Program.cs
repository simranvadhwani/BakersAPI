using Bakers.ContextClass;
using Bakers.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace Bakers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            //CORS Configuration..
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactAppPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                              .AllowCredentials();
                    });
            });
            // For Entity Framework
            builder.Services.AddDbContext<BakersDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

            // For Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BakersDbContext>()
                .AddDefaultTokenProviders();

            // Adding Authentication
            builder.Services.AddAuthentication(options =>
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
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };
            });
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
             {
                 c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                 {
                     In = ParameterLocation.Header,
                     Description = "Please enter JWT token in the field",
                     Name = "Authorization",
                     Type = SecuritySchemeType.ApiKey,
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
                            Array.Empty<string>()
                     }
                 });
             });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.OAuthClientId("swagger");
                    c.OAuthAppName("Swagger UI");
                });
            }

            app.UseHttpsRedirection();

            app.UseCors("ReactAppPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}