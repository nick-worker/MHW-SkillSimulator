using System;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHWSSv1
{
    class SQLUtill
    {
        public static DataTable select(string sqlSentence)
        {
            SQLiteDataReader returnObect = null;
            var sqlConnectionSb = new SQLiteConnectionStringBuilder { DataSource = "mhwss.db" };

            using (var cn = new SQLiteConnection(sqlConnectionSb.ToString()))
            {
                cn.Open();

                using (var cmd = new SQLiteCommand(cn))
                {
                    cmd.CommandText = sqlSentence;
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                    DataTable data = new DataTable();
                    adapter.Fill(data);

                    //returnObect = cmd.ExecuteReader();

                    cn.Close();
                    //return cmd.ExecuteReader();
                    return data;
                }


            }
        }
    }
}
