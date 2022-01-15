using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using JecPizza.Infostucture.Command;
using JecPizza.ViewModels.Base;

namespace JecPizza.ViewModels
{
    public class SetBudgetDialogVM : BaseViewModel
    {
        private readonly BaseViewModel _Host;
        private readonly string _Password = "admin";

        public MainWindowVM Mvm { get; set; }


        #region Visibility : Visibility - DESCRIPTION

        /// <summary>DESCRIPTION</summary>
        private Visibility _Visibility;

        /// <summary>DESCRIPTION</summary>
        public Visibility Visibility { get => _Visibility; set => Set(ref _Visibility, value); }

        #endregion

        #region PasValidVisibility : Visibility - DESCRIPTION

        /// <summary>DESCRIPTION</summary>
        private Visibility _PasValidVisibility;

        /// <summary>DESCRIPTION</summary>
        public Visibility PasValidVisibility { get => _PasValidVisibility; set => Set(ref _PasValidVisibility, value); }

        #endregion

        #region ErrorMsg : string - Error Message to info about not correct password

        /// <summary>Error Message to info about not correct password</summary>
        private string _ErrorMsg;

        /// <summary>Error Message to info about not correct password</summary>
        public string ErrorMsg { get => _ErrorMsg; set => Set(ref _ErrorMsg, value); }

        #endregion

        public ICommand Accept { get; set; }

        public SetBudgetDialogVM()
        {

        }

        public SetBudgetDialogVM(BaseViewModel host)
        {
            _Host = host;
            Mvm = (MainWindowVM)_Host;
            Visibility = Visibility.Visible;
            PasValidVisibility = Visibility.Collapsed;
            Accept = new RellayCommand(OnAccept);
        }

        private void OnAccept(object Obj)
        {
            if (Obj is PasswordBox pb)
            {

                if (Visibility != Visibility.Collapsed)
                {
                    if (string.IsNullOrEmpty(pb.Password) || !pb.Password.Equals(_Password))
                    {
                        ErrorMsg = "Not Correct Password";
                        pb.Password = "";
                        return;
                    }
                    else
                    {
                        Visibility = Visibility.Collapsed;
                        PasValidVisibility = Visibility.Visible;
                    }


                }
                else
                {
                    System.Windows.MessageBox.Show($"Budget Seted!{Mvm.Budget}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    Mvm.TodaysTotalSales = Mvm.PurchaseDeliveryService.GetTodaysTotalSales() / Mvm.Budget * 100;

                    Properties.Settings.Default["Buget"] = Mvm.Budget;
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();

                    Mvm.IsDialogOpen = false;
                }
            }
        }
    }
}