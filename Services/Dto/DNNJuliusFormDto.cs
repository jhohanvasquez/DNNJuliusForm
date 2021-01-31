using DNNJuliusForm.Common.Dto;
using System;
using System.Collections.Generic;

namespace DNNJuliusModules.DNNJuliusForm.Services.Dto
{
    public class DNNJuliusFormDto
    {
        public long Id { get; set; }
        public string CodigoNiu { get; set; }
        public string Factibilidad { get; set; }
        public string NombreRazonSocial { get; set; }
        public string TipoDocumentoSolicitante { get; set; }
        public string DocumentoIdentidadSolicitante { get; set; }
        public string DireccionSolicitante { get; set; }
        public string DepartamentoSolicitante { get; set; }
        public string MunicipioSolicitante { get; set; }
        public string TelefonoSolicitante { get; set; }
        public string CelularSolicitante { get; set; }
        public string EmailSolicitante { get; set; }
        public string NombrePropietario { get; set; }
        public string TipoDocumentoPropietario { get; set; }
        public string DocumentoIdentidadPropietario { get; set; }
        public string DireccionPropietario { get; set; }
        public string DepartamentoPropietario { get; set; }
        public string MunicipioPropietario { get; set; }
        public string TelefonoPropietario { get; set; }
        public string CelularPropietario { get; set; }
        public string EmailPropietario { get; set; }
        public string NombreObra { get; set; }
        public string DireccionObra { get; set; }
        public int EstratoObra { get; set; }
        public string NombreIngeniero { get; set; }
        public string DocumentoIdentidadIngeniero { get; set; }
        public string MatriculaProfesionalIngeniero { get; set; }
        public bool Respuesta { get; set; }
        public string Planos { get; set; }
        public string Memorias { get; set; }
        public string Licencia { get; set; }
        public string Permiso { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Observaciones { get; set; }
        public List<FileUploadDto> Files { get; set; }
    }
}