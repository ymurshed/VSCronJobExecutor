using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VSCronJobExecutor.ApiService.Handlers;
using VSCronJobExecutor.ApiService.Interfaces;
using VSCronJobExecutor.ApiService.Models.ResponseModels;
using VSCronJobExecutor.Common.Constants;
using VSCronJobExecutor.Common.Models.OptionModels;

namespace VSCronJobExecutor.ApiService.Services
{
    public class AuthService : ApiHandlerBase, IAuthService
    {
        private readonly IOptions<VSCronOptions> _vsCronOptions;

        public AuthService(IOptions<VSCronOptions> vsCronOptions) : base(vsCronOptions)
        {
            _vsCronOptions = vsCronOptions;
        }

        public override void SetAttributes(Enums.RequestFor requestFor, IDictionary<string, object> queryParams = null)
        {
            ClearAttributes();
            RequestModel.Headers.Add(SharedConstant.ContentType, SharedConstant.ContentTypeFormUrlencoded);
            RequestModel.QueryParams.Add(VSCronConstant.UserPlaceholder, _vsCronOptions.Value.UserName);
            RequestModel.QueryParams.Add(VSCronConstant.PasswordPlaceholder, GetStringFromSecureString(_vsCronOptions.Value.Password));
        }

        public async Task<AuthResponse> AuthenticateAsync()
        {
            SetAttributes(Enums.RequestFor.NONE);
            ConstructUrl(VSCronConstant.AuthenticateUrl);
            var responseMessage = await ExecuteAsync();

            if (responseMessage.StatusCode != HttpStatusCode.OK)
                throw new Exception(ErrorConstant.AuthFail +
                                    $"Status Code: {responseMessage.StatusCode}, " +
                                    $"Reason: {responseMessage.ReasonPhrase}");

            var stringResponse = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AuthResponse>(stringResponse);
        }

        #region Private methods
        private static string GetStringFromSecureString(SecureString secureValue)
        {
            return new NetworkCredential(string.Empty, secureValue).Password;
        }
        #endregion
    }
}
