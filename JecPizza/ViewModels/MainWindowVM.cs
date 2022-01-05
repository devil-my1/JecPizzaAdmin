using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using JecPizza.Infostructure.Command;
using JecPizza.Models;
using JecPizza.Services;
using JecPizza.ViewModels.Base;
using JecPizza.Views.Dialogs;
using LocalizatorHelper;

namespace JecPizza.ViewModels
{
    public class MainWindowVM : BaseViewModel
    {
        public GoodsService GoodsService { get; set; }
        public readonly CollectionViewSource cv = new CollectionViewSource();
        private ICollectionView _collectionView;
        public bool IsAscending { get; set; } = true;




        #region Properties

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
                cv.View.Refresh();
            }
        }

        #endregion

        #region FilterOption : string - Filter option

        /// <summary>Filter option</summary>
        private string _FilterOption;

        /// <summary>Filter option</summary>
        public string FilterOption { get => _FilterOption; set => Set(ref _FilterOption, value); }

        #endregion


        public ICollectionView GoodsCollection { get => _collectionView; set => Set(ref _collectionView, value); }

        #endregion


        #region Commands

        public ICommand CloseWindowCommand { get; }
        public ICommand ChangeLanguageCommand { get; }
        public ICommand OpenEditGoodsCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand AddNewGoodsCommand { get; set; }
        public ICommand SortCommand { get; set; }
        public ICommand CheckedCommand { get; set; }

        #endregion




        public MainWindowVM()
        {
            ResourceManagerService.RegisterManager("lang", Content.Languages.Language.ResourceManager);
            ResourceManagerService.ChangeLocale(Properties.Settings.Default.Language);
            GoodsService = new GoodsService();

            cv.Filter += OnGoodsTableFilter;
            cv.Source = GoodsService.GetAllGoods();
            cv.View.Refresh();
            GoodsCollection = cv.View;
            GoodsCollection.Refresh();

            #region Properties

            IsDialogOpen = false;

            #endregion

            #region Commands

            CloseWindowCommand = new RellayCommand(p => App.GetActiveWindow.Close());

            ChangeLanguageCommand = new RellayCommand(p =>
            {
                ResourceManagerService.ChangeLocale(p?.ToString() ?? "en-US");
                Properties.Settings.Default["Language"] = p?.ToString() ?? "en-US";
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
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

            SortCommand = new RellayCommand(OnSortCollection, p => !string.IsNullOrEmpty(FilterOption));

            CheckedCommand = new RellayCommand(OnChecked);

            #endregion
        }


        #region Handlers

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
            cv.View.Refresh();
        }

        #endregion

    }
}