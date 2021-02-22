using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VSCronJobExecutor.ApiService.Handlers;
using VSCronJobExecutor.ApiService.Interfaces;
using VSCronJobExecutor.ApiService.Models.ResponseModels;
using VSCronJobExecutor.Common;
using VSCronJobExecutor.Common.Constants;
using VSCronJobExecutor.Common.Models.OptionModels;

namespace VSCronJobExecutor.ApiService.Services
{
    public class JobService : ApiHandlerBase, IJobService
    {
        private readonly IOptions<VSCronOptions> _vsCronOptions;
        private string JobName { get; set; }

        public JobService(IOptions<VSCronOptions> vsCronOptions) : base(vsCronOptions)
        {
            _vsCronOptions = vsCronOptions;
        }

        public override void SetAttributes(Enums.RequestFor requestFor, IDictionary<string, object> queryParams = null)
        {
            ClearAttributes();
            RequestModel.Headers.Add(SharedConstant.ContentType, SharedConstant.ContentTypeFormUrlencoded);
            RequestModel.QueryParams.Add(VSCronConstant.TokenPlaceholder, AppInstance.Token);

            switch (requestFor)
            {
                case Enums.RequestFor.GET_JOB:
                    RequestModel.QueryParams.Add(VSCronConstant.JobNamePlaceholder, JobName);
                    break;

                case Enums.RequestFor.RUN_JOB:
                    foreach (var (key, value) in queryParams)
                    {
                        if (!RequestModel.QueryParams.ContainsKey(key))
                        {
                            RequestModel.QueryParams.Add(key, value);
                        }
                        else
                        {
                            RequestModel.QueryParams[key] = value;
                        }
                    }
                    break;
            }
        }

        public async Task<JobResponse> GetJobAsync(string jobName)
        {
            JobName = jobName;
            SetAttributes(Enums.RequestFor.GET_JOB);
            ConstructUrl(VSCronConstant.GetJobUrl);
            var responseMessage = await ExecuteAsync();

            if (responseMessage.StatusCode != HttpStatusCode.OK)
                throw new Exception(ErrorConstant.GetJobFail +
                                    $"Status Code: {responseMessage.StatusCode}, " +
                                    $"Reason: {responseMessage.ReasonPhrase}");

            var stringResponse = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JobResponse>(stringResponse);
        }

        public async Task<HttpStatusCode> RunJobAsync(IDictionary<string, object> queryParams)
        {
            SetAttributes(Enums.RequestFor.RUN_JOB, queryParams);
            ConstructUrl(VSCronConstant.RunJobUrl);
            var responseMessage = await ExecuteAsync();

            if (responseMessage.StatusCode != HttpStatusCode.OK)
                throw new Exception(ErrorConstant.RunJobFail +
                                    $"Status Code: {responseMessage.StatusCode}, " +
                                    $"Reason: {responseMessage.ReasonPhrase}");

            return responseMessage.StatusCode;
        }
    }
}
