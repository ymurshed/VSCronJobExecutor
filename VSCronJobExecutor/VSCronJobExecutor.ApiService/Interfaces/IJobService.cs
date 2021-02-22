using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using VSCronJobExecutor.ApiService.Models.ResponseModels;

namespace VSCronJobExecutor.ApiService.Interfaces
{
    public interface IJobService
    {
        Task<JobResponse> GetJobAsync(string jobName);
        Task<HttpStatusCode> RunJobAsync(IDictionary<string, object> queryParams);
    }
}
