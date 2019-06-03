using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace SQSSerialStepResults
{
    public class SqliteDataAccess
    {
        private string output;

        public static List<ResultModel> LoadResults()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<ResultModel>("select * from Results order by Id desc", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveResult(ResultModel result)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Results (ResultData) values (@ResultData)", result);
            }

        }
        //public string timeDelta = "2";
        public string DeleteResult(string timeDelta)
        {
            if (output != null)
            {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {

                    cnn.Execute("DELETE FROM Results WHERE TimeStamp <= datetime('now', '-" + timeDelta + " days')");
                }
            }
            
            return timeDelta;

        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
