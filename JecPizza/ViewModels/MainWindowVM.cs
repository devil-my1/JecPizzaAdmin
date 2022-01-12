using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using JecPizza.Infostructure.Assist;
using JecPizza.Infostructure.Command;
using JecPizza.Models;
using JecPizza.Services;
using JecPizza.ViewModels.Base;
using JecPizza.Views.Dialogs;
using LiveCharts;
using LiveCharts.Wpf;
using LocalizatorHelper;

namespace JecPizza.ViewModels
{
    public class MainWindowVM : BaseViewModel
    {
        public GoodsService GoodsService { get; set; }
        public ReservationService ReservationServices { get; set; }
        public PurchaseDeliveryService PurchaseDeliveryService { get; set; }
        public readonly CollectionViewSource gcv = new CollectionViewSource();
        public readonly CollectionViewSource rcv = new CollectionViewSource();
        public bool IsAscending { get; set; } = true;




        #region Properties

        #region SeriesCollection : SeriesCollection - Goods Group

        /// <summary>Goods Group</summary>
        private SeriesCollection _SeriesCollection;

        /// <summary>Goods Group Series</summary>
        public SeriesCollection GoodsGroupSeries { get => _SeriesCollection; set => Set(ref _SeriesCollection, value); }

        #endregion

        #region MoneyLabelFormater : Func<double,string> - Chart Money label Formater

        /// <summary>Chart Money label Formater</summary>
        private Func<double, string> _MoneyLabelFormatter;

        /// <summary>Chart Money label Formater</summary>
        public Func<double, string> MoneyLabelFormatter { get => _MoneyLabelFormatter; set => Set(ref _MoneyLabelFormatter, value); }

        #endregion

        #region Months : List<string> - Months Item for Combo Box

        /// <summary>Months Item for Combo Box</summary>
        private List<string> _Months;

        /// <summary>Months Item for Combo Box</summary>
        public List<string> Months { get => _Months; set => Set(ref _Months, value); }

        #endregion

        #region Dates : IList<string> - X Axsis label

        /// <summary>X Axsis label</summary>
        private IList<string> _Dates;

        /// <summary>X Axsis label</summary>
        public IList<string> Dates { get => _Dates; set => Set(ref _Dates, value); }

        #endregion

        #region ColumnValues : IChartValues - Chart Column Values

        /// <summary>Chart Column Values</summary>
        private IChartValues _ColumnValues;

        /// <summary>Chart Column Values</summary>
        public IChartValues ColumnValues { get => _ColumnValues; set => Set(ref _ColumnValues, value); }

        #endregion

        #region MonthDataValues : IDictionary<DateTime,int> - Getting totals for each day in a selecte month 

        /// <summary>Getting totals for each day in a selecte month </summary>
        private IDictionary<DateTime, int> _MonthDataValues;

        /// <summary>Getting totals for each day in a selecte month </summary>
        public IDictionary<DateTime, int> MonthDataValues { get => _MonthDataValues; set => Set(ref _MonthDataValues, value); }

        #endregion

        #region SelectedMonth : string - Combo box Selected Month Value

        /// <summary>Combo box Selected Month Value</summary>
        private int _SelectedMonthIndex;

        /// <summary>Combo box Selected Month Value</summary>
        public int SelectedMonthIndex { get => _SelectedMonthIndex; set => Set(ref _SelectedMonthIndex, value); }

        #endregion

        #region Func : Func<double,str> - Chart Label Formater 

        /// <summary>Chart Label Formater </summary>
        private Func<double, string> _Label;

        /// <summary>Chart Label Formater </summary>
        public Func<double, string> Label { get => _Label; set => Set(ref _Label, value); }

        #endregion

        #region SalesValue : double - Sales Value

        /// <summary>Sales Value</summary>
        private double _SalesValue;

        /// <summary>Sales Value</summary>
        public double SalesValue { get => _SalesValue; set => Set(ref _SalesValue, value); }

        #endregion

        #region Budget : int - Today's Budget

        /// <summary>Today's Budget</summary>
        private int _Budget;

        /// <summary>Today's Budget</summary>
        public int Budget { get => _Budget; set => Set(ref _Budget, value); }

        #endregion

        #region TodaysTotalSales : double - Todays Total Sales

        /// <summary>Todays Total Sales</summary>
        private double _TodaysTotalSales;

        /// <summary>Todays Total Sales</summary>
        public double TodaysTotalSales { get => _TodaysTotalSales; set => Set(ref _TodaysTotalSales, value); }

        #endregion

        #region CurrentSelectedGoods : Goods - Select Goods

        /// <summary>Select Goods</summary>
        private Goods _CurrentSelectedGoods;

        /// <summary>Select Goods</summary>
        public Goods CurrentSelectedGoods { get => _CurrentSelectedGoods; set => Set(ref _CurrentSelectedGoods, value); }

        #endregion

        #region DialogContent : object - Dialog Content

        /// <summary>Dialog Content</summary>
        private object _DialogContent;

        /// <summary>Dialog Content for dialog host</summary>
        public object DialogContent { get => _DialogContent; set => Set(ref _DialogContent, value); }

        #endregion

        #region IsDialogOpen : bool - Dialog open indetification

        /// <summary>Dialog open indetification</summary>
        private bool _IsDialogOpen;

        /// <summary>Dialog open indetification</summary>
        public bool IsDialogOpen { get => _IsDialogOpen; set => Set(ref _IsDialogOpen, value); }

        #endregion

        #region SelectedItemIndex : int - Item Index

        /// <summary>Item Index</summary>
        private int _SelectedItemIndex;

        /// <summary>Item Index</summary>
        public int SelectedItemIndex { get => _SelectedItemIndex; set => Set(ref _SelectedItemIndex, value); }

        #endregion

        #region Search : string - Search into GoodsTable

        /// <summary>Search into GoodsTable</summary>
        private string _Search;

        /// <summary>Search into GoodsTable</summary>
        public string Search
        {
            get => _Search; set
            {
                Set(ref _Search, value);
                gcv.View.Refresh();
            }
        }

        #endregion

        #region FilterOption : string - Filter option

        /// <summary>Filter option</summary>
        private string _FilterOption;

        /// <summary>Filter option</summary>
        public string FilterOption { get => _FilterOption; set => Set(ref _FilterOption, value); }

        #endregion

        #region GoodsCollection : ICollectionView - Goods Data

        private ICollectionView _collectionView;
        public ICollectionView GoodsCollection { get => _collectionView; set => Set(ref _collectionView, value); }

        #endregion

        #region ReserveCollection : ICollectionView - Reservation Data

        /// <summary>Reservation Data</summary>
        private ICollectionView _ReserveCollection;

        /// <summary>Reservation Data</summary>
        public ICollectionView ReserveCollection { get => _ReserveCollection; set => Set(ref _ReserveCollection, value); }

        #endregion

        #region SelectedReserve : Reserve - Reserve Row Selected

        /// <summary>Reserve Row Selected</summary>
        private Reserve _SelectedReserve;

        /// <summary>Reserve Row Selected</summary>
        public Reserve SelectedReserve { get => _SelectedReserve; set => Set(ref _SelectedReserve, value); }

        #endregion

        #endregion


        #region Commands

        public ICommand CloseWindowCommand { get; }
        public ICommand ChangeLanguageCommand { get; }
        public ICommand OpenEditGoodsCommand { get; set; }
        public ICommand AddNewGoodsCommand { get; set; }
        public ICommand DeleteGoodsCommand { get; set; }
        public ICommand SortCommand { get; set; }
        public ICommand CheckedCommand { get; set; }
        public ICommand EditReserveCommand { get; set; }
        public ICommand DeleteReserveCommand { get; set; }
        public ICommand AddReserveCommand { get; set; }
        public ICommand UpdateReserveTableCommand { get; set; }
        public ICommand SetBudgetCommand { get; set; }
        public ICommand MonthChangedCommand { get; set; }
        public ICommand ColumnClickedCommand { get; set; }


        #endregion




        public MainWindowVM()
        {

            ResourceManagerService.RegisterManager("lang", Content.Languages.Language.ResourceManager);
            ResourceManagerService.ChangeLocale(Properties.Settings.Default.Language);

            GoodsService = new GoodsService();
            ReservationServices = new ReservationService();
            PurchaseDeliveryService = new PurchaseDeliveryService();

            gcv.Filter += OnGoodsTableFilter;
            gcv.Source = GoodsService.GetAllGoods();
            rcv.Source = ReservationServices.GetAllReserve();



            #region Properties

            GoodsGroupSeries = new SeriesCollection();
            Months = new List<string>(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Take(12));
            SelectedMonthIndex = 0;

            MonthDataValues = PurchaseDeliveryService.GetTotalSalesByMonth(1);
            ColumnValues = new ChartValues<int>(MonthDataValues.Values);
            Dates = new List<string>();


            foreach (DateTime dt in MonthDataValues.Keys)
                Dates.Add(dt.Date.ToString("MM/dd", CultureInfo.InvariantCulture) ?? "0");

            var temp = PurchaseDeliveryService.GetTodaysPercentageOfGoods();

            if (temp != null)
            {
                double tempSumVal = temp.Values.Sum();

                Func<ChartPoint, string> ShowLabel = cp => $"{cp.Y} ({cp.Y / tempSumVal:p1})";



                foreach ((string key, int value) in temp)
                {
                    GoodsGroupSeries.Add(new PieSeries()
                    {
                        Title = key,
                        Values = new ChartValues<int>() { value },
                        DataLabels = true,
                        FontSize = 18,
                        LabelPoint = ShowLabel
                    });
                }
            }


            IsDialogOpen = false;

            GoodsCollection = gcv.View;
            GoodsCollection.Refresh();

            ReserveCollection = rcv.View;
            ReserveCollection.Refresh();

            Label = ShowLabelFormat;

            MoneyLabelFormatter = MoneyFormater;

            Budget = Properties.Settings.Default.Buget;

            TodaysTotalSales = PurchaseDeliveryService.GetTodaysTotalSales() / Budget * 100;


            #endregion

            #region Commands

            CloseWindowCommand = new RellayCommand(p => App.GetActiveWindow.Close());

            ChangeLanguageCommand = new RellayCommand(p =>
                    {
                        ResourceManagerService.ChangeLocale(p?.ToString() ?? "en-US");
                        Properties.Settings.Default["Language"] = p?.ToString() ?? "en-US";
                        Properties.Settings.Default.Save();
                        Properties.Settings.Default.Reload();

                        Months = new List<string>(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Take(12));
                        SelectedMonthIndex = 0;

                    }, p => !Equals(p?.ToString() ?? "us-US", Properties.Settings.Default.Language));

            OpenEditGoodsCommand = new RellayCommand(
                p =>
                {
                    var edit_dialog = new GoodsEditDialog()
                    {
                        DataContext = new GoodsEditDialogVM(this)
                    };

                    DialogContent = edit_dialog;
                    IsDialogOpen = true;
                }, p => CurrentSelectedGoods != null);

            AddNewGoodsCommand = new RellayCommand(OnAddNewGoods);

            DeleteGoodsCommand = new RellayCommand(OnDeleteGoods, p => CurrentSelectedGoods != null);

            SortCommand = new RellayCommand(OnSortCollection, p => !string.IsNullOrEmpty(FilterOption));

            CheckedCommand = new RellayCommand(OnChecked);

            AddReserveCommand = new RellayCommand(OnAddNewReserve);

            EditReserveCommand = new RellayCommand(OnEditReserve, p => SelectedReserve != null);

            DeleteReserveCommand = new RellayCommand(OnDeleteReserve, p => SelectedReserve != null);

            UpdateReserveTableCommand = new RellayCommand(OnReserveDateUpdate);

            SetBudgetCommand = new RellayCommand(OnSetBudget);

            MonthChangedCommand = new RellayCommand(OnSelectionMonthChanged);

            ColumnClickedCommand = new RellayCommand(OnColumnClicked);

            #endregion
        }



        #region Handlers

        private void OnColumnClicked(object Obj)
        {
            if (Obj is ChartPoint chart_point)
            {

                string data = chart_point.SeriesView.Model.CurrentXAxis.Labels[chart_point.Key];
                GoodsGroupSeries.Clear();

                var temp = PurchaseDeliveryService.GetTodaysPercentageOfGoods(data);

                if (temp != null)
                {
                    double tempSumVal = temp.Values.Sum();

                    string ShowLabel(ChartPoint cp) => $"{cp.Y} ({cp.Y / tempSumVal:p1})";



                    foreach ((string key, int value) in temp)
                    {
                        GoodsGroupSeries.Add(
                            new PieSeries()
                            {
                                Title = key,
                                Values = new ChartValues<int>() { value },
                                DataLabels = true,
                                FontSize = 18,
                                LabelPoint = ShowLabel
                            });
                    }
                }
            }

        }

        private string MoneyFormater(double Arg)
        {
            return Arg.ToString("## '円'");
        }

        private void OnSelectionMonthChanged(object Obj)
        {
            MonthDataValues = PurchaseDeliveryService.GetTotalSalesByMonth(SelectedMonthIndex + 1);
            if (MonthDataValues != null)
            {
                ColumnValues = new ChartValues<int>(MonthDataValues.Values);
                Dates.Clear();
                foreach (DateTime dt in MonthDataValues.Keys)
                    Dates.Add(dt.Date.ToString("MM/dd", CultureInfo.InvariantCulture) ?? "0");
            }
            else
            {
                ColumnValues.Clear();
                Dates.Clear();
            }

        }


        private void OnSetBudget(object Obj)
        {
            var sbg_dialog = new SetBudgetDialog()
            {
                DataContext = new SetBudgetDialogVM(this)
            };
            DialogContent = sbg_dialog;
            IsDialogOpen = true;
        }

        private string ShowLabelFormat(double value)
        {
            string temp = $"\n{Math.Floor(TodaysTotalSales * Budget / 100)}円 / {Budget}円";
            return $"{Math.Floor(TodaysTotalSales).ToString(CultureInfo.InvariantCulture).PadLeft(temp.Length - 1)}%"
                   + temp;
        }


        private void OnReserveDateUpdate(object Obj)
        {
            MessageBox.Show(
                $"Updated {ReservationServices.UpdadateReservedTable()} cases!", ResourceManagerService.GetResourceString("lang", "Title"), MessageBoxButton.OK,
                MessageBoxImage.Information);
            ReserveCollection.Refresh();
        }

        private void OnDeleteReserve(object Obj)
        {
            var res = System.Windows.MessageBox.Show("Delete the Reserve ID: " + SelectedReserve.ReserveId + "?", ResourceManagerService.GetResourceString("lang", "Title"), MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (res == MessageBoxResult.Cancel) return;
            ReservationServices.DeleteReserve(SelectedReserve.ReserveId);
            ReserveCollection.Refresh();
        }

        private void OnDeleteGoods(object Obj)
        {
            var res = System.Windows.MessageBox.Show("Delete the Goods Name: " + CurrentSelectedGoods.Name + "?", ResourceManagerService.GetResourceString("lang", "Title"), MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (res == MessageBoxResult.Cancel) return;
            GoodsService.DeleteGoods(CurrentSelectedGoods);
            GoodsCollection.Refresh();

        }

        private void OnEditReserve(object Obj)
        {
            var resAddDialog = new ReserveDialog()
            {
                DataContext = new ReserveDialogVM(this, ReserveMode.EditMode, SelectedReserve)
            };
            DialogContent = resAddDialog;
            IsDialogOpen = true;
        }

        private void OnAddNewReserve(object p)
        {
            var resAddDialog = new ReserveDialog()
            {
                DataContext = new ReserveDialogVM(this, ReserveMode.AddMode)
            };
            DialogContent = resAddDialog;
            IsDialogOpen = true;
        }

        private void OnChecked(object p)
        {
            if (p == null) return;
            FilterOption = p.ToString();
            IsAscending = true;
        }

        private void OnAddNewGoods(object Obj)
        {
            var add_dialog = new AddGoodsDialog()
            {
                DataContext = new AddGoodsDialogVM(this)
            };

            DialogContent = add_dialog;
            IsDialogOpen = true;
        }


        private void OnGoodsTableFilter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Goods g))
            {
                e.Accepted = false;
                return;
            }


            if (string.IsNullOrWhiteSpace(Search)) return;
            if (g.Name.Contains(Search, StringComparison.OrdinalIgnoreCase)) return;

            e.Accepted = false;
        }


        private void OnSortCollection(object Obj)
        {
            GoodsCollection.SortDescriptions.Clear();
            if (!IsAscending)
            {

                GoodsCollection.SortDescriptions.Add(new SortDescription(FilterOption, ListSortDirection.Ascending));
                IsAscending = true;
            }
            else
            {
                GoodsCollection.SortDescriptions.Add(new SortDescription(FilterOption, ListSortDirection.Descending));
                IsAscending = false;
            }
            gcv.View.Refresh();
        }

        #endregion

    }
}