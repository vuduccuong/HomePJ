using AutoMapper;
using HomePJ.API.Data;
using HomePJ.API.ParkyMapper;
using HomePJ.API.Repository;
using HomePJ.API.Repository.IRepository;
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HomePJ.API
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
            //SQL
            services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            //End Sql
            //Scope
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            //End scope

            //Automapper
            services.AddAutoMapper(typeof(ParkyMapings));
            //End Automaper

            //Swager
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("ParkyOpenAPI",
                    new OpenApiInfo() {
                        Title = "Parky Open API",
                        Version = "1",
                        Contact = new OpenApiContact() { 
                            Name = "Vũ Đức Cường",
                            Email = "vuduccuong.ck48@gmail.com",
                            Url = new Uri("https://fb.com/vuduc.cuong4")
                        }
                    });
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var fullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                opts.IncludeXmlComments(fullPath);
            });
            //End swager

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/ParkyOpenAPI/swagger.json", "Parky Open API");
                opt.RoutePrefix = "";
            });
            //end swagger
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
