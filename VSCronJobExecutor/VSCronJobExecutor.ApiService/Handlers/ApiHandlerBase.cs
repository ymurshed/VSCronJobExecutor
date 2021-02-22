using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VSCronJobExecutor.ApiService.Models.RequestModels;
using VSCronJobExecutor.Common.Constants;
using VSCronJobExecutor.Common.Models.OptionModels;

namespace VSCronJobExecutor.ApiService.Handlers
{
    public abstract class ApiHandlerBase
    {
        #region properties
        public HttpRequestModel RequestModel { get; set; }
        private readonly IOptions<VSCronOptions> _vsCronOptions;
        #endregion

        #region Private methods
        private static string CastValue(object value)
        {
            var newValue = value is List<string> list ? list.FirstOrDefault() : value.ToString();
            return newValue;
        }
        #endregion

        protected ApiHandlerBase(IOptions<VSCronOptions> vsCronOptions)
        {
            _vsCronOptions = vsCronOptions;

            RequestModel = new HttpRequestModel
            {
                Headers = new Dictionary<string, string>(),
                QueryParams = new Dictionary<string, object>()
            };
        }

        public abstract void SetAttributes(Enums.RequestFor requestFor, IDictionary<string, object> queryParams = null);

        public void ClearAttributes()
        {
            RequestModel.Headers.Clear();
            RequestModel.QueryParams.Clear();
        }

        public void ConstructUrl(string url)
        {
            var tempUrl = VSCronConstant.BaseUrl + url;
            RequestModel.Url = tempUrl.Replace(VSCronConstant.DomainNamePlaceholder, _vsCronOptions.Value.DomainName);
            
            if (!RequestModel.QueryParams.Any()) return;

            foreach (var (key, value) in RequestModel.QueryParams)
            {
                RequestModel.Url = RequestModel.Url.Replace(key, CastValue(value), StringComparison.Ordinal);
            }
        }

        public async Task<HttpResponseMessage> ExecuteAsync()
        {
            var httpResponse = await HttpClientHelper.GetAsync(RequestModel.Url, RequestModel.Headers);
            return httpResponse;
        }
    }
}
