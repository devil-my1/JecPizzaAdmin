using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using JecPizza.Infostructure.Command;
using JecPizza.Models;
using JecPizza.ViewModels.Base;
using Microsoft.Win32;

namespace JecPizza.ViewModels
{
    public class GoodsEditDialogVM : BaseViewModel
    {
        #region CurrentGoods : Goods - Current Goods

        private Goods _CurrentGoods;

        /// <summary>Current Goods</summary>
        public Goods CurrentGoods { get => _CurrentGoods; set => Set(ref _CurrentGoods, value); }

        #endregion

        #region Host : BaseViewModel - Dialog Host

        /// <summary>Dialog Host</summary>
        private BaseViewModel _Host;

        /// <summary>Dialog Host</summary>
        public BaseViewModel Host { get => _Host; set => Set(ref _Host, value); }

        #endregion

        #region Image : string - Goods Image

        /// <summary>Goods Image</summary>
        private string _Image;

        /// <summary>Goods Image</summary>
        public string Image { get => _Image; set => Set(ref _Image, value); }

        #endregion

        public ICommand ChangeCommand { get; set; }
        public ICommand ChangeImageCommand { get; set; }


        public GoodsEditDialogVM()
        {

        }

        public GoodsEditDialogVM(BaseViewModel host)
        {
            this.Host = host;
            MainWindowVM mvm = (MainWindowVM)host;
            this.CurrentGoods = (Goods)mvm.CurrentSelectedGoods.Clone();
            var tempGoods = mvm.CurrentSelectedGoods;
            Image = CurrentGoods.Image;

            #region Commands

            ChangeCommand = new RellayCommand(OnChange, p => CurrentGoods != tempGoods);

            ChangeImageCommand = new RellayCommand(OnImageChange);

            #endregion
        }


        #region Command Handlerts

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

                CurrentGoods.Image = curImgDir.FullName + opd.SafeFileName;
                Image = CurrentGoods.Image;
            }
        }

        private void OnChange(object Obj)
        {
           MainWindowVM mvm = (MainWindowVM)Host;
            bool eres = mvm.GoodsService.EditGoods(CurrentGoods);

            
            mvm.GoodsCollection.Refresh();

            mvm.IsDialogOpen = false;
            mvm.CurrentSelectedGoods = mvm.GoodsCollection.SourceCollection.Cast<Goods>().FirstOrDefault(g => g.GoodsId.Equals(CurrentGoods.GoodsId));

            System.Windows.MessageBox.Show(eres ? LocalizatorHelper.ResourceManagerService.GetResourceString("lang", "SucEdit") : LocalizatorHelper.ResourceManagerService.GetResourceString("lang", "ErEdit"), "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

    }

}