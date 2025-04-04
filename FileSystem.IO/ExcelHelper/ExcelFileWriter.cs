using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem.IO
{
    public class ExcelFileWriter : IDisposable
    {
        Application application = null;

        public Workbook Workbook { get; private set; }

        public ExcelFileWriter(string path)
        {
            application = new Application();

            Workbook = application.Workbooks.Open(path);
        }

        public void WriteResultToFile(string filePath, double[,] values, int zoneCount, string sheetName)
        {
            string foundSheetName = FindNextSheetName(sheetName);
            Worksheet newWorksheet = (Worksheet)Workbook.Worksheets.Add();
            newWorksheet.Name = foundSheetName;

            _Worksheet sheet = newWorksheet;

            Range refCell = sheet.Range["A1"];

            Range firstCell = sheet.Range["A1"];
            Range lastRow = firstCell.GetVerticalLastCellRange(zoneCount - 1);
            Range lastCell = lastRow.GetHorizontalLastCellRange(zoneCount - 1);

            Range allArea = sheet.Range[firstCell.Address, lastCell.Address];
            allArea.Value2 = values;

            Workbook.Save();
        }

        private string FindNextSheetName(string sheetName)
        {
            int lastCodeIndex = 0;

            var sheetCount = Workbook.Sheets.Count;
            foreach (_Worksheet sheet in Workbook.Sheets)
            {
                if (sheet.Name.StartsWith(sheetName) && sheet.Name.Contains("_"))
                {
                    int currentCodeIndex = sheet.Name.Split('_')[1].ToInt();
                    if (lastCodeIndex < currentCodeIndex)
                    {
                        lastCodeIndex = currentCodeIndex;
                    }
                }
            }

            return sheetName + "_" + (lastCodeIndex + 1).ToString();
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
