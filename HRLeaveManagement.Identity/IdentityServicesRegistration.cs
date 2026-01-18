using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Models.Identity;
using HRLeaveManagement.Identity.DbContext;
using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Identity
{
    public static class IdentityServicesRegistration
    {
        public static IServiceCollection AddConfigureIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add JWT
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            // Add DbContext
            services.AddDbContext<HRLeaveManagementIdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("HRDatabaseConnectionString")));

            // Add Identitity
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<HRLeaveManagementIdentityDbContext>().AddDefaultTokenProviders();

            // Auth / User Services. New instance each time
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();

            // Add Authentication Options
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // "bearer"
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // "bearer"
            })
            // Add Rules
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // Must have valid key
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    // By default, ASP will give you extra 5 minutes of invalidation of tokens livetime
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                };
            });

            return services;
        }
    }
}
