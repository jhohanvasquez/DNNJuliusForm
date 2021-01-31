using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Mail;
using DNNJuliusForm.Common;
using DNNJuliusForm.Common.Dto;
using DNNJuliusForm.Common.Handler;
using DNNJuliusModules.DNNJuliusForm.Services.Dto;
using DNNJuliusModules.DNNJuliusForm.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using DNNJuliusModules.DNNJuliusForm.ServiceReferenceDNNJuliusForm;

namespace DNNJuliusModules.DNNJuliusForm.Services.BackEnd
{
    public class DNNJuliusFormBusiness
    {
        private const string UrlResx = "/DesktopModules/DNNJuliusForm/App_LocalResources/View.resx";

        public string SubjectEmail
        {
            get
            {
                return Localization.GetString("SubjectEmail.Text", UrlResx);
            }
            set { }
        }

        public string FromEmail
        {
            get
            {
                return Localization.GetString("FromEmail.Text", UrlResx);
            }
            set { }
        }

        public string ThemeEmail
        {
            get
            {
                return Localization.GetString("ThemeEmail.Text", UrlResx);
            }
            set { }
        }

        public string CopyEmail
        {
            get
            {
                return Localization.GetString("CopyEmail.Text", UrlResx);
            }
            set { }
        }

        DNNJuliusFormData DNNJuliusFormData;
        /// <summary>
        /// Guarda en base de datos la información del form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BusinessResult<js_DNNJuliusForm> Add(js_DNNJuliusForm model)
        {
            try
            {
                DNNJuliusFormData = new DNNJuliusFormData();
                var add = DNNJuliusFormData.Add(model);
                return BusinessResult<js_DNNJuliusForm>.Sucess(add, string.Empty);
            }
            catch (Exception ex)
            {
                return BusinessResult<js_DNNJuliusForm>.Issue(null, "Error guardando formulario de Julius Form's", ex);
            }
        }

        /// <summary>
        /// Guarda en el formulario la información del archivo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BusinessResult<js_DNNJuliusFormArchivo> AddFile(js_DNNJuliusFormArchivo model)
        {
            try
            {
                DNNJuliusFormData = new DNNJuliusFormData();
                var add = DNNJuliusFormData.AddFile(model);
                return BusinessResult<js_DNNJuliusFormArchivo>.Sucess(add, string.Empty);
            }
            catch (Exception ex)
            {
                return BusinessResult<js_DNNJuliusFormArchivo>.Issue(null, "Error guardando formulario de Julius Form's", ex);
            }
        }

        DepartamentHandler departamentHandler = new DepartamentHandler();
        private string GetDepartamentNameById(int id)
        {
            var dep = departamentHandler.GetDepartamentByIdCountry(1);
            string name = string.Empty;
            if (dep != null)
            {
                var fil = dep.FirstOrDefault(c => c.Id == id);
                name = fil != null ? fil.Name : string.Empty;
            }
            return name;
        }

        CityHandler cityHandler = new CityHandler();
        private string GetCityNameById(int idDep, int idCity)
        {
            var city = cityHandler.GetCityByDepartamentId(idDep);
            string name = string.Empty;
            if (city != null)
            {
                var fil = city.FirstOrDefault(c => c.Id == idCity);
                name = fil != null ? fil.Name : string.Empty;
            }
            return name;
        }

        /// <summary>
        /// Envía correo electrónico a el usuario
        /// </summary>
        /// <param name="PE"></param>
        public BusinessResult<bool> SendEmail(DNNJuliusFormDto PE, List<FileUploadDto> files)
        {
            try
            {
                List<Attachment> attachment = new List<Attachment>();

                string Plantilla = File.ReadAllText($"{HttpRuntime.AppDomainAppPath}{ThemeEmail}");

                Plantilla = Plantilla.Replace("{{niu}}", PE.CodigoNiu);
                Plantilla = Plantilla.Replace("{{factibilidad}}", PE.Factibilidad);
                Plantilla = Plantilla.Replace("{{observaciones}}", PE.Observaciones);

                Plantilla = Plantilla.Replace("{{strNombreUsuario}}", PE.NombreRazonSocial);
                Plantilla = Plantilla.Replace("{{nombresS}}", PE.NombreRazonSocial);
                Plantilla = Plantilla.Replace("{{tipoDocumentoS}}", PE.TipoDocumentoSolicitante);
                Plantilla = Plantilla.Replace("{{documentoIdentidadS}}", PE.DocumentoIdentidadSolicitante);
                Plantilla = Plantilla.Replace("{{direccionS}}", PE.DireccionSolicitante);
                Plantilla = Plantilla.Replace("{{municipioS}}", GetCityNameById(string.IsNullOrEmpty(PE.DepartamentoSolicitante) ? 0 : Convert.ToInt32(PE.DepartamentoSolicitante), string.IsNullOrEmpty(PE.MunicipioSolicitante) ? 0 : Convert.ToInt32(PE.MunicipioSolicitante)));
                Plantilla = Plantilla.Replace("{{DepartamentoS}}", GetDepartamentNameById(string.IsNullOrEmpty(PE.DepartamentoSolicitante) ? 0 : Convert.ToInt32(PE.DepartamentoSolicitante)));
                Plantilla = Plantilla.Replace("{{telefonoS}}", PE.TelefonoSolicitante);
                Plantilla = Plantilla.Replace("{{celularS}}", PE.CelularSolicitante);
                Plantilla = Plantilla.Replace("{{emailS}}", PE.EmailSolicitante);
                Plantilla = Plantilla.Replace("{{nombresP}}", PE.NombrePropietario);
                Plantilla = Plantilla.Replace("{{tipoDocumentoP}}", PE.TipoDocumentoPropietario);
                Plantilla = Plantilla.Replace("{{documentoIdentidadP}}", PE.DocumentoIdentidadPropietario);
                Plantilla = Plantilla.Replace("{{direccionP}}", PE.DireccionPropietario);
                Plantilla = Plantilla.Replace("{{departamentoP}}", GetDepartamentNameById(string.IsNullOrEmpty(PE.DepartamentoPropietario) ? 0 : Convert.ToInt32(PE.DepartamentoPropietario)));
                Plantilla = Plantilla.Replace("{{municipioP}}", GetCityNameById(string.IsNullOrEmpty(PE.DepartamentoPropietario) ? 0 : Convert.ToInt32(PE.DepartamentoPropietario), string.IsNullOrEmpty(PE.MunicipioPropietario) ? 0 : Convert.ToInt32(PE.MunicipioPropietario)));
                Plantilla = Plantilla.Replace("{{telefonoP}}", PE.TelefonoPropietario);
                Plantilla = Plantilla.Replace("{{celularP}}", PE.CelularPropietario);
                Plantilla = Plantilla.Replace("{{emailP}}", PE.EmailPropietario);
                Plantilla = Plantilla.Replace("{{nombreO}}", PE.NombreObra);
                Plantilla = Plantilla.Replace("{{direccionO}}", PE.DireccionObra);
                Plantilla = Plantilla.Replace("{{estratoO}}", PE.EstratoObra.ToString());
                Plantilla = Plantilla.Replace("{{nombresI}}", PE.NombreIngeniero);
                Plantilla = Plantilla.Replace("{{documentoI}}", PE.DocumentoIdentidadIngeniero);
                Plantilla = Plantilla.Replace("{{MatriculaProfesionalI}}", PE.MatriculaProfesionalIngeniero);
                Plantilla = Plantilla.Replace("{{respuesta}}", (PE.Respuesta) ? "Sí" : "No");

                Mail.SendEmail(FromEmail, FromEmail, PE.EmailSolicitante, SubjectEmail, Plantilla, attachment);

                if (files != null && files.Count > 0)
                {
                    foreach (var item in files)
                    {
                        var read = File.ReadAllBytes($"{HttpRuntime.AppDomainAppPath}{item.SavePath.Replace("~", "")}");
                        Stream stream = new MemoryStream(read);
                        attachment.Add(new Attachment(stream, item.fileName));
                        stream.Close();
                    }
                }

                var split = CopyEmail.Split(';');
                if (split != null && split.Count() > 0)
                {
                    foreach (var item in split)
                    {
                        Mail.SendEmail(FromEmail, FromEmail, item.Trim(), SubjectEmail, Plantilla, attachment);
                    }
                }
                return BusinessResult<bool>.Sucess(true, string.Empty);
            }
            catch (Exception ex)
            {
                return BusinessResult<bool>.Issue(false, string.Empty, ex);
            }
        }

        /// <summary>
        /// Cuando ocurre un error en el sistema, este intenta borrar los datos para no consevar datos basura
        /// </summary>
        /// <param name="project"></param>
        public void RollBack(js_DNNJuliusForm project)
        {
            try
            {
                DNNJuliusFormData = new DNNJuliusFormData();
                DNNJuliusFormData.RollBack(project);
            }
            catch (Exception ex)
            {
                BusinessResult<bool>.Issue(false, "Módulo form julius", ex);
            }
        }
    }
}