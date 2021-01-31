using DotNetNuke.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNNJuliusModules.DNNJuliusForm.Services.ViewModels
{
    [TableName("js_DNNJuliusFormArchivo")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class DNNJuliusFormFileModel
    {
        public long Id { get; set; }
        public long DNNJuliusFormId { get; set; }
        public string RutaFisica { get; set; }
        public string Fuente { get; set; }
    }
}