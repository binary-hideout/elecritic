using System;

using Elecritic.Database;
using Elecritic.Services;

using EntityFramework.Exceptions.MySQL.Pomelo;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Elecritic {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddSingleton<ProductService>();
            services.AddSingleton<ReviewService>();
            services.AddSingleton<UserService>();

            // local function
            void setDbContextOptions(DbContextOptionsBuilder options) {
                options.UseMySql(
                    Configuration.GetConnectionString("ElecriticDb"),
                    new MySqlServerVersion(new Version(5, 7, 31)),
                    mySqlOptions => mySqlOptions
                        .CharSetBehavior(CharSetBehavior.NeverAppend))
                    .UseExceptionProcessor();
#if DEBUG
                options.EnableSensitiveDataLogging(true);
#endif
            }
            services.AddDbContext<UploadDataContext>(options => setDbContextOptions(options));
            services.AddDbContext<CategoryProductsContext>(options => setDbContextOptions(options));
            services.AddDbContext<UserContext>(options => setDbContextOptions(options));
            services.AddDbContext<ProductContext>(options => setDbContextOptions(options));
            services.AddDbContext<IndexContext>(options => setDbContextOptions(options));
            services.AddDbContext<MyFavoritesContext>(options => setDbContextOptions(options));
            // only used when migrating to the database
            //! do not uncomment it
            //services.AddDbContext<MainDbContext>(options => setDbContextOptions(options));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}