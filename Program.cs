using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;
using Ionic.Zip;

namespace QueryToCSV
{
    class Program
    {
        static Logger logger = new Logger(AppDomain.CurrentDomain.BaseDirectory + "QueryToCSV.log");

        /// <summary>
        /// Query
        /// File Location
        /// File Name
        /// Delimiter
        /// Zip Arhcive (bool)
        /// Archive Name
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            logger.Log("Application Started: " + DateTime.Now);

            logger.Log("Args: " + args.Length);

            if (args.Length == 0)
            {
                Console.WriteLine("No Arguements submitted");
                Console.ReadLine();
                return;
            }

            logger.Log("Setting Culture Info");

            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "MM-dd-yyyy";
            Thread.CurrentThread.CurrentCulture = culture;
            
            logger.Log("Initializing Process Data");

            ProcessData processData = new ProcessData();

            logger.Log("Done Initializing");

            bool invalidCommand = new[] { "select", "delete", "insert", "update", "alter" }.Any(c => args[0].Contains(c.ToLower()));

            if (invalidCommand)
            {
                logger.Log("Invalid Command Passed: " + args[0].ToString());
            }
            else
            {
                processData.ConvertToCSV(args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), Convert.ToBoolean(args[4].ToString()), args[5].ToString());
            }

            logger.Log("Application Done: " + DateTime.Now);
        }
    }
}
