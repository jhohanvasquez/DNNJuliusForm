namespace DNNJuliusForm.Common.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class js_Log
    {
        public long Id { get; set; }

        [Required]
        [StringLength(4000)]
        public string Message { get; set; }

        [StringLength(50)]
        public string Module { get; set; }

        public DateTime CreateOnDate { get; set; }
    }
}
