using GogWishlist.classes.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GogWishlist.classes.controller
{

    class UpdateResponse
    {

        public enum ResponseType { type_ok, type_error_web, type_error_response_empty, type_error_response_cache_not_found, type_error_wishlist_data, type_error_wishlist_products_not_found, type_error_username_empty, }

        public readonly ResponseType Type;
        public readonly string Message;

        public UpdateResponse(ResponseType type, string Message)
        {
            this.Type = type;
            this.Message = Message;
        }
    }

    class ControllerData
    {

        public enum SortOrderEnum { price, discount }

        private static ControllerData instance;
        public static ControllerData Instance
        {
            get
            {
                if (instance == null) instance = new ControllerData();
                return instance;
            }
        }
        public string Username { private get; set; }
        private readonly string prefix = "var gogData =";
        private readonly string suffix = ";";
        private readonly string product_page_format = "https://www.gog.com{0}";
        private readonly string wishlist_format = "http://gog.com/u/{0}/wishlist";
        private readonly string request_json_format = "https://www.gog.com/public_wishlist/{0}/search?page={1}&sortBy=title";

        private WebClient client;
        private List<Product> data;
        public List<Product> Data { get; private set; }
        private SortOrderEnum sortOrder;

        private bool filterDiscounted;
        public bool FilterDiscounted
        {
            set
            {
                filterDiscounted = value;
                filterData();
            }
        }

        public SortOrderEnum SortOrder
        {
            set
            {
                sortOrder = value;
                sortData();
            }
        }

        private void sortData()
        {
            if (Data == null) return;
            if (sortOrder == SortOrderEnum.price)
                Data = Data.OrderBy(o => o.price.amount).ToList();
            else
                Data = Data.OrderByDescending(o => o.price.discountPercentage).ToList();
        }

        private ControllerData()
        {
            client = new WebClient();
            sortOrder = SortOrderEnum.price;
            filterDiscounted = false;
        }

        public async Task<UpdateResponse> updateData()
        {
            string response;

            if (String.IsNullOrEmpty(Username))
                return new UpdateResponse(UpdateResponse.ResponseType.type_error_username_empty, null);

            try
            {
                response = await client.DownloadStringTaskAsync(new Uri(String.Format(wishlist_format, Username)));
            }
            catch (WebException e)
            {
                return new UpdateResponse(UpdateResponse.ResponseType.type_error_web, e.Message);
            }

            if (String.IsNullOrEmpty(response))
                return new UpdateResponse(UpdateResponse.ResponseType.type_error_response_empty, null);

            Match match = Regex.Match(response, prefix + "(.*)" + suffix);

            if (!match.Success)
                return new UpdateResponse(UpdateResponse.ResponseType.type_error_response_cache_not_found, null);

            String gogData = match.Value.Replace(prefix, "").Replace(suffix, "");

            if (String.IsNullOrEmpty(gogData))
                return new UpdateResponse(UpdateResponse.ResponseType.type_error_response_cache_not_found, null);

            Wishlist wishlist = null;
            try
            {
                wishlist = JsonConvert.DeserializeObject<Wishlist>(gogData);
            }
            catch (JsonException e) { }

            if (wishlist == null || wishlist.userInfo == null || wishlist.totalPages == 0)
                return new UpdateResponse(UpdateResponse.ResponseType.type_error_wishlist_data, null);

            string userId = wishlist.userInfo.id;
            int totalPages = wishlist.totalPages;

            Price.code = wishlist.currentCurrency != null ? wishlist.currentCurrency.code : "";
            data = null;
            for (int i = 1; i <= totalPages; ++i)
            {
                String url = String.Format(request_json_format, userId, i);
                response = await client.DownloadStringTaskAsync(new Uri(url));
                System.Diagnostics.Debug.WriteLine(response);
                if (String.IsNullOrEmpty(response))
                    continue;

                Wishlist wishlistPage = null;
                try
                {
                    wishlistPage = JsonConvert.DeserializeObject<Wishlist>(response);
                }
                catch (JsonException e) { }
                if (wishlistPage == null || wishlistPage.products == null)
                    continue;
                if (data == null) data = wishlistPage.products;
                else data.AddRange(wishlistPage.products);
            }

            if (data == null || data.Count == 0)
                return new UpdateResponse(UpdateResponse.ResponseType.type_error_wishlist_products_not_found, null);

            filterData();

            return new UpdateResponse(UpdateResponse.ResponseType.type_ok, null);

        }

        private void filterData()
        {
            if (data == null) return;
            Data = null;
            if (!filterDiscounted)
            {
                Data = data;
                sortData();
                return;
            }

            Data = data.Where(o => o.price.amount < o.price.baseAmount && o.price != null).ToList();
            sortData();
        }


        public string ItemPageUrl(int index)
        {
            if (Data == null
                || index < 0
                || Data.Count <= index
                || String.IsNullOrEmpty(Data[index].url))
                return null;

            return String.Format(product_page_format, Data[index].url);
        }
    }
}
