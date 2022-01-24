using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using JecPizza.Infostucture.Command;
using JecPizza.Models;
using JecPizza.ViewModels.Base;
using Microsoft.Win32;

namespace JecPizza.ViewModels
{
    public class AddGoodsDialogVM : BaseViewModel
    {
        private BaseViewModel _host;
        private IDictionary<string, string> GgDictionary;

        #region NewGoods : Goods - New Goods Model

        /// <summary>New Goods Model</summary>
        private Goods _NewGoods;

        /// <summary>New Goods Model</summary>
        public Goods NewGoods { get => _NewGoods; set => Set(ref _NewGoods, value); }

        #endregion


        #region Image : string - Show up goods image 

        /// <summary>Show up goods image </summary>
        private string _Image;

        /// <summary>Show up goods image </summary>
        public string Image { get => _Image; set => Set(ref _Image, value); }

        #endregion

        public ICollection<string> GoodsGroup { get; set; }

        public ICommand ChangeImageCommand { get; set; }
        public ICommand AddGoodsCommand { get; set; }


        public AddGoodsDialogVM()
        {

        }

        public AddGoodsDialogVM(BaseViewModel host)
        {
            this._host = host;
            MainWindowVM mvm = (MainWindowVM)_host;

            _NewGoods = new Goods()
            {
                Image = @"C:\Study\School\JecPizza\JecPizza\Content\Images\GCGP.jpg",
                IsNew = true
            };

            Image = NewGoods.Image;

            GgDictionary = mvm.GoodsService.GetAllGoodsGroup();

            GoodsGroup = GgDictionary.Keys;


            ChangeImageCommand = new RellayCommand(OnImageChange);
            AddGoodsCommand = new RellayCommand(OnAddGoods, OnCanAdd);
        }

        private bool OnCanAdd(object p) => !string.IsNullOrEmpty(NewGoods.Name) && !string.IsNullOrEmpty(NewGoods.Price.ToString())
                                                                                && !string.IsNullOrEmpty(NewGoods.GoodsGroupId)
                                                                                && !string.IsNullOrEmpty(NewGoods.GoodsId);

        private void OnAddGoods(object Obj)
        {
            MainWindowVM mvm = (MainWindowVM)_host;
            var temp = GgDictionary[NewGoods.GoodsGroupId];
            NewGoods.GoodsGroupId = temp;
            var res = mvm.GoodsService.InsertGoods(NewGoods);


            System.Windows.MessageBox.Show(res ? LocalizatorHelper.ResourceManagerService.GetResourceString("lang", "SucAdd") : LocalizatorHelper.ResourceManagerService.GetResourceString("lang", "ErEdit"), "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            mvm.IsDialogOpen = false;

        }

        private void OnImageChange(object Obj)
        {
            var opd = new OpenFileDialog
            {
                Multiselect = false,
                InitialDirectory = Environment.CurrentDirectory.Remove(Environment.CurrentDirectory.IndexOf("\\bin", StringComparison.OrdinalIgnoreCase))
                                   + "\\Content\\Images\\",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;"
            };


            if (opd.ShowDialog() == true)
            {
                bool isExsitInDir = false;
                var curImgDir = new DirectoryInfo(Environment.CurrentDirectory.Remove(Environment.CurrentDirectory.IndexOf("\\bin", StringComparison.OrdinalIgnoreCase)) + "\\Content\\Images\\");
                var files = curImgDir.EnumerateFiles();

                var file = new FileInfo(opd.FileName);

                foreach (var f in files)
                    if (f.Name.Equals(opd.SafeFileName))
                        isExsitInDir = true;


                if (!isExsitInDir)
                    File.Copy(opd.FileName, curImgDir.FullName + opd.SafeFileName);

                NewGoods.Image = curImgDir.FullName + opd.SafeFileName;
                Image = NewGoods.Image;
            }
        }


    }
}