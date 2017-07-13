using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Ionic.Zip;

namespace QueryToCSV
{
    class ProcessData
    {
        static Logger logger = new Logger(AppDomain.CurrentDomain.BaseDirectory + "QueryToCSV.log");

        // create connectionstring property and override the username and password
        MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["CONNSTRING"].ToString());
        MySqlCommand cmd;

        private DataSet GetData(string query)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                cmd = new MySqlCommand(query, conn);
                //cmd.CommandType = CommandType.StoredProcedure;                

                DataSet ds = new DataSet();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                da.Fill(ds);

                return ds;
            }
            catch (Exception e)
            {
                logger.Log(e.Message.ToString());
                throw;
            }
        }

        public void ConvertToCSV(string query, string fileLocation, string fileName, string delimiter = ",", bool zipAndEncrypt = false, string archiveName = "")
        {
            DataSet queryData = GetData(query);

            StringBuilder csvExport = new StringBuilder();

            var columnNames = queryData.Tables[0].Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();

            csvExport.AppendLine(string.Join(delimiter, columnNames));            

            foreach (DataRow row in queryData.Tables[0].Rows)
            {
                var fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                //csvExport.AppendLine(string.Join(",", fields));
                csvExport.AppendLine(string.Join(delimiter, fields.Select(Csv.Escape)));
            }

            string path = fileLocation + @"\" + fileName;

            File.WriteAllText(@path, csvExport.ToString());

            if (zipAndEncrypt)
            {
                ZipAndEncrypt(@path, archiveName);

                if (File.Exists(@path))
                    File.Delete(@path);
            }
        }

        private void ZipAndEncrypt(string fileToAdd, string archiveName)
        {
            using(ZipFile zip = new ZipFile())
            {
                zip.Password = ConfigurationManager.AppSettings["zipKey"];
                zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                //zip.AddFile(@filePath, "");
                zip.AddFile(fileToAdd, @"\");
                zip.Save(@ConfigurationManager.AppSettings["zipLocation"] + archiveName);
            }
        }
    }
}
