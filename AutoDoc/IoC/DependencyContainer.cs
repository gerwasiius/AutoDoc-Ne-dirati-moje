//using Blazored.Toast;
using AutoDocFront.Auth;
using AutoDocFront.Middlewares;
using AutoDocFront.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AutoDocFront.IoC
{
    public class DependencyContainer
    {
        /// <summary>
        /// A method with dependency configuration for services and interfaces
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterService(IServiceCollection services)
        {
            services.AddTransient<RequestMessageHandler>();
            services.AddScoped<AuthenticationStateProvider, PersistingAuthenticationStateProvider>();
            //services.AddBlazoredToast();
            services.AddFluentUIComponents();
            services.AddScoped<SectionGroupApiService>();
            services.AddScoped<DocumentTemplateApiService>();
            services.AddScoped<SectionsApiService>();
            services.AddScoped<DocumentRenderApiService>();
            services.AddScoped<PlaceholdersApiService>();

        }

        /// <summary>
        /// Registration of clients
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void RegisterClients(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHttpClient("Currency", c =>
            {
                c.BaseAddress = new Uri(Configuration["CurrencyUri"]?.ToString() ?? string.Empty);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddHttpMessageHandler<RequestMessageHandler>();

            services.AddHttpClient("AutoDocService", c =>
            {
                c.BaseAddress = new Uri(Configuration["AutoDocServiceUri"]?.ToString() ?? string.Empty);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddHttpMessageHandler<RequestMessageHandler>();

            services.AddHttpClient("PdfService", c =>
            {
                c.BaseAddress = new Uri(Configuration["PdfUri"]?.ToString() ?? string.Empty);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddHttpMessageHandler<RequestMessageHandler>();
        }
    }
}
