using System.Threading.Tasks;

namespace VSCronJobExecutor.ConsoleApp.ActionExecutor
{
    public abstract class BaseAction
    {
        public abstract Task ExecuteAsync();
    }
}
