using FullFraim.Data;
using FullFraim.Data.Seed;
using Utilities.API_JwtService;
using FullFraim.Services.ContestCatgeoryServices;
using FullFraim.Services.ContestServices;
using FullFraim.Services.ContestTypeServices;
using FullFraim.Services.JuryServices;
using FullFraim.Services.PhaseServices;
using FullFraim.Services.PhotoJunkieServices;
using FullFraim.Services.PhotoService;
using FullFraim.Web.Configurations.StartupConfig;
using FullFraim.Web.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Utilities.Security;

namespace FullFraim.Web
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
            services.AddDbContext<FullFraimDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews(options =>
            {
                options.Filters
                    .Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            services.AddControllers();
            services.AddRazorPages();

            services.AddMemoryCache();
            services.AddScoped<IJwtServices, JWTUtils>();
            services.AddScoped<IContestService, ContestService>();
            services.AddScoped<IContestCategoryService, ContestCategoryService>();
            services.AddScoped<IContestTypeService, ContestTypeService>();
            services.AddScoped<IPhaseService, PhaseService>();
            services.AddTransient<APIExceptionFilter>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IPhotoJunkieService, PhotoJunkieService>();
            services.AddScoped<IJuryService, JuryService>();
            services.AddScoped<ISecurityUtils, SecutiryUtils>();

            ServicesConfig.ConfigureIdentityCookiePaths(services);
            ServicesConfig.ConfigureCloudinary(services, Configuration);
            ServicesConfig.ConfigureEmailSender(services, Configuration);

            AuthenticationConfig.IdentityConfiguration(services);
            AuthenticationConfig.ConfigureWith_JwtAndMVC(services, Configuration);

            SwaggerConfig.Configure(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<FullFraimDbContext>();
                dbContext.Database.Migrate();
                new FullFraimContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/Error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FullFraim");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                            "areaRoute",
                            "{area:exists}/{controller=Home}/{action=Index}/{projectId?}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
