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
        //Start logger for class
        static Logger logger = new Logger(AppDomain.CurrentDomain.BaseDirectory + "QueryToCSV.log");

        // Stores the database connection in an external config file
        //Command line arguments are not used for the connection
        MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["CONNSTRING"].ToString());
        MySqlCommand cmd;

        /// <summary>
        /// Helper function that uses the connection string from .config file and executes the query argument passed in
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private DataSet GetData(string query)
        {
            try
            {
                //Open connection if not open
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                //use query and connection already established
                cmd = new MySqlCommand(query, conn);
                //cmd.CommandType = CommandType.StoredProcedure;                

                //Create a dataset to store the results
                DataSet ds = new DataSet();

                //Create sql adapter to communicate with database
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                //fill the dataset using the adapter
                da.Fill(ds);

                //return the filled dataset
                return ds;
            }
            //catch all errors
            catch (Exception e)
            {
                //write all errors to log file
                logger.Log(e.Message.ToString());
                //throw the error after logging
                throw;
            }
        }

        /// <summary>
        /// Retrieves a dataset from a query and writes it to a csv file
        /// </summary>
        /// <param name="query"></param>
        /// <param name="fileLocation"></param>
        /// <param name="fileName"></param>
        /// <param name="delimiter"></param>
        /// <param name="zipAndEncrypt"></param>
        /// <param name="archiveName"></param>
        public void ConvertToCSV(string query, string fileLocation, string fileName, string delimiter = ",", bool zipAndEncrypt = false, string archiveName = "")
        {
            //Create a dataset to store the query results and call GetData to fill the dataset
            DataSet queryData = GetData(query);

            //Use stringbuilder to build the csv file
            StringBuilder csvExport = new StringBuilder();

            //Grabs all the column names from the dataset and stores them in a local variable
            var columnNames = queryData.Tables[0].Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();

            //Append the column names to the csv file eperated by the delimiter
            csvExport.AppendLine(string.Join(delimiter, columnNames));            

            //Append rows of data to the csv file seperated by the delimiter until all rows are addded
            foreach (DataRow row in queryData.Tables[0].Rows)
            {
                var fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                //csvExport.AppendLine(string.Join(",", fields));
                csvExport.AppendLine(string.Join(delimiter, fields.Select(Csv.Escape)));
            }

            //set path to file location + filename
            string path = fileLocation + @"\" + fileName;

            //save csv to location indicated by path
            File.WriteAllText(@path, csvExport.ToString());

            //Zip and encrypt if bool set to true
            if (zipAndEncrypt)
            {
                ZipAndEncrypt(@path, archiveName);

                //delete original file if still exists
                if (File.Exists(@path))
                    File.Delete(@path);
            }
        }

        /// <summary>
        /// Helper function that Encrypts and zips a file
        /// Sets the password and location of new file based on config settings
        /// </summary>
        /// <param name="fileToAdd"></param>
        /// <param name="archiveName"></param>
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
