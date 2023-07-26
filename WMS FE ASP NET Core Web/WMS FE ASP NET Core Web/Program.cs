using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using WMS_FE_ASP_NET_Core_Web.Services;

namespace WMS_FE_ASP_NET_Core_Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            /*
            builder.Services.AddHttpClient<ApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7272/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            */
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            builder.Services.AddSingleton<ApiClient>(new ApiClient("https://localhost:7272/", loggerFactory.CreateLogger<ApiClient>()) );


            builder.Services.AddScoped<WMSApiService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}