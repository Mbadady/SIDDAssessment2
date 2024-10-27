namespace SDD.Services.AuthAPI.Util
{
    public static class Constants
    {
        static IConfiguration configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json") // Adjust this to your configuration file location
        .Build();

        public static string emailHost = configuration["MailSettings:Host"];
        public static string emailFrom = configuration["MailSettings:From"];
        //public static bool emailUseSSL = configuration["MailSettings:UseSSl"];
        public static bool emailUseSSL = configuration.GetValue<bool>("MailSettings:UseSSl");
        public static int emailPort = configuration.GetValue<int>("MailSettings:Port");
        public static string emailPassword = configuration["MailSettings:Password"];
    }
}
