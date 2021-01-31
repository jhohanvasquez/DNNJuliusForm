using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNNJuliusForm.Common.SQL.Implementation
{
    public class CreateTableSql
    {
        public static void CreateTable(Exception ex)
        {
            try
            {
                if (ex.InnerException.InnerException.Message.Contains("Invalid object name"))
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = "DNNJuliusForm.Common.SQL.Script.js_Log.sql";

                    string resource = null;
                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            resource = reader.ReadToEnd();
                        }
                    }

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[Common.Constants.MainConnectionString].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand command = new SqlCommand(resource, con))
                            command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
