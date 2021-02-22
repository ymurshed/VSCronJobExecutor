using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VSCronJobExecutor.ApiService.Interfaces;
using VSCronJobExecutor.Common;
using VSCronJobExecutor.Common.Constants;
using VSCronJobExecutor.Common.Models.OptionModels;

namespace VSCronJobExecutor.ConsoleApp.ActionExecutor
{
    public class RunJobAction : BaseAction
    {
        private readonly IOptions<VSCronOptions> _vsCronOptions;

        public RunJobAction(IOptions<VSCronOptions> vsCronOptions)
        {
            _vsCronOptions = vsCronOptions;
        }

        public override async Task ExecuteAsync()
        {
            var jobNames = _vsCronOptions.Value.JobNames.Split(",").ToList();
            foreach (var task in jobNames.Select(jobName => Task.Run(async () => { await ExecuteJob(jobName); })))
            {
                await task;
            }
        }

        private static async Task ExecuteJob(string jobName)
        {
            var jobService = ServiceConfigurationInstance.ServiceProvider.GetService<IJobService>();
            var response = await jobService.GetJobAsync(jobName.Trim());
            
            var jobId = response == null ? throw new Exception(ErrorConstant.InvalidJobName) : response.Id;
            var queryParams = new Dictionary<string, object> {{VSCronConstant.JobIdPlaceholder, jobId}};
            
            var statusCode = await jobService.RunJobAsync(queryParams);
            LogHelper.Print($"Job ({response.Name}) executed successfully with ({statusCode}) status code.");
        }
    }
}
