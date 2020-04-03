using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Voting.API
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        //public static void Main(string[] args)
        //{
        //    var host = new HostBuilder()
        //        .UseContentRoot(Directory.GetCurrentDirectory())
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseKestrel(serverOptions =>
        //            {
        //        // Set properties and call methods on options
        //    })
        //            .UseIISIntegration()
        //            .UseStartup<Startup>();
        //        })
        //        .Build();

        //    host.Run();
        //}
    }
}