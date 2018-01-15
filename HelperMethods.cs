using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPISearch
{
    public static class HelperMethods
    {
        public static DataTable ConvertExcelToDataTable(string filePath, bool isXlsx = false)
        {
            FileStream stream = null;
            IExcelDataReader excelReader = null;
            DataTable dataTable = null;
            stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            excelReader = isXlsx ? ExcelReaderFactory.CreateOpenXmlReader(stream) : ExcelReaderFactory.CreateBinaryReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();
            if (result != null && result.Tables.Count > 0)
                dataTable = result.Tables[0];
            return dataTable;
        }
        public static void SavebBulkLogFile(string path, string Messages)
        {
            try
            {
                using (TextWriter writer = File.AppendText(path))
                {
                    StringBuilder builder = new StringBuilder();
                    writer.WriteLine(Messages);
                    writer.Dispose();
                }
            }
            catch (IOException)
            {
                SavebBulkLogFile(path, Messages);
            }
            
        }
    }
}
