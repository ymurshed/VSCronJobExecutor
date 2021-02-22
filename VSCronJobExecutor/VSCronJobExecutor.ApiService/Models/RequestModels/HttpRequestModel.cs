using System.Collections.Generic;

namespace VSCronJobExecutor.ApiService.Models.RequestModels
{
    public class HttpRequestModel
    {
        public string Url { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public IDictionary<string, object> QueryParams { get; set; }
    }
}
