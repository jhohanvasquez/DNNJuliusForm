using DNNJuliusForm.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DNNJuliusForm.Common.Data
{
    public class CityData
    {
        public BusinessResult<List<js_City>> GetCities(int departamentId)
        {
            try
            {
                using (var context = new Model.Model())
                {
                    var data = context.js_City.Where(c => c.Show && c.IdDepartament == departamentId);
                    return BusinessResult<List<js_City>>.Sucess(data.ToList(), string.Empty);
                }
            }
            catch (Exception ex)
            {
                return BusinessResult<List<js_City>>.Issue(null, "Error obteniendo las ciudades de la BD", ex);
            }
        }
    }
}
