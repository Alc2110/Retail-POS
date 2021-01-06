using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace POS.View
{
    /// <summary>
    /// Generates an Excel spreadsheet file.
    /// </summary>
    public class ExcelWriter
    {
        // TODO: cell protection
        // TODO: deal with "number stored as text" errors: https://stackoverflow.com/questions/26483496/is-it-possible-to-ignore-excel-warnings-when-generating-spreadsheets-using-epplu/26484969

        private string _dir;
        private string _filename;
        private string _title;
        private ExcelPackage _pck;
        private ExcelWorksheet _ws;

        private int _headerRow;
        
        public int headerRow
        {
            get { return this._headerRow; }
        }

        private const string META_STYLE = "Metadata";
        private const string HEADER_STYLE = "Header";
        private const string EVEN_DATA_ROW_STYLE = "Even Data Row";
        private const string ODD_DATA_ROW_STYLE = "Odd Data Row";

        private int _currentRow;

        public int currentRow
        {
            get { return this._currentRow; }
        }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="dir">Directory in which to save the file.</param>
        /// <param name="filename">Filename without extension.</param>
        /// <param name="title">Title to display in the spreadsheet.</param>
        /// <param name="worksheetName">Name of the worksheet.</param>
        public ExcelWriter(string dir, string filename, string title, string worksheetName)
        {
            // check for null or empty filename, title or worksheet name

            if (filename == null || filename.Equals(""))
                throw new ArgumentNullException("Cannot have a spreadsheet without a filename.");

            if (worksheetName == null || worksheetName.Equals(""))
                throw new ArgumentNullException("Cannot have a worksheet without a name.");

            if (title==null || title.Equals(""))
                throw new ArgumentNullException("Cannot have an empty title.");

            // create a new package
            this._dir = dir;
            this._filename = filename;
            this._title = title;
            this._pck = new ExcelPackage(new System.IO.FileInfo(_filename + ".xlsx"));

            // create a worksheet
            _ws = _pck.Workbook.Worksheets.Add(worksheetName);

            // by default, the header row is 7
            _headerRow = 7;

            // add named styles
            // metadata
            OfficeOpenXml.Style.XmlAccess.ExcelNamedStyleXml namedStyleMeta = _pck.Workbook.Styles.CreateNamedStyle(META_STYLE);
            namedStyleMeta.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            System.Drawing.Color metaColour = System.Drawing.Color.MediumSeaGreen;
            namedStyleMeta.Style.Fill.BackgroundColor.SetColor(metaColour);
            // header
            OfficeOpenXml.Style.XmlAccess.ExcelNamedStyleXml namedStyleHeader = _pck.Workbook.Styles.CreateNamedStyle(HEADER_STYLE);
            namedStyleHeader.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            System.Drawing.Color headerColour = System.Drawing.Color.MediumSlateBlue;
            namedStyleHeader.Style.Fill.BackgroundColor.SetColor(headerColour);
            // even data row
            OfficeOpenXml.Style.XmlAccess.ExcelNamedStyleXml namedStyleEvenDataRow = _pck.Workbook.Styles.CreateNamedStyle(EVEN_DATA_ROW_STYLE);
            namedStyleEvenDataRow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            System.Drawing.Color evenDataRowColour = System.Drawing.Color.LightSkyBlue;
            namedStyleEvenDataRow.Style.Fill.BackgroundColor.SetColor(evenDataRowColour);
            // odd data row
            OfficeOpenXml.Style.XmlAccess.ExcelNamedStyleXml namedStyleOddDataRow = _pck.Workbook.Styles.CreateNamedStyle(ODD_DATA_ROW_STYLE);
            namedStyleOddDataRow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            System.Drawing.Color oddDataRowColour = System.Drawing.Color.PaleTurquoise;
            namedStyleOddDataRow.Style.Fill.BackgroundColor.SetColor(oddDataRowColour);
        }

        /// <summary>
        /// Write metadata, write headers.
        /// </summary>
        /// <param name="staffID"></param>
        /// <param name="staffFullName"></param>
        /// <param name="headerNames"></param>
        /// <param name="extraMetadataEntries">Optional.</param>
        public void writeHeadersAndMetadata(int staffID, string staffFullName, string[] headerNames, Dictionary<string, string> extraMetadataEntries)
        {
            // write and style the basic metadata to the spreadsheet:
            // - store name
            // - title ("Invoice", "Products", etc.)
            // - timestamp
            // - staff
            _ws.Cells[1, 1].Value = Configuration.storeName;
            _ws.Cells[1, 1].StyleName = META_STYLE;
            _ws.Cells[1, 1].Style.Font.Bold = true;
            _ws.Cells[2, 1].Value = _title;
            _ws.Cells[2, 1].StyleName = META_STYLE;
            _ws.Cells[2, 1].Style.Font.Bold = true;
            _ws.Cells["A1:B1"].Merge = true;

            _ws.Cells[3, 1].Value = "Timestamp: ";
            _ws.Cells[3, 1].StyleName = META_STYLE;
            _ws.Cells[3, 1].Style.Font.Bold = true;
            string timestampValue = DateTime.Now.ToString(); 
            _ws.Cells[3, 2].Value = timestampValue;
            _ws.Cells[3, 2].StyleName = META_STYLE;
            _ws.Cells["A2:B2"].Merge = true;

            _ws.Cells[4, 1].Value = "Staff ID: ";
            _ws.Cells[4, 1].StyleName = META_STYLE;
            _ws.Cells[4, 1].Style.Font.Bold = true;
            _ws.Cells[4, 2].Value = staffID;
            _ws.Cells[4, 2].StyleName = META_STYLE;
            _ws.Cells[5, 1].Value = "Staff Name: ";
            _ws.Cells[5, 1].StyleName = META_STYLE;
            _ws.Cells[5, 1].Style.Font.Bold = true;
            _ws.Cells[5, 2].Value = staffFullName;
            _ws.Cells[5, 2].StyleName = META_STYLE;

            // optionally, write and style extra entries to the metadata
            if (extraMetadataEntries != null)
            {
                int currMetaRow = 5;
                foreach (KeyValuePair<string, string> keyVal in extraMetadataEntries)
                {
                    // for each new metadata row, move the header down by one row
                    _headerRow++;
                    currMetaRow++;

                    // write and style the extra metadata

                    _ws.Cells[currMetaRow, 1].Value = keyVal.Key;
                    _ws.Cells[currMetaRow, 2].Value = keyVal.Value;

                    _ws.Cells[currMetaRow, 1].StyleName = META_STYLE;
                    _ws.Cells[currMetaRow, 1].Style.Font.Bold = true;
                    _ws.Cells[currMetaRow, 2].StyleName = META_STYLE;
                }
            }

            // now write and style the headers
            for (int i = 1; i <= headerNames.Length; i++)
            {
                _ws.Cells[_headerRow, i].Value = headerNames[i - 1];

                _ws.Cells[_headerRow, i].StyleName = HEADER_STYLE;
                _ws.Cells[_headerRow, i].Style.Font.Bold = true;
            }

            _currentRow = headerRow + 1;
        }

        /// <summary>
        /// Write a record to the spreadsheet.
        /// </summary>
        /// <param name="currRow">row number</param>
        /// <param name="record">values</param>
        public void writeRecord(string[] record)
        {
            // write and style a record of data to the spreadsheet
            for (int currentCol = 1; currentCol <= record.Length; currentCol++)
            {
                _ws.Cells[currentRow, currentCol].Value = record[currentCol - 1];

                if (currentRow % 2 == 0)
                {
                    _ws.Cells[currentRow, currentCol].StyleName = EVEN_DATA_ROW_STYLE;
                }
                else
                {
                    _ws.Cells[currentRow, currentCol].StyleName = ODD_DATA_ROW_STYLE;
                }
            }

            _currentRow++;
        }

        /// <summary>
        /// Save the file to disk.
        /// </summary>
        public void close()
        {
            // autofit
            _ws.Cells[_ws.Dimension.Address].AutoFitColumns();

            // create the file, combine directory and filename
            System.IO.FileInfo file = new System.IO.FileInfo(System.IO.Path.Combine(_dir, _filename) + ".xlsx");

            // save and dispose
            _pck.SaveAs(file);
            _pck.Dispose();
        }
    }
}
