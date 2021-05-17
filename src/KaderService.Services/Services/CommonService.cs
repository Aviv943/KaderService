using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using Newtonsoft.Json.Linq;

namespace KaderService.Services.Services
{
    public class CommonService
    {
        /// <summary>
        /// Will return "x,y"
        /// </summary>
        /// <param name="address">Hebrew only, without country</param>
        /// <returns></returns>
        public async Task<string> GetLocationAsync(string address)
        {
            using var client = new WebClient();
            string geoLocationString = await client.DownloadStringTaskAsync($@"https://es.govmap.gov.il/TldSearch/api/DetailsByQuery?query={address}&lyrs=1&gid=govmap");
            JObject jObject = JObject.Parse(geoLocationString);
            JToken jToken = jObject["data"]["ADDRESS"]?[0] ?? jObject["data"]["STREET"]?[0];

            if (jToken == null)
            {
                throw new KeyNotFoundException("Cannot find this street");
            }

            if (jObject["data"]["STREET"] != null)
            {
                //We will get here when the extact street could not be found
                //var streetAddress = jObject["data"]["STREET"]?[0]["ResultLable"].ToString();
                //address.Address1 = streetAddress.Split(",")[0].Trim();
                //address.City = streetAddress.Split(",")[1].Trim();
            }

            return $"{(int) jToken["X"]},{(int) jToken["Y"]}";
        }
    }
}
