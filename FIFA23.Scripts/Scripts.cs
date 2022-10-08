using DocumentFormat.OpenXml.Wordprocessing;
using FifaLibrary;
using System.Data;
using static System.Windows.Forms.AxHost;


namespace FIFA23.Scripts
{

    public class Scripts
    {
        private int IndexOffset;
        private DataSet[] dataSetCollection { get; set; }
        private DataRowCollection PlayersTable;
        private DataRowCollection Teamplayerlinks;

        private string myteamid;
        private int season;
        private FileType fileType;
        private List<string> MyTeamPlayerIDs;
        private FileHandling File;
        public static List<string> PlayerStats { get; private set; } = new List<string>
            {
                   "overallrating",
                   "potential",
                   "birthdate",
                   //Pace 
                   "acceleration",
                   "sprintspeed",
                  
                  //Shooting
                   "positioning",
                   "finishing",
                   "shotpower",
                   "longshots",
                   "volleys",
                   "penalties",

                   //Passing
                    "vision",
                    "crossing",
                    "freekickaccuracy",
                    "shortpassing",
                    "longpassing",
                    "curve",

                   //Dribbling
                    "agility",
                    "balance",
                    "reactions",
                    "ballcontrol",
                    "dribbling",
                    "composure",
                  
                    //Defending
                     "interceptions",
                     "defensiveawareness",
                     "standingtackle",
                     "slidingtackle",
                     "headingaccuracy",
                    
                     //Physicality
                     "jumping",
                     "stamina",
                     "strength",
                     "aggression",


                    //GK
                     "gkdiving",
                     "gkhandling",
                     "gkkicking",
                     "gkpositioning",
                     "gkreflexes",
        };
        private Dictionary<string, string> MyTeamPlayersIDtoName;

        public Scripts(FileHandling _file)
        {
            this.File = _file;
            this.dataSetCollection = _file.m_DataSetEa;
            this.fileType = _file.Type;
           
            if (fileType == FileType.Career)
            {
                IndexOffset = 1;
                this.myteamid = GetMyTeamID();
                this.MyTeamPlayerIDs = GetMyTeamPlayerIDs(myteamid);
             
            }
            else IndexOffset = 0;
            this.PlayersTable = GetPlayersTable();
            this.Teamplayerlinks = GetTeamPlayerLinks();

            if (fileType == FileType.Career) this.MyTeamPlayersIDtoName = GetMyTeamPLayerNames();

        }


        #region  Getters
        private string GetMyTeamID()
        {
            if (this.fileType != FileType.Career) return null;
            return dataSetCollection[0].Tables["career_users"].Rows[0]["clubteamid"].ToString();
        }
        private string GetSeasonCount()
        {
            return dataSetCollection[0].Tables["career_users"].Rows[0]["seasoncount"].ToString();
        }
        private List<string> GetMyTeamPlayerIDs(string myTeamID)
        {
            if (this.fileType != FileType.Career) return null;
            var tempPlayerList = new List<string>();
            DataRowCollection _playersContractInfo = dataSetCollection[0].Tables["career_playercontract"].Rows;
            foreach (DataRow _player in _playersContractInfo)
            {
                if (_player["teamid"].ToString() == myTeamID)
                {
                    string playerID = _player["playerid"].ToString();                   
                    tempPlayerList.Add(playerID);
                    Console.WriteLine($"{playerID} is in Team {myTeamID}");
                }
            }
            return tempPlayerList;
        }
        private Dictionary<string, string> GetMyTeamPLayerNames()
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();

            foreach (DataRow _player in PlayersTable)
            {
                string? playerID = _player["playerid"].ToString();
                if (MyTeamPlayerIDs.Contains(playerID))
                {
                    
                    var tempNameID = _player["commonnameid"].ToString();                   
                    if(tempNameID == "0")
                    {
                        tempNameID = _player["lastnameid"].ToString();
                    }

                    foreach (DataRow name in File.m_PlayerNames.Rows)
                    {
                        var nameid = name["nameid"].ToString();
                        if(nameid == tempNameID)
                        {
                            temp.Add(playerID, name["name"].ToString());
                        }

                    }
                }


            }

            return temp;
        }        
        private DataRowCollection GetPlayersTable()
        {
            return dataSetCollection[IndexOffset].Tables["players"].Rows;


        }
        private DataRowCollection GetTeamPlayerLinks()
        {
            return dataSetCollection[IndexOffset].Tables["teamplayerlinks"].Rows;

        }
        #endregion





        public CareerInfo ExportCareerInfo()
        {

            var careerInfo = new CareerInfo(dataSetCollection[IndexOffset], myteamid, MyTeamPlayerIDs,
                MyTeamPlayersIDtoName);

            return careerInfo;
        }
        public int ImportCareerInfo(CareerInfo careerInfo)
        {

            this.myteamid        = careerInfo.MyTeamID;
            this.MyTeamPlayerIDs = careerInfo.MyTeamPlayerIDs;
            this.MyTeamPlayersIDtoName = careerInfo.MyTeamPlayerNamesDict;
            ImportDataSet(careerInfo);

         

            return 0;
        }
        private void ImportDataSet(CareerInfo careerInfo)
        {


            foreach(DataTable savedTable in careerInfo.MainDataSet.Tables)
            {
                string tablename = savedTable.TableName;
                if (tablename == "manager") continue; 


                var TargetTable = dataSetCollection[IndexOffset].Tables[tablename].Rows;

                foreach(DataRow row in savedTable.Rows)
                {
                    TargetTable.RemoveAt(0);
                    TargetTable.Add(row.ItemArray);
                }

            }

         


        }
     

      

        public void SetPlayerStat(string playerID, string stat, int value = 99)
        {
            if (stat == "birthdate") value = 154482;
            foreach (DataRow _player in PlayersTable)
            {

                string? _playerID = _player["playerid"].ToString();
                if (_playerID == playerID)
                {

                    _player[stat] = value;
                }
            }

        }

        public void SetPlayerStat(List<string> playerIDs, string stat, int value = 99)
        {
            if (stat == "birthdate") value = 154482;
            foreach (DataRow _player in PlayersTable)
            {

                string? _playerID = _player["playerid"].ToString();
                if (playerIDs.Contains(_playerID))
                {

                    _player[stat] = value;
                }
            }
        }




    }
}
