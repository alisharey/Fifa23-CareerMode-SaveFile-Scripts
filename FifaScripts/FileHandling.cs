﻿using FifaLibrary;
using System.Data;


namespace Fifa_Career_Script
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


        public FileHandling()
 
        {

            this.m_FifaDbFileName = Path.Combine(Environment.CurrentDirectory, @"Data\", "fifa_ng_db.db");
            this.m_FifaDbXmlFileName = Path.Combine(Environment.CurrentDirectory, @"Data\", "fifa_ng_db-meta.xml");


        }

        public int Load()
        {
            //getting the file name and path
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = Path.Combine(userPath, @"OneDrive\Documents\FIFA 23\settings");

            if (DialogResult.OK == openDialog.ShowDialog())
            {
                this.m_InternalFile = openDialog.FileName;
            }
            if (string.IsNullOrEmpty(this.m_InternalFile)) return -1;  
            
            LoadEA();
            return 0;
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
            //var x = this.m_CareerFile.InGameName;

        }
        
        public void SaveEA()
        {
            var text  = "Saving ...";            
            //Console.WriteLine(text);         
            this.m_CareerFile.ConvertFromDataSet(this.m_DataSetEa);            
            string directoryName = Path.GetDirectoryName(this.m_CareerFile.FileName);
            string fileName = Path.GetFileName(this.m_CareerFile.FileName);
            for (int i = 1; i <= 99; i++)
            {
                text = string.Concat(new string[]
                {
            directoryName,
            "\\_",
            i.ToString(),
            "_",
            fileName
                });
                if (!File.Exists(text))
                {
                    break;
                }
            }
            File.Copy(this.m_CareerFile.FileName, text, true);
            fileName.StartsWith("Squad");
            fileName.StartsWith("Career");
            this.m_CareerFile.SaveEa(this.m_CareerFile.FileName);
             
        }
    }
}
