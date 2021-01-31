using DotNetNuke.ComponentModel.DataAnnotations;

namespace DNNJuliusForm.Common.Model
{
    [TableName("js_City")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class CityModel
    {
        public int Id { get; set; }
        public int IdDepartament { get; set; }
        public string Name { get; set; }
        public string Show { get; set; }
    }
}
