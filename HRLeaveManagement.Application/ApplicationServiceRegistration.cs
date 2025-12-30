using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.MappingProfiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace HRLeaveManagement.Application
{
    public static class ApplicationServiceRegistration
    {
        
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile(new LeaveTypeProfile(), ILoggerFactory<ApplicationServiceRegistration> loggerFactory);
            //});

            services.AddAutoMapper(cfg => { }, typeof(LeaveTypeProfile));

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
