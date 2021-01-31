using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNNJuliusForm.Common.Dto
{
    public class FileUploadDto
    {
        public string Base { get; set; }
        public string Source { get; set; }
        public string fileName { get; set; }
        public string[] AllowedExtensions { get; set; }
        public string SavePath { get; set; }
    }
}
