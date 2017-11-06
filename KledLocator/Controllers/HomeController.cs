using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KledLocator.Models;
using MaxMind.Db;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace KledLocator.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = new Locator();
            Regex regexIp = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            model.ip = GetRequestIP();
            try
            {
                model.ip = model.ip.Split(':', StringSplitOptions.RemoveEmptyEntries)[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }

            
            HttpClient client = new HttpClient();

            

            PersonLocation newModel = null;
                if (regexIp.Match(model.ip).Success)
                {
                    newModel = await getFromDb(model.ip);
                }
                else
                {
                    model.ip = "Defined to: 177.156.131.41";
                    newModel = await getFromDb("177.156.131.41");
                }
            
            if (newModel != null)
            {
                model.Location = newModel;
                
            }


            return View(model);
        }

        public async Task<PersonLocation> getFromDb(string ip)
        {
            PersonLocation returnObj = null;

            using (var reader = new DatabaseReader("App_Data/GeoLite2-City.mmdb"))
            {
                // Replace "City" with the appropriate method for your database, e.g.,
                // "Country".
                var city  = reader.City(ip);
                returnObj = new PersonLocation(city.City.Name, city.MostSpecificSubdivision.Name,city.Country.Name,city.Location.Latitude,city.Location.Longitude);

                //Console.WriteLine(returnObj.Country.IsoCode); // 'US'
                //Console.WriteLine(returnObj.Country.Name); // 'United States'
                //Console.WriteLine(returnObj.Country.Names["zh-CN"]); // '美国'

                //Console.WriteLine(returnObj.MostSpecificSubdivision.Name); // 'Minnesota'
                //Console.WriteLine(returnObj.MostSpecificSubdivision.IsoCode); // 'MN'

                //Console.WriteLine(returnObj.City.Name); // 'Minneapolis'

                //Console.WriteLine(returnObj.Postal.Code); // '55455'

                //Console.WriteLine(returnObj.Location.Latitude); // 44.9733
                //Console.WriteLine(returnObj.Location.Longitude); // -93.2323
            }

            return returnObj;
        }

        public async Task<Locator> getLocator(string ip)
        {
            HttpClient client = new HttpClient();


            var obj = await client.GetAsync(new Uri(string.Format("https://tools.keycdn.com/geo.json?host={0}", ip)));
            var data = obj.Content.ReadAsStringAsync().Result;
            return string.IsNullOrEmpty(data) ? new Locator(): JsonConvert.DeserializeObject<Locator>(data);
            
        }


        public string GetRequestIP(bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                ip = GetHeaderValueAs<string>("X-Forwarded-For").SplitCsv().FirstOrDefault();
            
            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (ip.IsNullOrWhitespace() && HttpContext?.Connection?.RemoteIpAddress != null)
                ip = HttpContext.Connection.RemoteIpAddress.ToString();

            if (ip.IsNullOrWhitespace())
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (ip.IsNullOrWhitespace())
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        public T GetHeaderValueAs<T>(string headerName)
        {
            StringValues values;

            if (HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!rawValues.IsNullOrWhitespace())
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }



        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
