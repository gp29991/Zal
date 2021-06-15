using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zal.Data;
using Zal.Models;

namespace Zal
{
    public class Startup
    {

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            /*services.AddDbContext<UserContext>(options =>
            {
                options.UseMySql(_configuration.GetConnectionString("users"), ServerVersion.AutoDetect(_configuration.GetConnectionString("users")));
            });

            services.AddDbContext<EmployeeContext>(options =>
            {
                options.UseMySql(_configuration.GetConnectionString("employees"), ServerVersion.AutoDetect(_configuration.GetConnectionString("employees")))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });*/

            services.AddDbContext<UserContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("users-mssql"));
            });

            services.AddDbContext<EmployeeContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("employees-mssql"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("pl-PL");
                options.AddSupportedCultures(new string[]{ "pl-PL" });
            });

            services.AddControllersWithViews().AddMvcOptions(options =>
            {
                options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((s1, s2) => "Wprowadzona wartość jest nieprawidłowa");
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<UserContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Home/Login";
                options.AccessDeniedPath = "/Home/AccessDenied";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserContext context1, EmployeeContext context2)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            context1.Database.EnsureCreated();
            context2.Database.EnsureCreated();
        }
    }
}
