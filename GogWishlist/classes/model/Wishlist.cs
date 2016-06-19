using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GogWishlist.classes.model
{
    public class CustomAttributes
    {
        public int xp { get; set; }
    }

    public class Price
    {
        public static string code;
        public string Amount { get { return code == null ? amount.ToString() : String.Format("{0} {1}", amount, code); } }
        public string BaseAmount { get { return code == null ? baseAmount.ToString() : String.Format("{0} {1}", baseAmount, code); } }

        private string discountFormat = "{0} %";
        public int amount { get; set; }
        public int baseAmount { get; set; }
        public int finalAmount { get; set; }
        public bool isDiscounted { get; set; }

        public string DiscountPercent { get { return String.Format(discountFormat, discountPercentage); } }
        public int discountPercentage { get; set; }
        public int discountDifference { get; set; }
        public string symbol { get; set; }
        public bool isFree { get; set; }
        public int discount { get; set; }
        public bool isBonusStoreCreditIncluded { get; set; }
        public int bonusStoreCreditAmount { get; set; }
    }

    public class Availability
    {
        public bool isAvailable { get; set; }
        public bool isAvailableInAccount { get; set; }
    }

    public class FromObject
    {
        public string date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
    }

    public class ToObject
    {
        public string date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
    }

    public class SalesVisibility
    {
        public bool isActive { get; set; }
        public FromObject fromObject { get; set; }
        public int from { get; set; }
        public ToObject toObject { get; set; }
        public int to { get; set; }
    }

    public class WorksOn
    {
        public bool Windows { get; set; }
        public bool Mac { get; set; }
        public bool Linux { get; set; }
    }

    public class Product
    {
        public CustomAttributes customAttributes { get; set; }
        public Price price { get; set; }
        public bool isDiscounted { get; set; }
        public bool isInDevelopment { get; set; }
        public int id { get; set; }
        public int? releaseDate { get; set; }
        public Availability availability { get; set; }
        public SalesVisibility salesVisibility { get; set; }
        public bool buyable { get; set; }
        public string title { get; set; }
        public string Image { get { return String.IsNullOrEmpty(image) ? image : "http:" + image + "_100.jpg"; } set { image = value; } }
        private string image;
        public string url { get; set; }
        public string supportUrl { get; set; }
        public string forumUrl { get; set; }
        public WorksOn worksOn { get; set; }
        public string category { get; set; }
        public string originalCategory { get; set; }
        public int rating { get; set; }
        public int type { get; set; }
        public bool isComingSoon { get; set; }
        public bool isPriceVisible { get; set; }
        public bool isMovie { get; set; }
        public bool isGame { get; set; }
        public string slug { get; set; }
    }

    public class Filters
    {
        public List<string> category { get; set; }
        public List<string> language { get; set; }
        public List<string> feature { get; set; }
        public List<string> system { get; set; }
        public List<string> price { get; set; }
    }

    public class Avatars
    {
        public string small { get; set; }
        public string small2x { get; set; }
        public string medium { get; set; }
        public string medium2x { get; set; }
        public string large { get; set; }
        public string large2x { get; set; }
    }

    public class UserInfo
    {
        public string id { get; set; }
        public string username { get; set; }
        public int userSince { get; set; }
        public Avatars avatars { get; set; }
    }

    public class CurrentCurrency
    {
        public string code { get; set; }
        public string symbol { get; set; }
    }

    public class AvailableCurrency
    {
        public string code { get; set; }
        public string symbol { get; set; }
    }

    public class AvailableLanguage
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class DateFormats
    {
        public string tiny { get; set; }
    }

    public class Wishlist
    {
        public string sortBy { get; set; }
        public int page { get; set; }
        public int totalProducts { get; set; }
        public int totalPages { get; set; }
        public int productsPerPage { get; set; }
        public object contentSystemCompatibility { get; set; }
        public List<Product> products { get; set; }
        public Filters filters { get; set; }
        public UserInfo userInfo { get; set; }
        public bool anonymous_personalization { get; set; }
        public CurrentCurrency currentCurrency { get; set; }
        public List<AvailableCurrency> availableCurrencies { get; set; }
        public string currentLanguage { get; set; }
        public List<AvailableLanguage> availableLanguages { get; set; }
        public DateFormats dateFormats { get; set; }
        public int now { get; set; }
        public string currentCountry { get; set; }
        public int personalizationEndpointCacheTtl { get; set; }
    }
}
