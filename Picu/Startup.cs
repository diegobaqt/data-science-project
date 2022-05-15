using System;
using DataAccess.Base;
using DataAccess.Base.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Picu.Repositories;
using Picu.Repositories.Interfaces;
using Picu.Services;
using Picu.Services.Context;
using Picu.Services.Interfaces;

namespace Picu
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PicuContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(120)));

            services.AddDbContext<BaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Add Context for Unit of Work and BarbershopContext
            services.AddTransient<IPicuService, PicuService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<BaseContext, PicuContext>();

            // Services
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IVitalSignsService, VitalSignsService>();
            services.AddTransient<IMongoDbService, MongoDbService>();

            // Repositories
            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IVitalSignsRepository, VitalSignsRepository>();

            // Add httpClient
            services.AddHttpClient();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PICU",
                    Version = "v1",
                    Description = "A academic project",
                    Contact = new OpenApiContact
                    {
                        Name = "Diego Andrés Baquero",
                        Email = "dabaquerot@unal.edu.co"
                    },
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
                if (serviceScope != null)
                {
                    new PicuInitializer().Initialize(serviceScope.ServiceProvider.GetService<PicuContext>());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Picu v1"));

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
