using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace projects
{
    public class Program
    {
        public static void Main(string[] args)
        {
          var webhosting=  CreateHostBuilder(args).Build();
          runmigrations(webhosting);
            webhosting.Run();
        }
        private static void runmigrations(Iwebhosting webhosting)
        {
            using(var scop=webhosting.services.createScope())

            {
                var db=scop.ServiceProvider.GetRequiredService<DataCountext>();
                db.Database.Migrate();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
