using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIFA23.Scripts
{
    public class CareerInfo
    {
        public string MyTeamID { get; private set; }     
        public DataSet MainDataSet { get; private set; }      
        public List<string> MyTeamPlayerIDs { get; private set; }
        public Dictionary<string, string> MyTeamPlayerNamesDict { get; private set; }
        public CareerInfo(DataSet MainDataSet,string MyTeamID,List<string> MyTeamPLayerIDs, Dictionary<string, string> MyTeamPLayerNamesToID )
        {
            this.MyTeamID = MyTeamID;           
            this.MyTeamPlayerIDs = MyTeamPLayerIDs;
            this.MyTeamPlayerNamesDict = MyTeamPLayerNamesToID;
            this.MainDataSet = MainDataSet;
        }
    }
}
