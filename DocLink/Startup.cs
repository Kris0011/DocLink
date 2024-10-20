using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using DocLink.Repository;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DocLink
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

        


            services.AddAuthentication("HospitalCookies")
                .AddCookie("HospitalCookies", options =>
                {
                    options.LoginPath = "/Hospital/Login"; 
                    options.AccessDeniedPath = "/Home/AccessDenied"; 
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                });
            services.AddAuthentication("PatientCookies")
               .AddCookie("PatientCookies", options =>
               {
                   options.LoginPath = "/Patient/Login";
                   options.AccessDeniedPath = "/Home/AccessDenied";
                   options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
               });
            
            services.AddAuthentication("DoctorCookies")
               .AddCookie("DoctorCookies", options =>
               {
                   options.LoginPath = "/Doctor/Login";
                   options.AccessDeniedPath = "/Home/AccessDenied";
                   options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
               });

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddRazorPages();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<IHospitalRepository, HospitalRepository>();

            services.AddDbContext<DocLinkDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DocLink"), new MySqlServerVersion(new Version(8, 0, 21))));

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

      

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseSession();

      
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Patient}/{action=Login}/{id?}");
            });
        }

    }
}
