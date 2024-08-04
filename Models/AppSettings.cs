using System.Globalization;

namespace BackendAPIDevelopmentTask.Models
{
    public class AppCustomSettings
    {
        public static decimal VAT { get; set; }
        public AppCustomSettings(IConfiguration config)
        {
            VAT = Convert.ToDecimal(config["VAT"], CultureInfo.InvariantCulture);
        }
    }
}
