using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Fifa_Career_Script
{

    public class Scripts
    {
        private DataSet[] dataSetCollection;
        private string myteamid;
        private List<string> myTeamPlayerIDs;
        private DataRowCollection _allplayerInfo;
        static List<string> PlayerStats = new List<string>
            {
                "overallrating",
                "potential",

                //Skill
                "crossing",
                "dribbling",
                "curve",
                "freekickaccuracy",
                "longpassing",
                "ballcontrol",

                // Movement
                "acceleration",
                "sprintspeed",
                "agility",
                "reactions",
                "balance",

                // POWER
                "shotpower",
                "jumping",
                "stamina",
                "strength",
                "longshots",

                // MENTALITY
                "aggression",
                "interceptions",
                "positioning",
                "vision",
                "penalties",
                "composure",
                
                // DEFENDING
                "defensiveawareness",
                "standingtackle",
                "slidingtackle",

                // GOALKEEPING
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
            this.myTeamPlayerIDs = GetMyTeamPlayerIDs(dataSetCollection, myteamid);
            this._allplayerInfo = GetAllPlayerInfo(dataSetCollection);
        }

        private string GetMyTeamID(DataSet[] dataSetCollection)
        {
            return dataSetCollection[0].Tables["career_users"].Rows[0]["clubteamid"].ToString();
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
        public void MyTeamPlayersto99()
        {

            foreach (DataRow _player in _allplayerInfo)
            {
                string playerID = _player["playerid"].ToString();
                if (myTeamPlayerIDs.Contains(playerID))
                {
                    foreach (string stat in PlayerStats)
                    {
                        _player[stat] = 77;
                        //Console.WriteLine($" setting Player : {playerID}, {stat} == 99");
                    }
                }


            }


            // Console.WriteLine("Script Excuted");     
        }

        public void MyTeamPlayerAgeTo15()
        {
            foreach (DataRow _player in _allplayerInfo)
            {
                string playerID = _player["playerid"].ToString();
                if (myTeamPlayerIDs.Contains(playerID))
                {
                    _player["birthdate"] = 155185;

                }


            }

        }


    }
}
