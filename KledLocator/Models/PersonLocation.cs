using System.Globalization;

namespace KledLocator.Models
{
    public class PersonLocation
    {
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        public PersonLocation(string _cityName, string _stateName, string _countryName, double? _latitude,
            double? _longitude)
        {
            CityName = _cityName;
            StateName = _stateName;
            CountryName = _countryName;
            Latitude = _latitude;
            Longitude = _longitude;
        }
    }
}