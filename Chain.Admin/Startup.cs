using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Voting.Infrastructure;
using Voting.Infrastructure.AutoMapper;
using Voting.Infrastructure.MiddleWares;
using Voting.Infrastructure.PeerToPeer;
using Voting.Infrastructure.Services;
using Voting.Infrastructure.Services.BlockChainServices;
using Voting.Infrastructure.Services.BlockServices;
using Voting.Model.Context;

namespace Chain.Admin
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

            services.AddRazorPages().AddRazorRuntimeCompilation();

            string p2p_port = Environment.GetEnvironmentVariable("P2P_PORT") != null
                ? Environment.GetEnvironmentVariable("P2P_PORT")
                : Configuration.GetSection("P2P").GetSection("DEFAULT_PORT").Value;

            string connection = string.Format(Configuration.GetConnectionString("BlockchainContext"), p2p_port);

            services.AddDbContext<BlockchainContext>(opt =>
                opt.UseSqlServer(connection));

            services.AddDbContext<BlockchainCommonContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("BlockchainCommonContext")));

            services.AddCors(opt =>
            {
                opt.AddPolicy("BlockChain Policy", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Vote Chain API",
                    Description = "© Mohammed Tanbir Hossain",
                    Version = "v1"
                });
            });
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpContextAccessor();
            services.AddScoped<BlockService>();
            services.AddScoped<BlockChainService>();
            services.AddScoped<TransactionPoolService>();
            services.AddScoped<TransactionService>();
            services.AddScoped<WalletService>();
            services.AddScoped<MinerService>();
            services.AddScoped<ProfileService>();
            services.AddScoped<ElectionService>();
            services.AddScoped<VotingService>();

            services.AddSingleton<BlockChain>();
            services.AddSingleton<P2PNetwork>();

            services.AddSingleton(Configuration);

            services.AddAutoMapper(typeof(ElectionProfile).Assembly);
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();


            //app.ApplicationServices.GetService<P2PNetwork>().InitialNetwrok();


            app.UseCors("BlockChain Policy");

            app.UseExceptionHandlerMiddleware();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vote Chain API V1");
            });



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

}
