using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.MappingProfiles;
using HRLeaveManagement.BlazorUI.Services;
using HRLeaveManagement.BlazorUI.Services.Base;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;

namespace HRLeaveManagement.BlazorUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // Custom services
            builder.Services.AddHttpClient<IClient, Client>(client => client.BaseAddress = new Uri("https://localhost:7079"));
            
            builder.Services.AddScoped<ILeaveTypeService, LeaveTypeService>();
            builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
            builder.Services.AddScoped<ILeaveAllocationService, LeaveAllocationService>();

            // AutoMapper
            builder.Services.AddAutoMapper(cfg => { }, typeof(MappingConfig));



            await builder.Build().RunAsync();
        }
    }
}
