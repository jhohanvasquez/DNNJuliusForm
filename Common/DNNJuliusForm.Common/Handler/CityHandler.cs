using DotNetNuke.Data;
using DNNJuliusForm.Common.Data;
using DNNJuliusForm.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace DNNJuliusForm.Common.Handler
{
    public class CityHandler
    {
        public List<js_City> GetCityByDepartamentId(int departamentId)
        {
            ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;
            var dep = cache.Get($"CityDataCollectionCache{departamentId}");
            List<js_City> cities = null;
            if (dep == null)
            {
                CityData cityData = new CityData();
                var success = cityData.GetCities(departamentId);
                cache.Set($"CityDataCollectionCache{departamentId}", success.IsSuccess ? success.Result : null, DateTimeOffset.Now.AddHours(1));
                cities = success.IsSuccess ? success.Result : null;
            }
            else { cities = (List<js_City>)cache.Get($"CityDataCollectionCache{departamentId}"); }
            return cities;
        }
    }
}
