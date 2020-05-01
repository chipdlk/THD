using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.MiddleWares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using reCAPTCHA.AspNetCore;
namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static string GoogleReCaptcha { get; private set; }
        public static string ConnectionString { get; private set; }
        public static string SqlOldConnection { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver
                    = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            });

            
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(100000);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });
            // --------- Add Capcha google
            services.Configure<RecaptchaSettings>(Configuration.GetSection("RecaptchaSettings"));
            services.AddTransient<IRecaptchaService, RecaptchaService>();
            // --------- End Add Capcha google
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                ConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
                app.UseDeveloperExceptionPage();
            }
            else
            {
                ConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            SqlOldConnection = Configuration["ConnectionStrings:SqlOldConnection"];

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseStaticFiles();
            app.UseSession();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());            
            app.UseMyAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areaRoute",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "HoiDap",
                    template: "hoi-dap.html",
                    defaults: new { controller = "Contacts", action = "Index" });

                routes.MapRoute(
                    name: "LienHe",
                    template: "lien-he.html",
                    defaults: new { controller = "Home", action = "Contact" });

            

                routes.MapRoute(
                    name: "Videos",
                    template: "videos.html",
                    defaults: new { controller = "Videos", action = "Index" });

                routes.MapRoute(
                   name: "Users",
                   template: "doi-ngu.html",
                   defaults: new { controller = "Users", action = "Index" });

                routes.MapRoute(
                   name: "SiteMap",
                   template: "sitemap.html",
                   defaults: new { controller = "Home", action = "SiteMap" });

                routes.MapRoute(
                   name: "Users_Detail",
                   template: "doi-ngu/{alias}-{id}.html",
                   defaults: new { controller = "Users", action = "Detail" });

                routes.MapRoute(
                    name: "Documents",
                    template: "van-ban.html",
                    defaults: new { controller = "Documents", action = "Index" });

                routes.MapRoute(
                    name: "Reviews",
                    template: "thong-tin-tu-van.html",
                    defaults: new { controller = "Reviews", action = "Index" });

                routes.MapRoute(
                  name: "Events",
                  template: "chuong-trinh-kham.html",
                  defaults: new { controller = "Events", action = "Index" });


                routes.MapRoute(
                   name: "Articles",
                   template: "articles/{alias}.html",
                   defaults: new { controller = "Articles", action = "Index" });

                routes.MapRoute(
                   name: "DMCoQuan",
                   template: "phong-ban-khoa/{alias}.html",
                   defaults: new { controller = "DMCoQuan", action = "Index" });


                routes.MapRoute(
                  name: "EventsDetail",
                  template: "chuong-trinh-kham/{alias}-{id}.html",
                  defaults: new { controller = "Events", action = "Detail" });


                routes.MapRoute(
                  name: "ReviewsDetail",
                  template: "cam-nhan/{alias}-{id}.html",
                  defaults: new { controller = "Reviews", action = "Detail" });

                routes.MapRoute(
                   name: "ArticlesCategories",
                   template: "categories/{alias}-{id}.html",
                   defaults: new { controller = "Articles", action = "GetByCat" });

                routes.MapRoute(
                   name: "ProductsCategories",
                   template: "products/{alias}-{id}.html",
                   defaults: new { controller = "Products", action = "Index" });

                routes.MapRoute(
                   name: "GetListChildCat",
                   template: "tuyen-sinh/{alias}-{id}.html",
                   defaults: new { controller = "Articles", action = "GetListChildCat" });

                routes.MapRoute(
                   name: "Articles_Id_1",
                   template: "gioi-thieu.html",
                   defaults: new { controller = "Articles", action = "Detail", Id = 1 });
                routes.MapRoute(
                   name: "Articles_Id_2",
                   template: "thong-tin-chung.html",
                   defaults: new { controller = "Articles", action = "Detail", Id = 2 });

                routes.MapRoute(
                   name: "Articles_Detail",
                   template: "{alias}-{id}.html",
                   defaults: new { controller = "Articles", action = "Detail" });

                routes.MapRoute(
                   name: "Albums",
                   template: "Albums.html",
                   defaults: new { controller = "Albums", action = "Index" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            GoogleReCaptcha = Configuration["GoogleReCaptcha:key"];
        }
    }
}
