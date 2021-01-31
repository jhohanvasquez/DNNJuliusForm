using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using DNNJuliusForm.Common;
using DNNJuliusForm.Common.Handler;
using DNNJuliusModules.DNNJuliusForm.Services.BackEnd;
using DNNJuliusModules.DNNJuliusForm.Services.Dto;
using DNNJuliusModules.DNNJuliusForm.Services.ViewModels;
using System.Linq;
using System.Net;
using System.Net.Http;
using DNNJuliusForm.Common.Utility;
using System.Collections.Generic;
using DNNJuliusForm.Common.Dto;
using DNNJuliusModules.DNNJuliusForm.ServiceReferenceDNNJuliusForm;

namespace DNNJuliusModules.DNNJuliusForm.Services
{
    [SupportedModules("DNNJuliusForm")]
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]

    public class ItemController : DnnApiController
    {
        DNNJuliusFormBusiness DNNJuliusFormBusiness;
        private List<FileUploadDto> fileSuccess;

        /// <summary>
        /// Método que guarda el formulario en la base de datos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HttpResponseMessage Add(DNNJuliusFormDto model)
        {
            DNNJuliusFormBusiness = new DNNJuliusFormBusiness();

            var mapp = AutoMapp<DNNJuliusFormDto, js_DNNJuliusForm>.Convert(model);

            var add = DNNJuliusFormBusiness.Add(mapp);
            if (!add.IsSuccess)
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error procesando la información, por favor inténtelo más tarde.");

            model.Id = add.Result.Id;
            bool saveFileSuccess = SaveFile(model);
            if (!saveFileSuccess)
            {
                DNNJuliusFormBusiness.RollBack(add.Result);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error subiendo los archivos al servidor.");
            }

            //var emailSuccess = DNNJuliusFormBusiness.SendEmail(model, fileSuccess);
            //if (!emailSuccess.IsSuccess)
            //    DNNJuliusFormBusiness.RollBack(add.Result);

            return Request.CreateResponse(add.IsSuccess ? HttpStatusCode.OK : HttpStatusCode.InternalServerError, add);

        }

        /// <summary>
        /// Método que guarda los archivos adjuntos en la base de datos
        /// </summary>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool AddFileDNNJuliusForm(string source, string path, long id)
        {
            js_DNNJuliusFormArchivo file = new js_DNNJuliusFormArchivo();
            file.Fuente = source;
            file.RutaFisica = path;
            file.DNNJuliusFormId = id;
            return DNNJuliusFormBusiness.AddFile(file).IsSuccess;
        }

        /// <summary>
        /// Método que valida el tipo de documento, le hace un filtro de seguridad y lo sube al servidor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveFile(DNNJuliusFormDto model)
        {
            if (model.Files != null && model.Files.Count > 0)
            {
                fileSuccess = new List<FileUploadDto>();

                foreach (var file in model.Files)
                {
                    FileUploadHandler fileUploadHandler = new FileUploadHandler();
                    BusinessResult<string> success = null;
                    switch (file.Source)
                    {
                        case "planos":
                            file.AllowedExtensions = new string[] { ".dwg", ".dwf", ".zip" };
                            file.SavePath = Constants.PathPlans;
                            success = fileUploadHandler.Upload(file);
                            file.SavePath = success.Result;
                            fileSuccess.Add(file);
                            if (!AddFileDNNJuliusForm(file.Source, success.Result, model.Id))
                            {
                                return false;
                            }
                            break;
                        case "memorias":
                            file.AllowedExtensions = new string[] { ".xls", ".xlsx", ".docx", ".pdf", ".zip" };
                            file.SavePath = Constants.PathMemories;
                            success = fileUploadHandler.Upload(file);
                            file.SavePath = success.Result;
                            fileSuccess.Add(file);
                            if (!AddFileDNNJuliusForm(file.Source, success.Result, model.Id))
                            {
                                return false;
                            }
                            break;
                        case "licencia":
                            file.AllowedExtensions = new string[] { ".pdf" };
                            file.SavePath = Constants.PathLicense;
                            success = fileUploadHandler.Upload(file);
                            file.SavePath = success.Result;
                            fileSuccess.Add(file);
                            if (!AddFileDNNJuliusForm(file.Source, success.Result, model.Id))
                            {
                                return false;
                            }
                            break;
                        case "permiso":
                            file.AllowedExtensions = new string[] { ".pdf" };
                            file.SavePath = Constants.PathPermissions;
                            success = fileUploadHandler.Upload(file);
                            file.SavePath = success.Result;
                            fileSuccess.Add(file);
                            if (!AddFileDNNJuliusForm(file.Source, success.Result, model.Id))
                            {
                                return false;
                            }
                            break;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Método para la carga inicial del módulo
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            DepartamentHandler departamentHandler = new DepartamentHandler();

            var dep = departamentHandler.GetDepartamentByIdCountry(Constants.IdCountryColombia);

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                Departament = dep != null ? dep : null
            });
        }

        /// <summary>
        /// Obtiene las ciudades por departamento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage GetCityByDepartament(int id)
        {
            CityHandler cityHandler = new CityHandler();
            return Request.CreateResponse(HttpStatusCode.OK, cityHandler.GetCityByDepartamentId(id));
        }
    }
}
