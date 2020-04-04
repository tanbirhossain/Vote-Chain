using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Voting.API
{
    public class Program
    {

        public static void Main(string[] args)
        {

            var webhost = CreateWebHostBuilder(args).Build();

            webhost.Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseUrls(args)
                .UseKestrel()
                .UseStartup<Startup>();
    }
}