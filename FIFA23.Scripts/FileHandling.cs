using FifaLibrary;
using System.Data;
using System.IO;
using ClosedXML;
using System.Windows.Controls;
using Microsoft;
using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;

namespace FIFA23.Scripts
{
    public class FileHandling
    {
        string m_FifaDbFileName;
        string m_FifaDbXmlFileName;
        string m_InternalFile;
        DbFile m_FifaDb;
        DataSet m_DataSet;
        CareerFile m_CareerFile;

        public DataTable m_PlayerNames;
        public DataSet[] m_DataSetEa;
        public FileType Type { get; set; }
        
        
        public FileHandling(FileType fileType)

        {
            this.Type = fileType;
            this.m_FifaDbFileName = Path.Combine(Environment.CurrentDirectory, @"Data\", "fifa_ng_db.db");
            this.m_FifaDbXmlFileName = Path.Combine(Environment.CurrentDirectory, @"Data\", "fifa_ng_db-meta.xml");


        }

        
        public int Load(string InternalFile)
        {
            int ret = 0;
            this.m_InternalFile = InternalFile;     
            LoadEA();
            return ret;
        }

        public void Save()
        {
            SaveEA();
        }
        public void LoadDb()
        {

            m_FifaDb = new DbFile(this.m_FifaDbFileName, this.m_FifaDbXmlFileName);
            this.m_DataSet = this.m_FifaDb.ConvertToDataSet();
            this.m_PlayerNames = this.m_DataSet.Tables["playernames"];
            //var wb = new XLWorkbook();
            //wb.Worksheets.Add(this.m_DataSet);
            //wb.SaveAs("Sqaud.xlsx");
            


        }
        
        private void LoadEA()
        {
             
            this.m_CareerFile = new CareerFile(m_InternalFile, this.m_FifaDbXmlFileName);
            m_DataSetEa = this.m_CareerFile.ConvertToDataSet();

         

        }

        public void SaveEA()
        {

            //Console.WriteLine(text);         
            this.m_CareerFile.ConvertFromDataSet(this.m_DataSetEa);
            var directoryName = Path.GetDirectoryName(this.m_CareerFile.FileName);
            var fileName = Path.GetFileName(this.m_CareerFile.FileName);
            string backupFileName;
            var i = 0;
            do
            {
                backupFileName = directoryName + $"\\Backup{i}_" + fileName;
                i++;
                

            }
            while (File.Exists(backupFileName));            

            File.Copy(this.m_CareerFile.FileName, backupFileName, true);
            this.m_CareerFile.SaveEa(this.m_CareerFile.FileName);

        }
    }
}
