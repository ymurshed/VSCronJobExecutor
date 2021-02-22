using System;

namespace VSCronJobExecutor.ApiService.Models.ResponseModels.SubModels
{
    public class Stats
    {
        public string JobId { get; set; }
        public bool Active { get; set; }
        public int ExitCode { get; set; }
        public int ExitCodeResult { get; set; }
        public DateTime DateLastExecution { get; set; }
        public DateTime DateNextExecution { get; set; }
        public DateTime DateLastExited { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int NoExecutes { get; set; }
        public long ExecutionTime { get; set; }
        public string LastTriggerId { get; set; }
        public int Status { get; set; }
        public string LastExecutionId { get; set; }
    }
}
