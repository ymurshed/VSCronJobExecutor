using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using VSCronJobExecutor.ApiService.Interfaces;
using VSCronJobExecutor.Common;
using VSCronJobExecutor.Common.Constants;

namespace VSCronJobExecutor.ConsoleApp.ActionExecutor
{
    public class GetTokenAction : BaseAction
    {
        private readonly IAuthService _authService;

        public GetTokenAction()
        {
            _authService = ServiceConfigurationInstance.ServiceProvider.GetService<IAuthService>();
        }

        public override async Task ExecuteAsync()
        {
            var response = await _authService.AuthenticateAsync();
            AppInstance.Token = response.Token ?? throw new Exception(ErrorConstant.InvalidCredential);
            LogHelper.Print($"Valid token ({AppInstance.Token}) found.");
        }
    }
}
