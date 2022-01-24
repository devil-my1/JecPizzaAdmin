using System.Windows.Input;
using JecPizza.ViewModels.Base;

namespace JecPizza.ViewModels
{
    public class EmailDialogTempVM:BaseViewModel
    {
        #region SelectedTemplate : string - List view selected template

        /// <summary>List view selected template</summary>
        private string _SelectedTemplate;

        /// <summary>List view selected template</summary>
        public string SelectedTemplate { get => _SelectedTemplate; set => Set(ref _SelectedTemplate, value); }

        #endregion


        #region Commands

        public ICommand TemplateChangeCommand { get; set; }

        #endregion

        public EmailDialogTempVM(BaseViewModel vm)
        {
            
        }
    }
}