using System.Security;
using VSCronJobExecutor.Common.Constants;

namespace VSCronJobExecutor.Common.Models.OptionModels
{
    public class VSCronOptions
    {
        public string DomainName { get; set; }
        public string UserName { get; set; }
        public SecureString Password => ServiceConfigurationInstance.Configuration.GetSecureValue(SharedConstant.VSCronOptionsPassword);
        public string JobNames { get; set; }
    }
}
