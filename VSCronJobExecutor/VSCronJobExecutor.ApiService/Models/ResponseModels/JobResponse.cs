using VSCronJobExecutor.ApiService.Models.ResponseModels.SubModels;

namespace VSCronJobExecutor.ApiService.Models.ResponseModels
{
    public class JobResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public Stats Stats { get; set; }
    }
}
