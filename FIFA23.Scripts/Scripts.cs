using System.Data;
using static System.Windows.Forms.AxHost;


namespace FIFA23.Scripts
{

    public class Scripts
    {
        private int IndexOffset;
        private DataSet[] dataSetCollection { get; set; }
        private DataRowCollection AllplayerInfo;
        private DataRowCollection Teamplayerlinks;

        private string myteamid;
        private int season;
        private FileType m_FileType;
        private List<string> MyTeamPlayerIDs;
        private static List<string> PlayerStats = new List<string>
            {
                   "overallrating",
                   "potential",
                   
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
        public Scripts(FileHandling File)
        {
            this.dataSetCollection = File.m_DataSetEa;
            this.m_FileType = File.Type;
            if (m_FileType == FileType.Career)
            {
                IndexOffset = 1;
                this.myteamid = GetMyTeamID();
                this.MyTeamPlayerIDs = GetMyTeamPlayerIDs(myteamid);
            }
            else IndexOffset = 0;          
          
            this.AllplayerInfo = GetAllPlayerInfo();
            this.Teamplayerlinks = GetTeamPlayerLinks();
          

        }

        #region Private/Internal Methods

        private string GetMyTeamID()
        {
            if (this.m_FileType != FileType.Career) return null;
            return dataSetCollection[0].Tables["career_users"].Rows[0]["clubteamid"].ToString();
        }
        private string GetSeasonCount()
        {
            return dataSetCollection[0].Tables["career_users"].Rows[0]["seasoncount"].ToString();
        }
        private List<string> GetMyTeamPlayerIDs(string myTeamID)
        {
            if (this.m_FileType != FileType.Career) return null;
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
        private DataRowCollection GetAllPlayerInfo()
        {
            return dataSetCollection[IndexOffset].Tables["players"].Rows;


        }
        private DataRowCollection GetTeamPlayerLinks()
        {
            return dataSetCollection[IndexOffset].Tables["teamplayerlinks"].Rows;

        }

        private void ImportData(CareerInfo careerInfo)
        {
           
            this.AllplayerInfo = GetAllPlayerInfo();
            this.Teamplayerlinks = GetTeamPlayerLinks();
            this.AllplayerInfo.Clear();
            this.Teamplayerlinks.Clear();
            foreach (DataRow row in careerInfo.PlayersTable)
            {
                AllplayerInfo.Add(row.ItemArray);
            }

            foreach(DataRow row in careerInfo.TeamPlayerLinksTable)
            {
                Teamplayerlinks.Add(row.ItemArray);
            }

            //= careerInfo.PlayersTable;
            


        }


        #endregion

        #region Public Scripts

        public CareerInfo ExportCareerInfo()
        {

            var careerInfo = new CareerInfo(myteamid, AllplayerInfo, Teamplayerlinks, MyTeamPlayerIDs);

            return careerInfo;
        }
        public int ImportCareerInfo(CareerInfo careerInfo)
        {

            this.myteamid        = careerInfo.MyTeamID;
            this.MyTeamPlayerIDs = careerInfo.MyTeamPlayerIDs;
            ImportData(careerInfo);

         

            return 0;
        }
        public int UserTeamSingleStatScript(string stat)
        {

            var ret = CheckInvalidStat(stat);
            if (ret == -1) return -1; //invalid stat 

            foreach (DataRow _player in AllplayerInfo)
            {
                string? playerID = _player["playerid"].ToString();
                if (MyTeamPlayerIDs.Contains(playerID))
                {
                    switch (ret)
                    {
                        case 1:
                            _player[stat] = 99;
                            break;

                        case 2:
                            this.season = int.Parse(GetSeasonCount()) - 1;
                            _player[stat] = 155185 + (this.season * 365);
                            break;
                        default:
                            ret = -1;
                            return ret;

                    }



                }


            }
            return ret;
        }
        public void TempScriptForAllStats()
        {
            foreach (DataRow _player in AllplayerInfo)
            {


                foreach (string stat in PlayerStats)
                {

                    _player[stat] = 99;

                }

            }

        }

        #endregion

        private int CheckInvalidStat(string stat)
        {
            int ret;
            if (PlayerStats.Contains(stat))
            {
                ret = 1;
            }
            else if (stat == "birthdate")
            {
                ret = 2;
            }
            else
            {
                MessageBox.Show("Invalid Script/Stat Choice", "Invalid Choice", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ret = -1;
            }
            return ret;
        }
    }
}
