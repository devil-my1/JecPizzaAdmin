using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JecPizza.Models;
using JecPizza.Models.MapModel;
using JecPizza.ViewModels.Base;
using Microsoft.Maps.MapControl.WPF;

namespace JecPizza.ViewModels
{
    public class DeleveryInfoVM : BaseViewModel
    {
        private static readonly string __API_KEY = "AIzaSyAsQmLsONUaK6rl8So_S8YUEqaI28mJI78";

        public Delivery SelectedDelivery { get; private set; }

        #region PurchaseGoodsCol : ObservableCollection<Goods> - SPurchase Goods CollectionRIPTION

        /// <summary>Purchase Goods Collection</summary>
        private ObservableCollection<Goods> _PurchaseGoodsCol;

        /// <summary>Purchase Goods Collection</summary>
        public ObservableCollection<Goods> PurchaseGoodsCol { get => _PurchaseGoodsCol; set => Set(ref _PurchaseGoodsCol, value); }

        #endregion

        #region ShopLocation : ShopLocation - Current Shop

        /// <summary>Current Shop</summary>
        private Location _ShopLocation;

        /// <summary>Current Shop</summary>
        public Location ShopLocation { get => _ShopLocation; set => Set(ref _ShopLocation, value); }

        #endregion

        #region CustomerLaction : Location - Customer Lacation

        /// <summary>Customer Lacation</summary>
        private Location _CustomerLocation;

        /// <summary>Customer Lacation</summary>
        public Location CustomerLocation { get => _CustomerLocation; set => Set(ref _CustomerLocation, value); }

        #endregion

        #region PolylinePoints : LocationCollection - Polyline Collection

        /// <summary>Polyline Collection</summary>
        private LocationCollection _PolylinePoints;

        /// <summary>Polyline Collection</summary>
        public LocationCollection PolylinePoints { get => _PolylinePoints; set => Set(ref _PolylinePoints, value); }

        #endregion

        #region Distance : string - Shop to Customer distance

        /// <summary>Shop to Customer distance</summary>
        private string _Distance;

        /// <summary>Shop to Customer distance</summary>
        public string Distance { get => _Distance; set => Set(ref _Distance, value); }

        #endregion

        #region Time : string - Time to deliver

        /// <summary>Time to deliver</summary>
        private string _Time;

        /// <summary>Time to deliver</summary>
        public string Time { get => _Time; set => Set(ref _Time, value); }

        #endregion

        #region ArrivalTime : string - Arrival Time

        /// <summary>Arrival Time</summary>
        private string _ArrivalTime;

        /// <summary>Arrival Time</summary>
        public string ArrivalTime { get => _ArrivalTime; set => Set(ref _ArrivalTime, value); }

        #endregion


        public DeleveryInfoVM(Delivery SelectedDelivery)
        {
            this.SelectedDelivery = SelectedDelivery;
            ShopLocation = new Location(35.69849, 139.6983568);
            CustomerLocation = new Location();
            PolylinePoints = new LocationCollection();

            CallWebAPIAsync().ConfigureAwait(false);
        }

        private async Task CallWebAPIAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://maps.googleapis.com/maps/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"api/directions/json?origin=place_id:ChIJVVUFGymNGGARckMyJ_Gzzbc&destination={SelectedDelivery.Address}&language={CultureInfo.CurrentCulture.Name}&key={__API_KEY}");

                if (response.IsSuccessStatusCode)
                {
                    MapObject dir = await response.Content.ReadAsAsync<MapObject>();

                    foreach (var val in dir?.routes[0]?.legs[0]!?.steps!)
                    {
                        //PolylinePoints.Add(new Location(val.start_location.lat, val.start_location.lng));
                        PolylinePoints.Add(new Location(val.end_location.lat, val.end_location.lng));
                    }
                    PolylinePoints.Add(CustomerLocation);

                    CustomerLocation.Latitude = dir.routes[0].legs[0].end_location.lat;
                    CustomerLocation.Longitude = dir.routes[0].legs[0].end_location.lng;
                    Time = dir.routes[0]?.legs[0]?.duration.text;
                    Distance = dir.routes[0]?.legs[0]?.distance.text;
                    ArrivalTime = DateTime.Now.AddMinutes(15).AddSeconds(dir.routes[0]?.legs[0]?.duration?.value ?? 0).ToLongTimeString();
                }

            }
        }


    }
}