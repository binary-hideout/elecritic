using System;

using Blazored.LocalStorage;

using Elecritic.Database;
using Elecritic.Services;

using EntityFramework.Exceptions.MySQL.Pomelo;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Elecritic {
    public class Startup {

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment) {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddBlazoredLocalStorage();

            services.AddSingleton<ProductService>();
            services.AddSingleton<ReviewService>();

            services.AddScoped<AuthenticationStateProvider, AuthenticationService>();
            services.AddSingleton<TokenService>();

            services.AddMediatR(typeof(Startup));

            // TODO: refactor all this db contexts mess
            void setDbContextOptions(DbContextOptionsBuilder options) {
                string connectionString = "";
                if (Environment.IsDevelopment()) {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                    options.UseExceptionProcessor();

                    connectionString = Configuration.GetConnectionString("ElecriticDev");
                }
                else {
                    connectionString = Configuration.GetConnectionString("ElecriticDb");
                }

                options.UseMySql(
                    connectionString,
                    new MySqlServerVersion(new Version(5, 7, 31)),
                    mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend));
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
        public void Configure(IApplicationBuilder app) {
            if (Environment.IsDevelopment()) {
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}