using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Http.Description;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProdajaMobilnihAplikacija.Model.Requests;
using ProdajaMobilnihAplikacija.WebAPI.Database;
using ProdajaMobilnihAplikacija.WebAPI.Filters;
using ProdajaMobilnihAplikacija.WebAPI.Security;
using ProdajaMobilnihAplikacija.WebAPI.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
//using Swashbuckle.Swagger;
//using IDocumentFilter = Swashbuckle.Swagger.IDocumentFilter;


namespace ProdajaMobilnihAplikacija.WebAPI
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
            services.AddControllers();
            services.AddMvc(x => x.Filters.Add<ErrorFilter>()).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddAutoMapper(typeof(Startup));

            //services.AddAutoMapper();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddAuthentication("BasicAuthentication")
                        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);


            services.AddScoped<IZaposlenikService, ZaposlenikServices>();
            services.AddScoped<ISoftverService, SoftverService>();
            services.AddScoped<IKlijentService, KlijentService>();
            services.AddScoped<ISoftverService, SoftverService>();
            services.AddScoped<IAnalizaService, AnalizaService>();
            services.AddScoped<IRacunService, RacunService>();
            services.AddScoped<IOcjenaService, OcjenaServices>();
            services.AddScoped<IKategorijaService, KategorijaService>();
            services.AddScoped<IGradService, GradService>();
            services.AddScoped<IMojSoftverService, MojSoftverService>();
            services.AddScoped<IRecommenderService, RecommenderService>();


            services.AddScoped<ICRUDService<Model.Fakultet, FakultetSearchRequest, FakultetUpsertRequest, FakultetUpsertRequest>, FakultetService>();
           
           
            services.AddDbContext<Prodaja_Mobilnog_SoftveraContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("Prodaja_Mobilnog_Softvera")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
