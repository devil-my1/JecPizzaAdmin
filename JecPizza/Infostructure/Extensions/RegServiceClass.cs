using JecPizza.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace JecPizza.Infostructure.Extensions
{
    public static class RegServiceClass
    {
        public static void RegisterViewModels(this IServiceCollection service)
        {
            service.AddSingleton<MainWindowVM>();
        }

        public static void RegisterServices(this IServiceCollection service)
        {
            //service.AddSingleton<IAccountService, AccountService>();
        }
    }
}