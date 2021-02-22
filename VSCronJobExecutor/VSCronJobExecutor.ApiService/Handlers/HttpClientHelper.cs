using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VSCronJobExecutor.Common.Constants;

namespace VSCronJobExecutor.ApiService.Handlers
{
    public class HttpClientHelper
    {
        public static async Task<HttpResponseMessage> GetAsync(string requestUrl, IDictionary<string, string> headers)
        {
            using var client = PrepareHttpClient(headers);

            try
            {
                return await client.GetAsync(requestUrl);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message),
                    ReasonPhrase = ex.Message
                };
            }
        }

        #region Private methods
        private static HttpClient PrepareHttpClient(IDictionary<string, string> headers)
        {
            var handler = new HttpClientHandler
            {
                UseDefaultCredentials = false
            };

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new HttpClient(handler);
            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            client.DefaultRequestHeaders.ExpectContinue = false;
            client.Timeout = TimeSpan.FromSeconds(SharedConstant.ApiResponseTimeout);

            if (headers == null || !headers.Any()) return client;

            foreach (var (key, value) in headers)
            {
                if (key.Contains(SharedConstant.ContentType)) continue;
                client.DefaultRequestHeaders.Add(key, value);
            }
            
            return client;
        }
        #endregion
    }
}
