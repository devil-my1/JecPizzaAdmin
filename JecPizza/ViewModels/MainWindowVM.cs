using JecPizza.ViewModels.Base;
using LocalizatorHelper;

namespace JecPizza.ViewModels
{
    public class MainWindowVM : BaseViewModel
    {
        private string _title;
        public string Title { get => _title; set => Set(ref _title, value); }


        public MainWindowVM()
        {
            ResourceManagerService.RegisterManager("lang", Content.Languages.Language.ResourceManager);
            ResourceManagerService.ChangeLocale("ja-JP");

            Title = ResourceManagerService.GetResourceString("lang", "Title");
        }

    }
}