using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Memory3
{
    public class SqliteDataAccess
    {
        public static List<Highscores> LoadHighscores()
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = conn.Query<Highscores>("SELECT * FROM Time ORDER BY TimeDouble DESC LIMIT 10", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveHighscore(Highscores highscore)
        {
            using IDbConnection conn = new SQLiteConnection(LoadConnectionString());
            conn.Execute("INSERT INTO Time (TimeString, TimeDouble) VALUES (@TimeString, @TimeDouble)", highscore);
        }

        private static string LoadConnectionString(string id="Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
