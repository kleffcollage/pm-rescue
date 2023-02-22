using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PropertyMataaz.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace PropertyMataaz
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.ConfigureSeriLog();
            Logger.Info("Logging works now");
            CreateHostBuilder(args).Build().Run();


        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseWebRoot(Directory.GetCurrentDirectory())
                        .UseStartup<Startup>()
                        .UseSerilog();
                });
    }
}
