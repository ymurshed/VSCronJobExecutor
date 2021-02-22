using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using VSCronJobExecutor.Common;
using VSCronJobExecutor.Common.Constants;
using VSCronJobExecutor.Common.Models.OptionModels;
using VSCronJobExecutor.ConsoleApp.ActionExecutor;

namespace VSCronJobExecutor.ConsoleApp
{
    public class Executor
    {
        private static Stopwatch _stopwatch;
        private static BaseAction _action;
        private static Enums.Actions _actionToExecute;

        public Executor(int state = 1)
        {
            _actionToExecute = (Enums.Actions)state;
            _stopwatch = new Stopwatch();
        }

        public async Task Execute()
        {
            while (_actionToExecute != Enums.Actions.END)
            {
                try
                {
                    switch (_actionToExecute)
                    {
                        case Enums.Actions.GET_TOKEN:
                            StartLog(_actionToExecute);

                            _action = new GetTokenAction();
                            await _action.ExecuteAsync();

                            EndLog(_actionToExecute);
                            break;

                        case Enums.Actions.RUN_JOB:
                            StartLog(_actionToExecute);

                            var vsCronOptions = new VSCronOptions();
                            ServiceConfigurationInstance.Configuration.GetSection(typeof(VSCronOptions).Name).Bind(vsCronOptions);

                            _action = new RunJobAction(Options.Create(vsCronOptions));
                            await _action.ExecuteAsync();

                            EndLog(_actionToExecute);
                            break;
                    }
                    SetNextState(_actionToExecute);
                }
                catch (Exception ex)
                {
                    LogHelper.Print(ex, "Exception occurred during action execution.", true);
                    _actionToExecute = Enums.Actions.END;
                }
            }
        }

        #region Private methods
        private static void StartLog(Enums.Actions currentAction)
        {
            _stopwatch.Restart();
            LogHelper.Print($"--->>> Start executing action: {currentAction}, step #{(int)currentAction}, at: {DateTime.Now}");
        }

        private static void EndLog(Enums.Actions currentAction)
        {
            _stopwatch.Stop();
            var totalTimeRequired = _stopwatch.Elapsed.TotalMinutes < 1 ?
                                    $"Total time required: {_stopwatch.Elapsed.TotalSeconds} seconds." :
                                    $"Total time required: {_stopwatch.Elapsed.TotalMinutes} minutes.";
            LogHelper.Print($"<<<--- Stop executing action: {currentAction}, step #{(int)currentAction}, at: {DateTime.Now}. " + totalTimeRequired);
        }

        private static void SetNextState(Enums.Actions currentAction)
        {
            _actionToExecute = currentAction switch
            {
                Enums.Actions.GET_TOKEN => Enums.Actions.RUN_JOB,
                Enums.Actions.RUN_JOB => Enums.Actions.END,
                _ => Enums.Actions.END
            };
        }
        #endregion
    }
}
