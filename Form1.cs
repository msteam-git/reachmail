using System;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Net;

namespace KPISearch
{
    public partial class Form1 : Form
    {
        private static string folderPath = "C:\\temp\\";
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                bool exists = System.IO.Directory.Exists(folderPath);
                if (!exists)
                    System.IO.Directory.CreateDirectory(folderPath);
                OpenFileDialog op1 = new OpenFileDialog();
                op1.Multiselect = true;
                op1.ShowDialog();
                op1.Filter = "allfiles|*.xls";
                string filePath = "";
                if (op1.FileNames.Length == 1)
                {
                    UploadLists uploadLists = new UploadLists();
                    foreach (string fileNames in op1.FileNames)
                    {
                        string fileExtension = Path.GetExtension(fileNames);
                        filePath = fileNames;
                        if (fileExtension == ".csv" || fileExtension == ".xls" || fileExtension == ".xlsx")
                        {
                            SplashScreen.SplashScreen.ShowSplashScreen();
                            uploadLists.UploadKPSARecords(filePath);
                            SplashScreen.SplashScreen.CloseForm();
                            MessageBox.Show("Uploaded successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Upload .csv, .xls or .xlsx file only", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else if (op1.FileNames.Length > 1)
                {
                    MessageBox.Show("Select one file only", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                SplashScreen.SplashScreen.CloseForm();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
