﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Voting.Model.Exceptions;

namespace Voting.Infrastructure.MiddleWares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BlockChainException> _logger;

        public ExceptionHandler(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<BlockChainException>() ??
                      throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning(
                        "The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                if (ex is BlockChainException)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = (int) ((BlockChainException) ex).StatusCode;
                    context.Response.ContentType = ((BlockChainException) ex).ContentType;
                    if (ex is UnauthorizedException)
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        ((BlockChainException) ex).StatusCode = HttpStatusCode.Unauthorized;
                    }
                }
                else
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                }

                string result = JsonConvert.SerializeObject(ex);

                await context.Response.WriteAsync(result);
                return;
            }
        }
    }

    public static class ExceptionHandlerMiddleware
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<ExceptionHandler>();
        }
    }
}