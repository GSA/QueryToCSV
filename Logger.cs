using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace QueryToCSV
{
    class Logger
    {
        // the file path of log
        private String _filePath = string.Empty;

        public Logger(String filePath)
        {
            _filePath = filePath;
        }

        public void DeleteFile()
        {
            if (File.Exists(this._filePath))
                File.Delete(this._filePath);
        }

        /// <summary>
        /// Log method responsible for logging given text
        /// to a text file
        /// </summary>
        /// <param name="text"></param>
        public void Log(String text)
        {
            // Open the file using stream write.
            // If file does not exist, StreamWriter will create it.
            // Use the overloaded constructor of StreamWriter and 
            // pass second parameter as true for appending text to file.
            using (StreamWriter writer = new StreamWriter(this._filePath, true))
            {
                // write the text to writer
                writer.WriteLine(text);

                // clear all the buffer and 
                // write the buffered data to text file.
                writer.Flush();
            }
        }
    }
}
