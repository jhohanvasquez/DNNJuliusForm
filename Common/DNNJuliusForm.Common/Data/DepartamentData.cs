using DNNJuliusForm.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNNJuliusForm.Common.Data
{
    public class DepartamentData
    {
        public BusinessResult<List<js_Departament>> GetDepartaments(int countryId)
        {
            try
            {
                //new Exception();

                using (var context = new Model.Model())
                {
                    var data = context.js_Departament.Where(c => c.Show && c.IdCountry == countryId);
                    return BusinessResult<List<js_Departament>>.Sucess(data.ToList(), string.Empty);
                }
            }
            catch (Exception ex)
            {
                return BusinessResult<List<js_Departament>>.Issue(null, "Error obteniendo los departamentos de la BD", ex);
            }
        }
    }
}
