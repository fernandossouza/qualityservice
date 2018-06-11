using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using qualityservice.Data;
using qualityservice.Service;
using qualityservice.Service.Interface;
using securityfilter.Services;
using securityfilter.Services.Interfaces;

namespace qualityservice {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddCors (o => o.AddPolicy ("CorsPolicy", builder => {
                builder.AllowAnyOrigin ()
                    .AllowAnyMethod ()
                    .AllowAnyHeader ();
            }));
            services.AddSingleton<IConfiguration> (Configuration);
            services.AddTransient<IEncryptService, EncryptService> ();
                      
            if (!String.IsNullOrEmpty (Configuration["KeyFolder"]))
                services.AddDataProtection ()
                .SetApplicationName ("Lorien")
                .PersistKeysToFileSystem (new DirectoryInfo (Configuration["KeyFolder"]));
                
            services.AddDbContext<ApplicationDbContext> (options =>
                options.UseNpgsql (Configuration.GetConnectionString ("ProductionOrderQualityDb")));
            services.AddTransient<IProductionOrderQualityService, ProductionOrderQualityService> ();
            services.AddTransient<IAnalysisService, AnalysisService> ();
            services.AddTransient<ICalculateAnalysisService, CalculateAnalysisService> ();
            services.AddTransient<IProductionOrderService,ProductionOrderService>();
            services.AddMvc ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            app.UseCors ("CorsPolicy");
            app.UseForwardedHeaders (new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseMvc ();
        }
    }
}