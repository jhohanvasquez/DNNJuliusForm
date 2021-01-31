using DNNJuliusForm.Common.Dto;
using System;
using System.IO;
using System.Linq;

namespace DNNJuliusForm.Common.Handler
{
    public class FileUploadHandler
    {
        public BusinessResult<string> Upload(FileUploadDto file)
        {
            try
            {
                string ext = Path.GetExtension(file.fileName);
                var split = file.Base.Split(',');
                string fullPath = string.Empty;
                if (file.AllowedExtensions.FirstOrDefault(c => c.ToLower() == ext.ToLower()) != null)
                {
                    string fileName = Guid.NewGuid().ToString().Replace("-", "");
                    DirectoryInfo di = Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(file.SavePath));

                    var bytes = Convert.FromBase64String(string.Empty);
                    if (split.Length > 1)
                    {
                        bytes = Convert.FromBase64String(split[1]);
                    }
                    fullPath = $"{di.FullName}/{fileName}{ext}";
                    using (var imageFile = new FileStream(fullPath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    return BusinessResult<string>.Sucess($"{file.SavePath}/{fileName}{ext}", string.Empty);
                }
                return BusinessResult<string>.Issue(string.Empty, $"Archivo no permitido", null);
            }
            catch (Exception ex)
            {
                return BusinessResult<string>.Issue(string.Empty, $"Error subiendo el archivo {file.fileName} al servidor", ex);
            }
        }
    }
}
