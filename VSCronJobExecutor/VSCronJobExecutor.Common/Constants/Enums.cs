namespace VSCronJobExecutor.Common.Constants
{
    public class Enums
    {
        public enum Actions
        {
            GET_TOKEN = 1,
            RUN_JOB = 2,
            END = 3
        }

        public enum RequestFor
        {
            NONE,
            GET_JOB,
            RUN_JOB
        }
    }
}
