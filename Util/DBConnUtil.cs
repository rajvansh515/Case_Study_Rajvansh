using System;
using Microsoft.Extensions.Configuration;
namespace CarRentalSystem.Util
{
    internal static class DbConnUtil
    {
        private static IConfiguration _iconfiguration;

        static DbConnUtil()
        {
            GetAppSettingsFile();
        }

        private static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())//sets the base path where configuration file is located
                .AddJsonFile("appSettings.json");
            _iconfiguration = builder.Build();
        }

        public static string GetConnString()
        {
            return _iconfiguration.GetConnectionString("LocalConnectionString");
        }
    }

}

