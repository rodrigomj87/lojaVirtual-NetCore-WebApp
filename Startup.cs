using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using AspNetCoreWebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using AspNetCoreWebApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Http;
using AspNetCoreWebApp.Entities;
using AspNetCoreWebApp.Services;

namespace AspNetCoreWebApp
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
            services.Configure<CookiePolicyOptions>(options => 
            {
                options.CheckConsentNeeded = ctx => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentity<AppUser, IdentityRole>(options => 
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/Login";
                options.SlidingExpiration = true;
            });

            services.AddAuthorization(options => 
            {
                options.AddPolicy("isAdmin", policy => policy.RequireRole("admin"));
            });

            services.AddRazorPages(options => 
            {
                options.Conventions.AuthorizePage("/Admin", "isAdmin");
                options.Conventions.AuthorizeFolder("/ProdutoCRUD", "isAdmin");
            }).AddCookieTempDataProvider(options => 
            {
                options.Cookie.IsEssential = true;
            });

            services.AddMvc();

            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("ApplicationDbContext")));

            services.Configure<EmailConfiguration>(Configuration.GetSection("EmailConfiguration"));
            services.AddSingleton<IEmailSender, EmailSender>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            var defaultCulture = new CultureInfo("pt-BR");
            var localizationOpts = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture},
                SupportedUICultures = new List<CultureInfo> { defaultCulture}
            };
            app.UseRequestLocalization(localizationOpts);
        }
    }
}
