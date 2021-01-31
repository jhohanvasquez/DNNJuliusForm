namespace DNNJuliusForm.Common.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class js_Departament
    {
        public int Id { get; set; }

        public int IdCountry { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public bool Show { get; set; }
    }
}
