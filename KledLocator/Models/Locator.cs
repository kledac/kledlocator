using StackExchange.Redis;

namespace KledLocator.Models
{
    public class Locator
    {
        public string ip { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public Data data { get; set; }
        public PersonLocation Location { get; set; }
    }

    public class Data
    {
        public Geo geo {
            get;
            set;
        }
    }
}
