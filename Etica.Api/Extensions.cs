using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Etica.Api
{
    public static class Extensions
    {
        public static void UseCulture(this IApplicationBuilder builder, IConfiguration configuration)
        {
            var culture = configuration["Settings:Culture"];
            var cultureInfo = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}
