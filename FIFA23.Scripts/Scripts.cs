using System.Data;
using static System.Windows.Forms.AxHost;


namespace FIFA23.Scripts
{

    public class Scripts
    {
        private DataSet[] dataSetCollection;
        private string myteamid;
        private int season;
        private List<string> myTeamPlayerIDs;
        private DataRowCollection _allplayerInfo;
        static List<string> PlayerStats = new List<string>
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
            this.myteamid = GetMyTeamID(dataSetCollection);
            this.season = int.Parse(GetSeasonCount(dataSetCollection))  - 1;
            this.myTeamPlayerIDs = GetMyTeamPlayerIDs(dataSetCollection, myteamid);
            this._allplayerInfo = GetAllPlayerInfo(dataSetCollection);
        }

        #region Private/Internal Methods

        private string GetMyTeamID(DataSet[] dataSetCollection)
        {
            return dataSetCollection[0].Tables["career_users"].Rows[0]["clubteamid"].ToString();
        }
        private string GetSeasonCount(DataSet[] dataSetCollection)
        {
            return dataSetCollection[0].Tables["career_users"].Rows[0]["seasoncount"].ToString();
        }

        private List<string> GetMyTeamPlayerIDs(DataSet[] dataSetCollection, string myTeamID)
        {
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
        private static DataRowCollection GetAllPlayerInfo(DataSet[] dataSetCollection)
        {

            return dataSetCollection[1].Tables["players"].Rows;
        }

        #endregion

        #region Public Scripts

        public void UserTeamSingleStatScript(string stat)
        {
            foreach (DataRow _player in _allplayerInfo)
            {
                string? playerID = _player["playerid"].ToString();
                if (myTeamPlayerIDs.Contains(playerID))
                {
                    if(stat == "birthdate") _player[stat] = 155185 + (this.season * 365);
                    else _player[stat] = 99;

                }


            }
        }
        public void TempScriptForAllStats()
        {
            foreach (DataRow _player in _allplayerInfo)
            {
                string? playerID = _player["playerid"].ToString();
                if (myTeamPlayerIDs.Contains(playerID))
                {
                   

                    foreach (string stat in PlayerStats)
                    {

                        _player[stat] = 99;

                    }
                }


            }




        }

        #endregion


    }
}
