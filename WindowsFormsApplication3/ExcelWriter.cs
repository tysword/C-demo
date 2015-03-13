using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace DataSet2ExcelDemo
{   
    class ExcelWriter
    {
        public static void CreateExcel(DataTable dt, string fileName)
        {
            System.Diagnostics.Process[] arrProcesses; arrProcesses = System.Diagnostics.Process.GetProcessesByName("Excel");
            foreach (System.Diagnostics.Process myProcess in arrProcesses)
            {
                myProcess.Kill();
            }

            Object missing = Missing.Value;
            Microsoft.Office.Interop.Excel.Application m_objExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks m_objWorkBooks = m_objExcel.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook m_objWorkBook = m_objWorkBooks.Add(true);
            Microsoft.Office.Interop.Excel.Sheets m_objWorkSheets = m_objWorkBook.Sheets; ;
            Microsoft.Office.Interop.Excel.Worksheet m_objWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)m_objWorkSheets[1];
            int intFeildCount = dt.Columns.Count;
            for (int col = 0; col < intFeildCount; col++)
            {
                m_objWorkSheet.Cells[1, col + 1] = dt.Columns[col].ToString();
            }
            for (int intRowCount = 0; intRowCount < dt.Rows.Count; intRowCount++)
            {
                for (int intCol = 0; intCol < dt.Columns.Count; intCol++)
                {
                    m_objWorkSheet.Cells[intRowCount + 2, intCol + 1] = "'" + dt.Rows[intRowCount][intCol].ToString();
                }
            }
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            m_objWorkBook.SaveAs(fileName, missing, missing, missing, missing, missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing);
            m_objExcel = null;
        }
    }
}
