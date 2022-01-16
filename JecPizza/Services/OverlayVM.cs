using System;
using JecPizza.ViewModels.Base;

namespace JecPizza.Services
{
    public class OverlayVM : BaseViewModel
    {
        private static readonly OverlayVM _Instance = new OverlayVM();

        public static OverlayVM GetInstance() => _Instance;

        private OverlayVM() { }

        public Action<string> Show { get; set; }

        #region Text : string - DESCRIPTION

        /// <summary>DESCRIPTION</summary>
        private string _Text;

        /// <summary>DESCRIPTION</summary>
        public string Text { get => _Text; set => Set(ref _Text, value); }

        #endregion

        public void Close()
        {
            Text = "";
        }
    }
}