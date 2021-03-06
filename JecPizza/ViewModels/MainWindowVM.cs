using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using JecPizza.Infostucture.Assist;
using JecPizza.Infostucture.Command;
using JecPizza.Models;
using JecPizza.Services;
using JecPizza.ViewModels.Base;
using JecPizza.Views;
using JecPizza.Views.Dialogs;
using LiveCharts;
using LiveCharts.Wpf;
using LocalizatorHelper;
using Microsoft.Win32;

namespace JecPizza.ViewModels
{
    public class MainWindowVM : BaseViewModel
    {
        private static object locker = new object();
        public GoodsService GoodsService { get; set; }
        public ReservationService ReservationServices { get; set; }
        public PurchaseDeliveryService PurchaseDeliveryService { get; set; }
        public AccountService AccountService { get; set; }
        public readonly CollectionViewSource gcv = new CollectionViewSource();
        public readonly CollectionViewSource rcv = new CollectionViewSource();
        public readonly CollectionViewSource dcv = new CollectionViewSource();
        public readonly CollectionViewSource acv = new CollectionViewSource();

        public bool IsAscending { get; set; } = true;




        #region Properties

        #region ProgressMaxValue : int - Loading progress Max Value

        /// <summary>Loading progress Max Value</summary>
        private int _ProgressMaxValue;

        /// <summary>Loading progress Max Value</summary>
        public int ProgressMaxValue { get => _ProgressMaxValue; set => Set(ref _ProgressMaxValue, value); }

        #endregion

        #region ProgressValue : double - Loading Progress Current Value

        /// <summary>Loading Progress Current Value</summary>
        private double _ProgressValue;

        /// <summary>Loading Progress Current Value</summary>
        public double ProgressValue { get => _ProgressValue; set => Set(ref _ProgressValue, value); }

        #endregion

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
        private ObservableCollection<string> _Months;

        /// <summary>Months Item for Combo Box</summary>
        public ObservableCollection<string> Months { get => _Months; set => Set(ref _Months, value); }

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

        #region ReserveCollection : ICollectionView - Reservations Data

        /// <summary>Reservations Data</summary>
        private ICollectionView _ReserveCollection;

        /// <summary>Reservations Data</summary>
        public ICollectionView ReserveCollection { get => _ReserveCollection; set => Set(ref _ReserveCollection, value); }

        #endregion

        #region DelivartCollection : ICollectionView - Delivaries Data

        /// <summary>Delivaries Data</summary>
        private ICollectionView _DeliveryCollection;

        /// <summary>Delivaries Data</summary>
        public ICollectionView DeliveryCollection { get => _DeliveryCollection; set => Set(ref _DeliveryCollection, value); }

        #endregion

        #region AccountCollection : ICollectionView - Accounts Data

        /// <summary>Accounts Data</summary>
        private ICollectionView _AccountCollection;

        /// <summary>Accounts Data</summary>
        public ICollectionView AccountCollection { get => _AccountCollection; set => Set(ref _AccountCollection, value); }

        #endregion

        #region SelectedReserve : Reserve - Reserve Row Selected

        /// <summary>Reserve Row Selected</summary>
        private Reserve _SelectedReserve;

        /// <summary>Reserve Row Selected</summary>
        public Reserve SelectedReserve { get => _SelectedReserve; set => Set(ref _SelectedReserve, value); }

        #endregion

        #region SelectedAccount : Member - Selected Account itme

        /// <summary>Selected Account itme</summary>
        private Member _SelectedAccount;

        /// <summary>Selected Account itme</summary>
        public Member SelectedAccount
        {
            get => _SelectedAccount;
            set
            {
                Set(ref _SelectedAccount, value);
                ShowAccountInfo = (SelectedAccount != null) ? Visibility.Visible : Visibility.Collapsed;

            }

        }

        #endregion

        #region SearchAccount : string - Search Account in Account Group Box

        /// <summary>Search Account in Account Group Box</summary>
        private string _SearchAccount;

        /// <summary>Search Account in Account Group Box</summary>
        public string SearchAccount
        {
            get => _SearchAccount;
            set
            {
                Set(ref _SearchAccount, value);
                acv.View.Refresh();
            }
        }

        #endregion

        #region ShowAccountInfo : Visibility - Shwing Account Info while Account is selected

        /// <summary>Shwing Account Info while Account is selected</summary>
        private Visibility _ShowAccountInfo = Visibility.Collapsed;

        /// <summary>Shwing Account Info while Account is selected</summary>
        public Visibility ShowAccountInfo { get => _ShowAccountInfo; set => Set(ref _ShowAccountInfo, value); }

        #endregion

        #region ShowMultiplyAccountInfo : Visibility - Mlply Acc View

        /// <summary>Mlply Acc View</summary>
        private Visibility _ShowMultiplyAccountInfo = Visibility.Collapsed;

        /// <summary>Mlply Acc View</summary>
        public Visibility ShowMultiplyAccountInfo { get => _ShowMultiplyAccountInfo; set => Set(ref _ShowMultiplyAccountInfo, value); }

        #endregion

        #region SelectedAccoutes : IList<Member> - Multiply selectted Accounts

        /// <summary>Multiply selectted Accounts</summary>
        private IList<Member> _SelectedAccounts;

        /// <summary>Multiply selectted Accounts</summary>
        public IList<Member> SelectedAccounts { get => _SelectedAccounts; set => Set(ref _SelectedAccounts, value); }

        #endregion

        #region SelecteDelivery : Delivery - Selected Delevery

        /// <summary>Selected Delevery</summary>
        private Delivery _SelectedDelivery;

        /// <summary>Selected Delevery</summary>
        public Delivery SelectedDelivery { get => _SelectedDelivery; set => Set(ref _SelectedDelivery, value); }

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
        public ICommand AccountMultipleSelectedCommand { get; set; }
        public ICommand LoadProductsCommand { get; set; }
        public ICommand GoodsCsvReaderCommand { get; set; }
        public ICommand SendEmailCommand { get; set; }
        public ICommand DeliveryInfoCommand { get; set; }

        #endregion




        public MainWindowVM()
        {
            OverlayVM.GetInstance().Show = (str) =>
            {
                OverlayVM.GetInstance().Text = str;
            };

            ResourceManagerService.RegisterManager("lang", Content.Languages.Language.ResourceManager);
            ResourceManagerService.ChangeLocale(Properties.Settings.Default.Language);

            GoodsService = new GoodsService();
            GoodsService.SendEmailWhenAddGoods += GoodsServiceSendEmailWhenAddGoods;
            ReservationServices = new ReservationService();
            PurchaseDeliveryService = new PurchaseDeliveryService();
            AccountService = new AccountService();


            #region Properties

            ProgressMaxValue = 1;

            gcv.Filter += OnGoodsTableFilter;


            rcv.Source = ReservationServices.GetAllReserve();
            dcv.Source = PurchaseDeliveryService.GetAllDeliveries();
            acv.Filter += OnAccountFilter;
            acv.Source = AccountService.GetMembers();

            GoodsGroupSeries = new SeriesCollection();
            Months = new ObservableCollection<string>(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Take(12));
            BindingOperations.EnableCollectionSynchronization(Months, new object());
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
                foreach ((string key, int value) in temp)
                    GoodsGroupSeries.Add(new PieSeries()
                    {
                        Title = key,
                        Values = new ChartValues<int>() { value },
                        DataLabels = true,
                        FontSize = 18,
                        LabelPoint = cp => $"{cp.Y} ({cp.Y / tempSumVal:p1})"
                    });
            }

            ShowAccountInfo = Visibility.Collapsed;

            IsDialogOpen = false;


            ReserveCollection = rcv.View;
            ReserveCollection.Refresh();

            DeliveryCollection = dcv.View;
            DeliveryCollection.Refresh();

            AccountCollection = acv.View;
            AccountCollection.Refresh();


            Label = ShowLabelFormat;

            MoneyLabelFormatter = MoneyFormater;

            Budget = Properties.Settings.Default.Buget;

            TodaysTotalSales = PurchaseDeliveryService.GetTodaysTotalSales() / Budget * 100;


            #endregion

            #region Commands

            CloseWindowCommand = new RellayCommand(p => App.GetActiveWindow.Close());

            ChangeLanguageCommand = new RellayCommand(OnLanguageChanged, p => !Equals(p?.ToString() ?? "us-US", Properties.Settings.Default.Language));

            OpenEditGoodsCommand = new RellayCommand(OnEditGoods, p => CurrentSelectedGoods != null);

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

            AccountMultipleSelectedCommand = new RellayCommand(OnMultipleSelectedAccount);

            LoadProductsCommand = new RellayCommand(OnLoadProducts, p => GoodsCollection == null);

            GoodsCsvReaderCommand = new RellayCommand(OnGoodsFromCSV);

            SendEmailCommand = new RellayCommand(OnSendEmail, p => SelectedAccount != null);

            DeliveryInfoCommand = new RellayCommand(OnInfoClicked, p => SelectedDelivery != null);

            #endregion
        }



        #region Handlers

        private void OnInfoClicked(object Obj)
        {
            IsDialogOpen = true;
            var delInfo = new DeliveryInfoDialog()
            {
                DataContext = new DeleveryInfoVM(SelectedDelivery)
            };
            DialogContent = delInfo;
        }

        private async void OnGoodsFromCSV(object Obj)
        {
            OpenFileDialog opd = new OpenFileDialog
            {
                Multiselect = false,
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "Goods CSV File|*.csv;*.txt;"
            };


            if (opd.ShowDialog() != true) return;

            FileStream file = File.Open(opd.FileName, FileMode.Open, FileAccess.Read);
            List<Goods> goods = new List<Goods>();
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    string data = (await reader?.ReadLineAsync());
                    string[] fileds = data?.Split(',').Select((i, s) => i.Trim('\t')).ToArray();

                    if (fileds?.Length != 8)
                    {
                        MessageBox.Show("Not Correct CSV Format!");
                        return;
                    }
                    else
                    {
                        goods.Add(new Goods()
                        {
                            GoodsId = fileds[0],
                            Name = fileds[1],
                            Price = int.Parse(fileds[2]),
                            GoodsGroupId = fileds[3],
                            Image = fileds[4],
                            IsRecommend = fileds[5].Equals("1") ? true : false,
                            IsNew = fileds[6].Equals("1") ? true : false,
                            HasTopping = fileds[7].Equals("1") ? true : false

                        });
                    }
                }

            }

            var res = MessageBox.Show($"Detected {goods.Count} of Goods.\nWould u like to add it?", "JecPizzaAdmin", MessageBoxButton.YesNo, MessageBoxImage.Question);


            if (res == MessageBoxResult.Yes)
            {
                await Task.Factory.StartNew(
                    () =>
                    {
                        double i = 1;
                        OverlayVM.GetInstance().Show("Adding the Goods into the Table...\nPlease wait a while!");

                        foreach (Goods good in goods)
                        {
                            OverlayVM.GetInstance().Show($"Added the Goods {good.Name}");
                            ProgressValue = i / goods.Count;
                            i++;
                            Task.Delay(1000).Wait();
                        }

                        GoodsService.InsertRangeGoods(goods);

                        Task.Delay(1000).Wait();

                        OverlayVM.GetInstance().Close();
                    });

                MessageBox.Show($"{goods.Count} of Goods have been Added");
            }
        }

        private void OnSendEmail(object p)
        {

            IsDialogOpen = true;

            EmailTemplateDialog etd = new EmailTemplateDialog()
            {
                DataContext = new EmailDialogTempVM(this)
            };

            DialogContent = etd;

        }

        private async void GoodsServiceSendEmailWhenAddGoods(Goods g)
        {
            MailAddress fromAddress = new MailAddress("jecPizza@gmail.com", "JecPizza");
            MailAddress toAddress = new MailAddress("20jn0102@jec.ac.jp", "Fallen");
            const string fromPassword = "wvlffbfobhqbdsxi";
            string subject = "新商品登場!!!";
            string body = $"Fallen 様　本日より、新商品 {g.Name} 登場です。\n本日頼めば30％OFF!このチャンス逃がせずに！！！";

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            await smtp.SendMailAsync(message);
        }

        private void OnEditGoods(object p)
        {
            var edit_dialog = new GoodsEditDialog() { DataContext = new GoodsEditDialogVM(this) };

            DialogContent = edit_dialog;
            IsDialogOpen = true;
        }

        private void OnLanguageChanged(object p)
        {
            ResourceManagerService.ChangeLocale(p?.ToString() ?? "en-US");
            Properties.Settings.Default["Language"] = p?.ToString() ?? "en-US";
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

            Months = new ObservableCollection<string>(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Take(12));
            SelectedMonthIndex = 0;
        }

        private async void OnLoadProducts(object Obj)
        {
            ObservableCollection<Goods> temp_goods_list = new ObservableCollection<Goods>();

            await Task.Factory.StartNew(
                () =>
                {
                    double i = 1;
                    OverlayVM.GetInstance().Show("Loading the Goods Info...\nPlease wait a while!");
                    int cntItems = GoodsService.GetAllGoods().Count();

                    foreach (Goods goods in GoodsService.GetAllGoods())
                    {
                        temp_goods_list.Add(goods);

                        ProgressValue = i / cntItems;
                        i++;
                        Task.Delay(100).Wait();

                    }

                    OverlayVM.GetInstance().Close();
                });

            gcv.Source = temp_goods_list;
            GoodsCollection = gcv?.View;
        }

        private void OnMultipleSelectedAccount(object Obj)
        {
            if (Obj is IList accounts && accounts.Count > 1)
            {
                SelectedAccounts = new List<Member>(accounts.OfType<Member>());

                ShowAccountInfo = Visibility.Collapsed;
                ShowMultiplyAccountInfo = Visibility.Visible;
            }
            else
            {
                ShowAccountInfo = Visibility.Visible;
                ShowMultiplyAccountInfo = Visibility.Collapsed;
            }
        }

        private void OnAccountFilter(object Sender, FilterEventArgs e)
        {
            if (!(e.Item is Member g))
            {
                e.Accepted = false;
                return;
            }

            e.Accepted = string.IsNullOrWhiteSpace(SearchAccount) switch
            {
                false when !g.UserName.Contains(SearchAccount, StringComparison.OrdinalIgnoreCase) => false,
                _ => e.Accepted
            };
        }

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
                    foreach ((string key, int value) in temp)
                        GoodsGroupSeries.Add(
                            new PieSeries()
                            {
                                Title = key,
                                Values = new ChartValues<int>() { value },
                                DataLabels = true,
                                FontSize = 18,
                                LabelPoint = cp => $"{cp.Y} ({cp.Y / tempSumVal:p1})"
                            });
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

            if (gcv.Source is ObservableCollection<Goods> temp)
            {
                temp.Remove(CurrentSelectedGoods);
                gcv.Source = temp;
            }

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

            e.Accepted = string.IsNullOrWhiteSpace(Search) switch
            {
                false when !g.Name.Contains(Search, StringComparison.OrdinalIgnoreCase) => false,
                _ => e.Accepted
            };
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
//todo Send Mail Dialog
//todo MailTemplate and Saver Tempaltes
//todo CSV Products reader
