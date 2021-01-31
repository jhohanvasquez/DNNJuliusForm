using DotNetNuke.ComponentModel.DataAnnotations;

namespace DNNJuliusForm.Common.Model
{
    [TableName("js_Departament")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class DepartamentModel
    {
        public int Id { get; set; }
        public int IdCountry { get; set; }
        public string Name { get; set; }
        public string Show { get; set; }
    }
}
