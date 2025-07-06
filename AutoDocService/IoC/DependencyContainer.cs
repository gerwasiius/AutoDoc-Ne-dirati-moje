using AutoDocService.API.Controllers;
using AutoDocService.API.ServiceInterfaces;
using AutoDocService.BL.Services;
using AutoDocService.DL.Entities;

namespace AutoDocService.IoC
{
    /// <summary>
    /// IOC 
    /// </summary>
    public class DependencyContainer
    {
        /// <summary>
        /// A method with dependency configuration for services and interfaces
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterService(IServiceCollection services)
        {
            services.AddSingleton<ILogService, LogService>();
            services.AddScoped<ISectionGroupService, SectionGroupService>();
            services.AddScoped<ISectionsService, SectionsService>();
            services.AddScoped<IDocumentTemplateService, DocumentTemplateService>();
            services.AddScoped<ITemplateSectionsRelationService, TemplateSectionsRelationService>();
            services.AddScoped<IDocumentRenderService, DocumentRenderService>();
            services.AddScoped<IPlaceholderMetadataService, PlaceholderMetadataService>();
        }

        /// <summary>
        /// Registration of clients
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void RegisterClients(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHttpClient("Log", c =>
            {
                c.BaseAddress = new Uri(Configuration["LogUri"].ToString());
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}
