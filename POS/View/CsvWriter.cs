using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.View
{
    /// <summary>
    /// Generates a CSV file.
    /// </summary>
    public class CsvWriter
    {
        private string _dir;
        private string _fileName;

        private string[] _headers;

        private CsvWriter _csv;

        /// <summary>
        /// Constructor with parameter.
        /// </summary>
        /// <param name="headers">Header names.</param>
        public CsvWriter(string dir, string fileName, string[] headers)
        {
            this._dir = dir;
            this._fileName = fileName;

            this._headers = headers;
        }

        public void writeRecord(string[] values)
        {

        }

        public void writeRecord(object record)
        {

        }

        /// <summary>
        /// Save the file to disk.
        /// </summary>
        public void Close()
        {

        }
    }
}
