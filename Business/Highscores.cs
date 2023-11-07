namespace Memory3
{
    public class Highscores
    {
        //path to .txt file is stored in GetTxtPath.cs
        private static readonly string Path = GetTxtPath.getTxtPath();
        
        public readonly string TimeString;
        public readonly double TimeDouble;

        public Highscores(string timeString, double timeDouble)
        {
            TimeString = timeString;
            TimeDouble = timeDouble;
        }

        //save the highscore in a .txt file
        public static void SaveHighscore(string highScore)
        {
            using var writer = new StreamWriter(Path, true);
            writer.WriteLine(highScore, Environment.NewLine);
        }

        //retrieve highscores
        public static List<string> GetHighscores()
        {
            //create an empty list
            var highscores = new List<string>();
            //using a try-catch to catch possible exceptions
            try
            {
                var sr = new StreamReader(Path);
                //count how many highscores there are
                var lineCount = File.ReadLines(Path).Count();

                //linecount == 0 --> there are no scores yet - a .txt file always has at least 1 line
                if (lineCount > 0)
                {
                    //read all scores and put them in the list
                    for (int i = 0; i < lineCount; i++)
                    {
                        var line = sr.ReadLine();
                        highscores.Add(line);
                    }
                }
                sr.Close();
            }
            //if something went wrong
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //sort the list and return it
            return SortHighscores(highscores);
        }

        private static List<string> SortHighscores(List<string> list)
        {
            //sort and remove duplicates
            return list.OrderBy(x => x, new SemiNumericComparer()).Distinct().ToList();
        }
    }
}
