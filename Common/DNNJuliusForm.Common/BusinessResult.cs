using DNNJuliusForm.Common.Model;
using DNNJuliusForm.Common.SQL.Implementation;
using System;
using System.Data.SqlClient;

namespace DNNJuliusForm.Common
{
    public class BusinessResult<T>
    {
        public Exception Error
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public bool IsSuccess
        {
            get;
            set;
        }

        public T Result
        {
            get;
            set;
        }

        public BusinessResult()
        {
        }

        public static BusinessResult<T> Sucess(T result, string message)
        {
            return new BusinessResult<T>()
            {
                Message = message,
                IsSuccess = true,
                Result = result
            };
        }

        public static BusinessResult<T> Issue(T result, string message, Exception ex)
        {
            SaveLog($"{message} : {ex.Message} - {ex.InnerException} - {ex.StackTrace}");

            return new BusinessResult<T>()
            {
                Message = message,
                IsSuccess = false,
                Result = result,
                Error = ex
            };
        }

        private static void SaveLog(string message)
        {
            int aux = 0;
            TryAgain:
            try
            {
                if (aux < 2)
                {
                    js_Log log = new js_Log();
                    log.CreateOnDate = DateTime.Now;
                    log.Message = message;
                    log.Module = string.Empty;

                    using (var context = new Model.Model())
                    {
                        context.js_Log.Add(log);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                CreateTableSql.CreateTable(ex);
                aux++;
                goto TryAgain;
            }
        }

    }
}

