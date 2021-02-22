namespace VSCronJobExecutor.Common.Constants
{
    public class VSCronConstant
    {
        #region Param placeholders
        public const string DomainNamePlaceholder       = "{vscron_domain_name}";
        public const string UserPlaceholder             = "{user}";
        public const string PasswordPlaceholder         = "{password}";
        public const string TokenPlaceholder            = "{token}";
        public const string JobNamePlaceholder          = "{job_name}";
        public const string JobIdPlaceholder            = "{job_id}";
        #endregion

        #region Urls
        public static readonly string BaseUrl                  = $"http://{DomainNamePlaceholder}/VisualCron/json/";
        public static readonly string AuthenticateUrl          = $"logon?username={UserPlaceholder}&password={PasswordPlaceholder}";
        public static readonly string GetJobUrl                = $"Job/GetByName?token={TokenPlaceholder}&name={JobNamePlaceholder}";
        public static readonly string RunJobUrl                = $"Job/Run?token={TokenPlaceholder}&id={JobIdPlaceholder}";
        #endregion
    }
}
