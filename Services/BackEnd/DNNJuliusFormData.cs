using DotNetNuke.Data;
using DNNJuliusModules.DNNJuliusForm.Services.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Web;
using DNNJuliusModules.DNNJuliusForm.ServiceReferenceDNNJuliusForm;

namespace DNNJuliusModules.DNNJuliusForm.Services.BackEnd
{
    public class DNNJuliusFormData
    {
        public js_DNNJuliusForm Add(js_DNNJuliusForm model)
        {
            using (IDataContext context = DataContext.Instance())
            {
                using (ServiceTestClient oServiceTestClient = new ServiceTestClient())
                {
                    model.FechaCreacion = DateTime.Now;
                    return oServiceTestClient.GuardarDNNJuliusForm(model);
                }
            }
        }

        public js_DNNJuliusFormArchivo AddFile(js_DNNJuliusFormArchivo model)
        {
            using (IDataContext context = DataContext.Instance())
            {
                using (ServiceTestClient oServiceTestClient = new ServiceTestClient())
                {
                    return oServiceTestClient.GuardarDNNJuliusFormArchivo(model);
                }
            }
        }

        public void RollBack(js_DNNJuliusForm model)
        {

            using (IDataContext context = DataContext.Instance())
            {
                var rep = context.GetRepository<js_DNNJuliusForm>();
                var project = rep.GetById(model.Id);
                if (project != null)
                {
                    rep.Delete(project);
                }

                var repFile = context.GetRepository<DNNJuliusFormFileModel>();
                var files = repFile.Find("where DNNJuliusFormId = @0", model.Id);
                if (files != null && files.Count() > 0)
                {
                    foreach (var item in files)
                    {
                        repFile.Delete(item);
                        if (File.Exists($"{HttpRuntime.AppDomainAppPath}{item.RutaFisica.Replace("~", "")}"))
                        {
                            File.Delete($"{HttpRuntime.AppDomainAppPath}{item.RutaFisica.Replace("~", "")}");
                        }
                    }
                }
            }

        }
    }
}