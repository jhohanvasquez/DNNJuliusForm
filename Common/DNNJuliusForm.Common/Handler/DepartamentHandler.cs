using DotNetNuke.Data;
using DNNJuliusForm.Common.Data;
using DNNJuliusForm.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace DNNJuliusForm.Common.Handler
{
    public class DepartamentHandler
    {
        public List<js_Departament> GetDepartamentByIdCountry(int countryId)
        {
            ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;
            var dep = cache.Get($"DepartamentDataCollectionCache{countryId}");
            List<js_Departament> departaments = null;
            if (dep == null)
            {
                DepartamentData departamentData = new DepartamentData();

                var success = departamentData.GetDepartaments(countryId);
                cache.Set($"DepartamentDataCollectionCache{countryId}", success.IsSuccess ? success.Result : null, DateTimeOffset.Now.AddHours(1));
                departaments = success.IsSuccess ? success.Result : null;
            }
            else { departaments = (List<js_Departament>)cache.Get($"DepartamentDataCollectionCache{countryId}"); }
            return departaments;
        }
    }
}
