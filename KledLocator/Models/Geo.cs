using System.Net;

namespace KledLocator.Models
{
    public class Geo
    {
        public string host { get; set; }
        public string ip { get; set; }
        public string rdns { get; set; }
        public string asn { get; set; }
        public string isp { get; set; }
        public string country_name { get; set; }
        public string country_code { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        public string postal_code { get; set; }
        public string continent_code {get; set;}
        public double latitude {get; set;}
        public double longitude {get; set;}
        public double dma_code {get; set;}
        public double area_code {get; set;}
        public string timezone {get; set;}
        public string datetime {get; set;}
    }
}

//"host":"www.google.com",
//"ip":"74.125.29.147",
//"rdns":"qg-in-f147.1e100.net",
//"asn":"AS15169",
//"isp":"Google Inc. ",
//"country_name":"United States",
//"country_code":"US",
//"region":"CA",
//"city":"Mountain View",
//"postal_code":"94043",
//"continent_code":"NA",
//"latitude":37.419200897217,
//"longitude":-122.05740356445,
//"dma_code":807,
//"area_code":650,
//"timezone":"America\/Los_Angeles",
//"datetime":"2015-05-22 09:10:35"