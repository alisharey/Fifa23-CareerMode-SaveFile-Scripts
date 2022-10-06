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
        public DataRowCollection PlayersTable { get; private set; }
        public DataRowCollection TeamPlayerLinksTable { get; private set; }
        public List<string> MyTeamPlayerIDs { get; private set; }
        public Dictionary<string, string> MyTeamPlayerNamesDict { get; private set; }
        public CareerInfo(string myTeamID, DataRowCollection playersTable, DataRowCollection teamPlayerLinksTable, List<string> myTeamPlayerIDs, Dictionary<string, string> MTPITN)
        {
            this.MyTeamID = myTeamID;
            this.PlayersTable = playersTable;
            this.TeamPlayerLinksTable = teamPlayerLinksTable;
            this.MyTeamPlayerIDs = myTeamPlayerIDs;
            this.MyTeamPlayerNamesDict = MTPITN;
        }
    }
}
