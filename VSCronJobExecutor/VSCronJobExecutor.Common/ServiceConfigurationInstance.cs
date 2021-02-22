using System;
using System.Security;
using Microsoft.Extensions.Configuration;

namespace VSCronJobExecutor.Common
{
    public static class ServiceConfigurationInstance
    {
        public static IServiceProvider ServiceProvider;
        public static IConfigurationRoot Configuration;

        public static SecureString GetSecureValue(this IConfiguration configuration, string key)
        {
            var secureValue = new SecureString();
            var value = configuration.GetValue<string>(key);
            
            foreach (var v in value) secureValue.AppendChar(v);
            return secureValue;
        }
    }
}
