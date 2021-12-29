using JecPizza.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace JecPizza.Views
{
    public class ViewModelLocator
    {
        public MainWindowVM MainWindowVm => App.Host.Services.GetRequiredService<MainWindowVM>();
    }
}