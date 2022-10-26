using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Data.Data;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Business;
using WebApi.Controllers;
using Microsoft.OpenApi.Models;
using Business.Interfaces;
using Business.Services;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var conection = Configuration.GetConnectionString("Market");
            services.AddControllers();
            services.AddDbContext<TradeMarketDbContext>(o => o.UseSqlServer(conection));
            services.AddAutoMapper(typeof(AutomapperProfile));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IStatisticService, StatisticService>();
            services.AddSwaggerGen();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                //https://localhost:5001/swagger/index.html
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
