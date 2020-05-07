namespace CSharp.AspNetCoreDemo
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.IO;
    using Telerik.Reporting.Cache.File;
    using Telerik.Reporting.Services;
    using Telerik.Reporting.Services.AspNetCore;
    using Telerik.WebReportDesigner.Services;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region CORS
            services.AddCors(options =>
            {
                options.DefaultPolicyName = "MyPolicy";
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:57615", "http://localhost:4200")
                        .SetIsOriginAllowedToAllowWildcardSubdomains();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                        builder.AllowCredentials();
                    });
            });

            #endregion
            services.AddControllers();
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.AddRazorPages().AddNewtonsoftJson();

            // Configure dependencies for ReportsController.
            services.TryAddSingleton<IReportServiceConfiguration>(sp =>
                new ReportServiceConfiguration
                {
                    ReportingEngineConfiguration = ConfigurationHelper.ResolveConfiguration(sp.GetService<IWebHostEnvironment>()),
                    HostAppId = "ReportingCore3App",
                    Storage = new FileStorage(),
                    ReportResolver = new ReportFileResolver(
                        System.IO.Path.Combine(sp.GetService<IWebHostEnvironment>().ContentRootPath, "Reports"))
                });

            #region OLD CODE
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //// Configure dependencies for ReportsController.
            //services.TryAddSingleton<IReportServiceConfiguration>(sp =>
            //    new ReportServiceConfiguration
            //    {
            //        // The default ReportingEngineConfiguration will be initialized from appsettings.json or appsettings.{EnvironmentName}.json:
            //        ReportingEngineConfiguration = sp.GetService<IConfiguration>(),

            //        // In case the ReportingEngineConfiguration needs to be loaded from a specific configuration file, use the approach below:
            //        // ReportingEngineConfiguration = ResolveSpecificReportingConfiguration(sp.GetService<IHostingEnvironment>()),
            //        HostAppId = "Html5DemoAppCore",
            //        Storage = new FileStorage(),
            //        ReportResolver = new ReportTypeResolver()
            //            .AddFallbackResolver(new ReportFileResolver(
            //                Path.Combine(sp.GetService<IHostingEnvironment>().ContentRootPath, "..", "..", "..", "Report Designer", "Examples")))
            //    });

            //// Configure dependencies for ReportDesignerController.
            //services.TryAddSingleton<IReportDesignerServiceConfiguration>(sp => new ReportDesignerServiceConfiguration
            //{
            //    DefinitionStorage = new FileDefinitionStorage(
            //        Path.Combine(sp.GetService<IHostingEnvironment>().ContentRootPath, "..", "..", "..", "Report Designer", "Examples")),
            //    SettingsStorage = new FileSettingsStorage(
            //        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telerik Reporting"))
            //}); 
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    else
        //    {
        //        // app.UseHsts();
        //    }

        //    // app.UseHttpsRedirection();
        //    app.UseMvc();
        //    app.UseDefaultFiles();
        //    app.UseStaticFiles();
        //}

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("MyPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Loads a reporting configuration from a specific JSON-based configuration file.
        /// </summary>
        /// <param name="environment">The current web hosting environment used to obtain the content root path</param>
        /// <returns>IConfiguration instance used to initialize the Reporting engine</returns>
        //static IConfiguration ResolveSpecificReportingConfiguration(IHostingEnvironment environment)
        //{
        //    // If a specific configuration needs to be passed to the reporting engine, add it through a new IConfiguration instance.
        //    var reportingConfigFileName = System.IO.Path.Combine(environment.ContentRootPath, "reportingAppSettings.json");
        //    return new ConfigurationBuilder()
        //        .AddJsonFile(reportingConfigFileName, true)
        //        .Build();
        //}

        static class ConfigurationHelper
        {
            public static IConfiguration ResolveConfiguration(IWebHostEnvironment environment)
            {
                var reportingConfigFileName = System.IO.Path.Combine(environment.ContentRootPath, "appsettings.json");
                return new ConfigurationBuilder()
                    .AddJsonFile(reportingConfigFileName, true)
                    .Build();
            }
        }
    }
}
