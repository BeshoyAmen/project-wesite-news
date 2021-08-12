using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using projects.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using projects.Models;
using projects.Models.sub;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Mvc;
using projects.Models.DataApplicationIdentity;
using Microsoft.AspNetCore.Identity.UI.Services;
using projects.Models.User.admin;

namespace projects
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
            services.AddDbContext<DataCountext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
              .AddDefaultTokenProviders()
              .AddDefaultUI()
                .AddEntityFrameworkStores<DataCountext>();
            services.AddControllersWithViews();
           services.AddRazorPages();
                services.AddSingleton<IEmailSender,EmailSender>();
                services.AddMvc();

        services.AddAuthentication().AddFacebook(options=>{
            options.AppId="743330282990811";
            options.AppSecret="74e61745287c5aa24b8ff426550a7be4";
        });
           services.AddRazorPages().AddRazorRuntimeCompilation();
           services.AddScoped(typeof(IunitedRepoistery<>),typeof(UnitedRepositery<>));
           services.AddScoped(typeof(Isubject),typeof(sub));
           services.AddScoped<Iadmainstrator,admainstrator>();
            // services.AddMvc();

// send email
services.ConfigureApplicationCookie(o => {
    o.ExpireTimeSpan = TimeSpan.FromDays(5);
    o.SlidingExpiration = true;
});
    services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromHours(3));



services.AddMvc()
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Add("/{0}.cshtml");
    })
    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            // services.AddMvc();
          services.AddSession(
               options => {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
          //services.AddPaging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,Iadmainstrator dbInt)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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
            app.UseSession();
            dbInt.Inilations();
            app.UseAuthentication();
            app.UseAuthorization();

        // app.UseMvc(routes =>
        //     {
                
        //           routes.MapRoute(
        //             name: "default",
        //             template: "{controller=Home}/{action=Index}/{Id?}");
        //     });
        app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
