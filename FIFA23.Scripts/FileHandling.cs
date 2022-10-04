﻿using FifaLibrary;
using System.Data;
using System.IO;


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
        public DataSet[] m_DataSetEa;

        public FileType Type { get; set; }


        public FileHandling()

        {

            this.m_FifaDbFileName = Path.Combine(Environment.CurrentDirectory, @"Data\", "fifa_ng_db.db");
            this.m_FifaDbXmlFileName = Path.Combine(Environment.CurrentDirectory, @"Data\", "fifa_ng_db-meta.xml");


        }

        
        public int Load()
        {
            int ret = 0;
            this.m_InternalFile = string.Empty;            
            //Open File Dialog to select file
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = Path.Combine(userPath, @"OneDrive\Documents\FIFA 23\settings");
            if (DialogResult.OK == openDialog.ShowDialog())
            {
                this.m_InternalFile = openDialog.FileName;
            }
            if (string.IsNullOrEmpty(this.m_InternalFile)) return -1; //if no file is selected


            //check the file type 
            var fileName =  Path.GetFileName(this.m_InternalFile);            
            if (fileName.StartsWith("Squad"))

            {
                this.Type = FileType.Squad;
            }
            else if (fileName.StartsWith("Career"))
            {
                this.Type = FileType.Career;
            }
            else 
            {
                
                MessageBox.Show("Select a file which starts with Squads or Career and try again");
                ret = this.Load();
                if(ret != 0) return ret;
               
            }
            
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