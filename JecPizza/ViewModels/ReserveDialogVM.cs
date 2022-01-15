using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using JecPizza.Infostucture.Assist;
using JecPizza.Infostucture.Command;
using JecPizza.Models;
using JecPizza.ViewModels.Base;

namespace JecPizza.ViewModels
{
    public class ReserveDialogVM : BaseViewModel
    {
        private BaseViewModel Host { get; set; }
        private MainWindowVM MVM { get; set; }

        #region NewReserve : Reserve - New Reserve Model

        /// <summary>New Reserve Model</summary>
        private Reserve _ReserveModel;

        /// <summary>New Reserve Model</summary>
        public Reserve ReserveModel { get => _ReserveModel; set => Set(ref _ReserveModel, value); }

        #endregion

        #region Time : string - Reservation Time

        /// <summary>Reservation Time</summary>
        private string _Time;

        /// <summary>Reservation Time</summary>
        public string Time { get => _Time; set => Set(ref _Time, value); }

        #endregion

        #region InputTelNumberMask : string - Phone Number Input Format

        /// <summary>Phone Number Input Format</summary>
        private string _InputTelNumberMask;

        /// <summary>Phone Number Input Format</summary>
        public string InputTelNumberMask { get => _InputTelNumberMask; set => Set(ref _InputTelNumberMask, value); }

        #endregion

        public ICommand MultyCommand { get; set; }
        public ICommand PhoneTextChangeCommand { get; set; }


        public ReserveDialogVM(BaseViewModel host, ReserveMode mode, Reserve rSelceted = null)
        {
            Host = host;
            MVM = (MainWindowVM)Host;


            ReserveModel = (Reserve)rSelceted?.Clone() ?? new Reserve()
            {
                ReserveId = "R" + new Random().Next(999).ToString("D3"),
                Num = 1,
                Date = DateTime.Now,
                IsReserved = true
            };
            var tempReserve = MVM.SelectedReserve;

            InputTelNumberMask = mode == ReserveMode.AddMode ? "000-(0000)-0000" : null;

            ReserveModel.Tel = ReserveModel.Tel?.Trim() ?? "";

            Time = ReserveModel.Date.ToShortTimeString();

            PhoneTextChangeCommand = new RellayCommand(OnPTextChanged);

            MultyCommand = mode == ReserveMode.AddMode ? new RellayCommand(OnAddReserve, p => (!string.IsNullOrEmpty(ReserveModel.Tel) &&
                                                                                               !string.IsNullOrEmpty(ReserveModel.Date.ToString(CultureInfo.InvariantCulture)) &&
                                                                                               !string.IsNullOrEmpty(ReserveModel.TableNum.ToString()) &&
                                                                                               !string.IsNullOrEmpty(ReserveModel.Num.ToString()))) : new RellayCommand(OnChange, p => ReserveModel != tempReserve || !Time.Equals(ReserveModel.Date.ToShortTimeString()));
        }

        private void OnPTextChanged(object Obj)
        {
            if (ReserveModel.Tel.Length == 0) InputTelNumberMask = "000-(0000)-0000";
        }

        private void OnChange(object Obj)
        {
            ReserveModel.Date = DateTime.Parse(ReserveModel.Date.ToShortDateString() + " " + Time, CultureInfo.InvariantCulture);
            MVM.ReservationServices.UpdateReserve(ReserveModel);

            MVM.rcv.Source = MVM.ReservationServices.GetAllReserve();
            MVM.ReserveCollection = MVM.rcv.View;
            MVM.ReserveCollection.Refresh();


            MVM.IsDialogOpen = false;
        }

        private void OnAddReserve(object Obj)
        {
            ReserveModel.Date = DateTime.Parse(ReserveModel.Date.ToShortDateString() + " " + Time, CultureInfo.InvariantCulture);
            var res = MVM.ReservationServices.InsertReservation(ReserveModel);

            MVM.rcv.Source = MVM.ReservationServices.GetAllReserve();
            MVM.ReserveCollection = MVM.rcv.View;
            MVM.ReserveCollection.Refresh();

            System.Windows.MessageBox.Show(res ? LocalizatorHelper.ResourceManagerService.GetResourceString("lang", "SucAdd") : LocalizatorHelper.ResourceManagerService.GetResourceString("lang", "ErEdit"), "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            MVM.IsDialogOpen = false;
        }


    }
}