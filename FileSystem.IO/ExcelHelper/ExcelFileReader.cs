using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace FileSystem.IO
{
    public class ExcelFileReader : IDisposable
    {
        Application application = null;

        public Workbook Workbook { get; private set; }

        public ExcelFileReader(string path)
        {
            application = new Application();
            
            Workbook = application.Workbooks.Open(path);
        }

        public _Worksheet[] GetSheets() 
        {
            _Worksheet[] worksheets = new _Worksheet[Workbook.Sheets.Count];
            for (int i = 0; i < Workbook.Sheets.Count; i++)
            {
                worksheets[i] = Workbook.Sheets[i + 1];
            }

            return worksheets;
        }

        public object[,] ReadSheet(string sheetName, int zoneCount, bool hasHeader, bool hasIndexer)
        {
            int rowCount = hasHeader ? zoneCount + 1 : zoneCount;
            int columnCount = hasIndexer ? zoneCount + 1 : zoneCount;

            _Worksheet sheet = Workbook.Sheets[sheetName];
            object[,] values = new object[rowCount, columnCount];

            Range firstCell = sheet.Range["A1"];
            Range lastCell = firstCell.Offset[rowCount - 1, columnCount - 1];
            Range allDataRange = sheet.Range[firstCell.Address, lastCell.Address];
            object[,] allRawValues = (object[,])allDataRange.Value2;

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    object cellRangeValue = allRawValues[1 + i, 1 + j];
                    values[i, j] = cellRangeValue;
                }
            }

            return values;
        }


        public int FindRowCount(string sheetName, string firstCellRef)
        {
            _Worksheet sheet = Workbook.Sheets[sheetName];
            Range firstCell = sheet.Range[firstCellRef];
            return firstCell.CurrentRegion.Rows.Count;
        }

        public string[,] ReadRange(string sheetName, string firstCellRef, int rowSize, int columnSize = 0)
        {
            int columnCount = columnSize;
            int rowCount = rowSize;

            _Worksheet sheet = Workbook.Sheets[sheetName];
            Range firstCell = sheet.Range[firstCellRef];

            string[,] values = new string[rowSize, columnSize];

            Range lastCell = firstCell.Offset[rowCount, columnCount];
            Range allDataRange = sheet.Range[firstCell.Address, lastCell.Address];
            object[,] allRawValues = (object[,])allDataRange.Value2;

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    object cellRangeValue = allRawValues[i + 1, 1];
                    string cellValue = cellRangeValue.ToString();
                    values[i, j] = cellValue;
                }
            }

            return values;
        }

        public string[] ReadRange(string sheetName, string firstCellRef, int rowSize)
        {
            int rowCount = rowSize;

            _Worksheet sheet = Workbook.Sheets[sheetName];
            Range firstCell = sheet.Range[firstCellRef];

            string[] values = new string[rowSize];

            Range lastCell = firstCell.Offset[rowCount, 1];
            Range allDataRange = sheet.Range[firstCell.Address, lastCell.Address];
            object[,] allRawValues = (object[,])allDataRange.Value2;

            for (int i = 0; i < rowCount; i++)
            {
                object cellRangeValue = allRawValues[i + 1, 1];
                string cellValue = cellRangeValue.ToString();
                values[i] = cellValue;
            }

            return values;
        }

        public double[,] GetCoefficientValues(int zoneCount)
        {
            int rowCount = zoneCount;
            int columnCount = zoneCount;

            _Worksheet sheet = Workbook.Sheets["Coefficient"];
            double[,] values = new double[rowCount, columnCount];

            Range firstCell = sheet.Range["A1"];
            Range lastCell = firstCell.Offset[rowCount, columnCount];
            Range allDataRange = sheet.Range[firstCell.Address, lastCell.Address];
            object[,] allRawValues = (object[,])allDataRange.Value2;

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    //Range cellRange = (Range)sheet.Cells[2 + i, 2 + j];
                    object cellRangeValue = allRawValues[2 + i, 2 + j];
                    string cellValue = cellRangeValue.ToString();
                    double value = 0.0;
                    bool parseResult = double.TryParse(cellValue, out value);
                    if (!parseResult)
                    {
                        throw new Exception(string.Format("Error on reading value - Address {0}", (2 + i).ToString() + ":" + (2 + j).ToString()));
                    }

                    values[i, j] = value;
                }
            }


            return values;
        }

        public double[] GetAttractionValues(int zoneCount)
        {
            int rowCount = zoneCount;
            int columnCount = 0;

            _Worksheet sheet = Workbook.Sheets["ProductAttraction"];
            double[] values = new double[rowCount];

            Range firstCell = sheet.Range["C2"];
            Range lastCell = firstCell.Offset[rowCount, columnCount];
            Range allDataRange = sheet.Range[firstCell.Address, lastCell.Address];
            object[,] allRawValues = (object[,])allDataRange.Value2;

            for (int i = 0; i < rowCount; i++)
            {
                object cellRangeValue = allRawValues[i + 1, 1];
                string cellValue = cellRangeValue.ToString();
                double value = 0.0;
                bool parseResult = double.TryParse(cellValue, out value);
                if (!parseResult)
                {
                    throw new Exception(string.Format("Error on reading value - Address {0}", (2 + i).ToString()));
                }

                values[i] = value;
            }


            return values;
        }

        public double[] GetResultArrayFor(int zoneId, int zoneCount)
        {
            _Worksheet sheet = Workbook.Sheets["ZoneArrivals"];
            double[] values = new double[zoneCount];

            Range allDataRange = sheet.Range["G3", "G204"];
            object[,] allRawValues = (object[,])allDataRange.Value2;

            for (int i = 0; i < zoneCount; i++)
            {
                object cellRangeValue = allRawValues[i + 1, 1];
                string cellValue = cellRangeValue.ToString();
                double value = 0.0;
                bool parseResult = double.TryParse(cellValue, out value);
                if (!parseResult)
                {
                    throw new Exception(string.Format("Error on reading value - Address {0}", (i).ToString()));
                }

                values[i] = value;
            }


            return values;
        }

        /// <summary>
        /// Get a values
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="zoneId"></param>
        /// <param name="zoneCount"></param>
        /// <returns></returns>
        public double[] GetMaxUtilityArray(int zoneCount)
        {
            _Worksheet sheet = Workbook.Sheets["ZoneArrivals"];
            double[] values = new double[zoneCount];

            Range firstCell = sheet.Range["C3"];
            Range lastCell = firstCell.GetVerticalLastCellRange(zoneCount);

            Range allDataRange = sheet.Range[firstCell.Address, lastCell.Address];
            object[,] allRawValues = (object[,])allDataRange.Value2;

            for (int i = 0; i < zoneCount; i++)
            {
                object cellRangeValue = allRawValues[i + 1, 1];
                string cellValue = cellRangeValue.ToString();
                double value = 0.0;
                bool parseResult = double.TryParse(cellValue, out value);
                if (!parseResult)
                {
                    throw new Exception(string.Format("Error on reading value - Address {0}", (i).ToString()));
                }

                values[i] = value;
            }


            return values;
        }

        public double[,] GetCostValues(int zoneCount)
        {
            int rowCount = zoneCount;
            int columnCount = zoneCount;

            _Worksheet sheet = Workbook.Sheets["Cost"];
            double[,] values = new double[rowCount, columnCount];

            Range firstCell = sheet.Range["A1"];
            Range lastColumn = firstCell.GetHorizontalLastCellRange(columnCount);
            Range lastCell = lastColumn.GetVerticalLastCellRange(rowCount);
            Range allDataRange = sheet.Range[firstCell.Address, lastCell.Address];

            object[,] allRawValues = (object[,])allDataRange.Value2;

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    //Range cellRange = (Range)sheet.Cells[2 + i, 2 + j];
                    object cellRangeValue = allRawValues[2 + i, 2 + j];
                    string cellValue = cellRangeValue.ToString();
                    double value = 0.0;
                    bool parseResult = double.TryParse(cellValue, out value);
                    if (!parseResult)
                    {
                        throw new Exception(string.Format("Error on reading value - Address {0}", (2 + i).ToString() + ":" + (2 + j).ToString()));
                    }

                    values[i, j] = value;
                }
            }


            return values;
        }


        public void Dispose()
        {
            Workbook.Close(true);
            application.Workbooks.Close();
            application.Quit();

            Workbook = null;
            application = null;

            GC.Collect();
        }
    }
}
