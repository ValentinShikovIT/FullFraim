using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utilities.CloudinaryUtils;
using Utilities.Mailing;

namespace FullFraim.Web.Configurations.StartupConfig
{
    public static class ServicesConfig
    {
        public static void ConfigureCloudinary(IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<ICloudinaryUtils>
                (serviceProvider => new CloudinaryUtils(
                    configuration["Cloudinary:CloudName"],
                    configuration["Cloudinary:ApiKey"],
                    configuration["Cloudinary:ApiSecret"]));
        }

        public static void ConfigureEmailSender(IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IEmailSender>
                (serviceProvider => new SendGridEmailSender(configuration["SendGrid:ApiKey"]));
        }

        public static void ConfigureIdentityCookiePaths(IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
        }
    }
}
