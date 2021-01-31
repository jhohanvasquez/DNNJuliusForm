namespace DNNJuliusForm.Common.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model : DbContext
    {
        public Model()
            : base("name=SiteSqlServer")
        {
        }

        public virtual DbSet<js_City> js_City { get; set; }
        public virtual DbSet<js_Country> js_Country { get; set; }
        public virtual DbSet<js_Departament> js_Departament { get; set; }
        public virtual DbSet<js_Log> js_Log { get; set; }
        public virtual DbSet<js_Messages> js_Messages { get; set; }
        public virtual DbSet<js_Parameters> js_Parameters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
