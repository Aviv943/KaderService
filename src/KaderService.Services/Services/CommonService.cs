using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KaderService.Logger;
using KaderService.Services.Constants;
using Newtonsoft.Json.Linq;

namespace KaderService.Services.Services
{
    public class CommonService
    {
        private readonly ILoggerManager _logger;

        public CommonService(ILoggerManager logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Will return "x,y"
        /// </summary>
        /// <param name="address">Hebrew only, without country</param>
        /// <returns></returns>
        public async Task<string> GetLocationAsync(string address)
        {
            using var client = new WebClient();

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new NullReferenceException("Address can NOT be null");
            }

            string geoLocationString = await client.DownloadStringTaskAsync($@"https://es.govmap.gov.il/TldSearch/api/DetailsByQuery?query={address}&lyrs=1&gid=govmap");
            JObject jObject = JObject.Parse(geoLocationString);
            JToken jToken = jObject["data"]["ADDRESS"]?[0] ?? jObject["data"]["STREET"]?[0];

            if (jToken == null)
            {
                _logger.LogDebug($"Address '{address}' could not be found, will try to remove the street number");
                string newAddress = Regex.Replace(address.Replace("ישראל", string.Empty), @"[\d-]", string.Empty);
                geoLocationString = await client.DownloadStringTaskAsync($@"https://es.govmap.gov.il/TldSearch/api/DetailsByQuery?query={newAddress}&lyrs=1&gid=govmap");
                jObject = JObject.Parse(geoLocationString);
                jToken = jObject["data"]["ADDRESS"]?[0] ?? jObject["data"]["STREET"]?[0];

                if (jToken == null)
                {
                    throw new KeyNotFoundException("Address could NOT be found");
                }
            }

            if (jObject["data"]["STREET"] != null)
            {
                //We will get here when the extact street could not be found
                var streetAddress = jObject["data"]["STREET"]?[0]["ResultLable"].ToString();
                _logger.LogInfo($"Extract street address {address} could not be found, new address: '{streetAddress}'");
            }

            return $"{(int) jToken["X"]},{(int) jToken["Y"]}";
        }
    }
}
