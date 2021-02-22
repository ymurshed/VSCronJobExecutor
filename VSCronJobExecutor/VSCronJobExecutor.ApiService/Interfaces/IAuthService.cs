using System.Threading.Tasks;
using VSCronJobExecutor.ApiService.Models.ResponseModels;

namespace VSCronJobExecutor.ApiService.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync();
    }
}
