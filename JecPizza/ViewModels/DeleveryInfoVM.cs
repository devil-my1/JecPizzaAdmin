using System.Collections.ObjectModel;
using JecPizza.Models;
using JecPizza.ViewModels.Base;

namespace JecPizza.ViewModels
{
    public class DeleveryInfoVM:BaseViewModel
    {
        #region PurchaseGoodsCol : ObservableCollection<Goods> - SPurchase Goods CollectionRIPTION

        /// <summary>Purchase Goods Collection</summary>
        private ObservableCollection<Goods> _PurchaseGoodsCol;

        /// <summary>Purchase Goods Collection</summary>
        public ObservableCollection<Goods> PurchaseGoodsCol { get => _PurchaseGoodsCol; set => Set(ref _PurchaseGoodsCol, value); }

        #endregion

        public DeleveryInfoVM()
        {
            
        }
    }
}