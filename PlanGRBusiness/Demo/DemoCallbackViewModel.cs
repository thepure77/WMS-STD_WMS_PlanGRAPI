using System;
using System.Collections.Generic;
using System.Text;

namespace PlanGRBusiness.Demo
{
    public class DemoCallbackViewModel
    {
        public string packageCode { get; set; }
        public string packageOriginType { get; set; }
        public string packageType { get; set; }
        public string firstmileTplSlug { get; set; }
        public string trackingNumber { get; set; }
        public string tplSlug { get; set; }
        public string tplName { get; set; }
        public PlatformInfo platformInfo { get; set; }
        public string deliveryServiceType { get; set; }
        public bool deliveryPriority { get; set; }
        public string pickupType { get; set; }
        public Payment payment { get; set; }
        public Dimweight dimweight { get; set; }
        public Shipper shipper { get; set; }
        public Origin origin { get; set; }
        public Destination destination { get; set; }
        public ReturnInfo returnInfo { get; set; }
        public List<CallBackItem> items { get; set; }
        public string api { get; set; }
    }

    public class Shipper
    {
        public string sellerId { get; set; }
    }

    public class Address
    {
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string zipCode { get; set; }
        public string details { get; set; }
    }

    public class Destination
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public Address address { get; set; }
    }

    public class Dimweight
    {
        public decimal? weight { get; set; }
        public decimal? volume { get; set; }
        public decimal? height { get; set; }
        public decimal? width { get; set; }
        public decimal? length { get; set; }
    }

    public class CallBackItem
    {
        public string name { get; set; }
        public string sku { get; set; }
        public int? quantity { get; set; }
        public ItemDW itemDW { get; set; }
    }

    public class ItemDW
    {
        public decimal? weight { get; set; }
        public decimal? volume { get; set; }
        public decimal? height { get; set; }
        public decimal? width { get; set; }
        public decimal? length { get; set; }
    }

    public class Origin
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public Address address { get; set; }
    }

    public class Payment
    {
        public string paymentType { get; set; }
        public string shippingType { get; set; }
        public string currency { get; set; }
    }

    public class PlatformInfo
    {
        public string platformName { get; set; }
        public string platformOrderNumber { get; set; }
        public string platformOrderId { get; set; }
    }

    public class ReturnInfo
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public Address address { get; set; }
    }
}
