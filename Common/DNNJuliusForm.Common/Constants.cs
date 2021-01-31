namespace DNNJuliusForm.Common
{
    public class Constants
    {
        /// <summary>
        /// Contiene el nombre de la cadena de conexión a usar
        /// </summary>
        public const string MainConnectionString = "SiteSqlServer";

        /// <summary>
        /// Contiene el valor 1, para identificar el país Colombia en la BD
        /// </summary>
        public const int IdCountryColombia = 1;

        //Contiene la url donde se guardan los documentos adjuntos en planos del formulario de proyecto eléctrico
        public const string PathPlans = "~/DNNJuliusFormFiles/DNNJuliusForm/Planos";
        //Contiene la url donde se guardan los documentos adjuntos en memorias del formulario de proyecto eléctrico
        public const string PathMemories = "~/DNNJuliusFormFiles/DNNJuliusForm/Memorias";
        //Contiene la url donde se guardan los documentos adjuntos en licencias del formulario de proyecto eléctrico
        public const string PathLicense = "~/DNNJuliusFormFiles/DNNJuliusForm/Licencia";
        //Contiene la url donde se guardan los documentos adjuntos en 'permiso del formulario de proyecto eléctrico
        public const string PathPermissions = "~/DNNJuliusFormFiles/DNNJuliusForm/Permisos";
    }
}
