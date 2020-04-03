﻿using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Voting.Model.Context;
using Voting.Infrastructure;
using Voting.Infrastructure.AutoMapper;
using Voting.Infrastructure.MiddleWares;
using Voting.Infrastructure.PeerToPeer;
using Voting.Infrastructure.Services;
using Voting.Infrastructure.Services.BlockChainServices;
using Voting.Infrastructure.Services.BlockServices;
using Microsoft.OpenApi.Models;

namespace Voting.API
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public void ConfigureServices(IServiceCollection services)
        {
            string p2p_port = Environment.GetEnvironmentVariable("P2P_PORT") != null
                ? Environment.GetEnvironmentVariable("P2P_PORT")
                : _configuration.GetSection("P2P").GetSection("DEFAULT_PORT").Value;

            string connection = string.Format(_configuration.GetConnectionString("BlockchainContext"), p2p_port);

            services.AddDbContext<BlockchainContext>(opt =>
                opt.UseSqlServer(connection));

            services.AddDbContext<BlockchainCommonContext>(opt =>
                opt.UseSqlServer(_configuration.GetConnectionString("BlockchainCommonContext")));

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
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

            services.AddSingleton(_configuration);

            services.AddAutoMapper(typeof(ElectionProfile).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplicationServices.GetService<P2PNetwork>().InitialNetwrok();


            app.UseCors("BlockChain Policy");

            app.UseExceptionHandlerMiddleware();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();
            app.UseEndpoints(e =>
            {
                e.MapRazorPages();
            });
        }
    }
}