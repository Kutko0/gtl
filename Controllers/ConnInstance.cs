using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GTL.Controllers
{
    public class ConnInstance
    {
        private static string _connString = "Server=DESKTOP-GKCGP3R\\MSSQLSERVER01;Database=GTLDB;User ID=dbo;Integrated Security=true";
        
        private static SqlConnection instance = null;  
        public static SqlConnection Instance {  
        get {  
            if (instance == null) {  
                SqlConnection myConnection = new SqlConnection(ConnInstance._connString);
            }  
            return instance;  
        }  
    }  
    }
}