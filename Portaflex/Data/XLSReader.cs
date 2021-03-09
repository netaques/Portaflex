using System;
using System.Collections.Generic;
using Portaflex.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace Portaflex
{
    public static class XLSWorker
    {
        private const int ID_OFFSET = 0;
        private const int NAME_OFFSET = 3;
        private const int DATA_OFFSET = 13;
        private const int SUM_OFFSET = 2;

        /*
        private Total t;
        private List<DataGridView> data;
        private string path;
        */
        public static void writeData(Total t, List<DataGridView> data, string path)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            if (xlWorkBook.Worksheets.Count < t.Departments.Count + 1)
            {
                var count = t.Departments.Count + 1 - xlWorkBook.Worksheets.Count;
                for (var i = 0; i < count; i++)
                    xlWorkBook.Worksheets.Add();
            }
            for (var page = 0; page < data.Count; page++)
            {
                string title;
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(page + 1);
                if(page == 0)
                    title = Texts.TotalTitle;
                else
                    title = Texts.Department + " " + t.Departments[page - 1].Name;
                title = title.Substring(0, Math.Min(30, title.Length));
                xlWorkSheet.Name = title;
                xlWorkSheet.Cells[1, 1] = title;
                xlWorkSheet.get_Range("a1", "a1").Font.Size = 14;
                xlWorkSheet.get_Range("a1", "a1").Font.Bold = true;

                var c = 'a';
                xlWorkSheet.get_Range("a2").EntireRow.Font.Bold = true;
                for (var i = 0; i < data[page].ColumnCount; i++)
                {
                    xlWorkSheet.Cells[2, i + 1] = data[page].Columns[i].HeaderText;
                }
                for (var i = 0; i < data[page].RowCount; i++)
                {
                    if (i < t.Budgets.Count && t.Budgets[i].Sum)
                    {
                        var s = "a" + (int)(i + 3);
                        xlWorkSheet.get_Range(s).EntireRow.Font.Bold = true;
                    }
                    for (var j = 0; j < data[page].ColumnCount; j++)
                        xlWorkSheet.Cells[i + 3, j + 1] = data[page].Rows[i].Cells[j].Value;
                }
                c = 'a';
                for (var i = 0; i < data[page].ColumnCount; i++)
                {
                    xlWorkSheet.get_Range(c + "2").EntireColumn.AutoFit();
                    c++;
                }

                xlWorkSheet.get_Range("a" + (int)(2 + data[page].RowCount)).EntireRow.Font.Bold = true;
            }
            xlWorkBook.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
        }

        public static List<Budget> readBudgets(Total t, string path)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            var list = new List<Budget>();
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            var range = xlWorkSheet.UsedRange;

            var j = -1;
            var income = false;
            for (var i = 1; i <= range.Rows.Count; i++)                
            {
                if (j == -1)
                {
                    for (var col = 1; col < range.Columns.Count; col++)
                    {
                        var str = (string)(range.Cells[i, col] as Excel.Range).Value2;
                        if (str != null && str.Contains("Výsledovka"))
                        {
                            j = col;
                            break;
                        }
                    }
                }
                else
                {
                    var id = getCellContent(i,j + ID_OFFSET,range);                    
                    var name = getCellContent(i,j+NAME_OFFSET, range);
                    if (id != null)
                    {
                        var sum = id.Contains("x");
                        if (id.Contains("Výnosy"))
                            income = true;
                        if (id.Contains("Náklady"))
                            income = false;
                        if (sum)
                            name = getCellContent(i,j + SUM_OFFSET, range);
                        if (name != null && !t.ContainsBudget(id, name) && id.Length > 0 && Char.IsNumber(id[0]))
                            list.Add(new Budget(name, id, sum, income));
                    }
                }
            }

            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            return list;
        }

        private static SubDepartment initDep(int count)
        {
            var dep = new SubDepartment();
            for (var i = 0; i < count; i++)
                dep.Values.Add(0);
            return dep;
        }

        public static SubDepartment readData(List<Budget> budgets, string path)
        {
            var dep = initDep(budgets.Count);
            
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            var range = xlWorkSheet.UsedRange;

            var j = -1;
            for (var i = 1; i <= range.Rows.Count; i++)
            {
                if (j == -1)
                {
                    for (var col = 1; col < range.Columns.Count; col++)
                    {
                        var str = (string)(range.Cells[i, col] as Excel.Range).Value2;
                        if (str != null && str.Contains("Výsledovka"))
                        {
                            j = col;
                            break;
                        }
                    }
                }
                else
                {
                    var id = getCellContent(i, j + ID_OFFSET, range);
                    var name = getCellContent(i, j + NAME_OFFSET, range);
                    var data = getCellContent(i, j + DATA_OFFSET, range);
                    if (id != null && name != null && data != null)
                    {
                        var index = findPosition(id, name, budgets);
                        if (index != -1)
                            dep.Values[index] = Convert.ToDouble(data);
                    }
                }
            }

            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
            return dep;
        }

        private static int findPosition(string id, string name, List<Budget> list)
        {
            foreach (var b in list)
            {
                if (b.ID == id && b.Name == name)
                    return list.IndexOf(b);
            }
            return -1;
        }

        private static string getCellContent(int row, int col, Excel.Range range)
        {
            string id = null;
            object _id = (range.Cells[row, col] as Excel.Range).Value2;
            if (_id != null)
            {
                if (_id is double)
                    id = _id.ToString();
                else if (_id is string)
                    id = (string)_id;
            }
            return id;
        }


        public static Total reconcile(string path, Total t)
        {
            return new Total();
        }

        public static Total readFile(string path)
        {
            return new Total();
        }

        static private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
