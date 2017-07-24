using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;
using Ionic.Zip;

namespace QueryToCSV
{
    /// <summary>
    /// Command line program that calls and stored procedure with the below arguments and saves the result as a csv.
    /// </summary>
    class Program
    {
        //Begin logger
        static Logger logger = new Logger(AppDomain.CurrentDomain.BaseDirectory + "QueryToCSV.log");

        /// <summary>
        /// Query
        /// File Location
        /// File Name
        /// Delimiter
        /// Zip Archive (bool)
        /// Archive Name
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Application starts
            logger.Log("Application Started: " + DateTime.Now);

            logger.Log("Args: " + args.Length);

            //Args should contain the above list of arguments
            //No need to continue if arguments not provided
            if (args.Length == 0)
            {
                Console.WriteLine("No Arguments submitted");
                Console.ReadLine();
                return;
            }

            //Set culture info
            logger.Log("Setting Culture Info");
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "MM-dd-yyyy";
            Thread.CurrentThread.CurrentCulture = culture;
            
            //Initialize Process data class
            logger.Log("Initializing Process Data");
            ProcessData processData = new ProcessData();
            logger.Log("Done Initializing");

            //Prevents certain commands from being executed
            bool invalidCommand = new[] { "select", "delete", "insert", "update", "alter" }.Any(c => args[0].Contains(c.ToLower()));

            //If invalid command
            //Log command attempted and end program
            if (invalidCommand)
            {
                logger.Log("Invalid Command Passed: " + args[0].ToString());
            }
            //Else convert query to csv
            else
            {
                //OK to continue with query conversion
                processData.ConvertToCSV(args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), Convert.ToBoolean(args[4].ToString()), args[5].ToString());
            }

            //Log end of application
            logger.Log("Application Done: " + DateTime.Now);
        }
    }
}
