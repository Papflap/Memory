namespace Memory3
{
    public class Highscores
    {
        //path to .txt file is stored in GetTxtPath.cs
        private static readonly string Path = GetTxtPath.getTxtPath();

        public readonly string TimeString; //used to display time
        public readonly double TimeDouble; //used to calculate score
        public readonly double Score;
        public readonly int AmountOfCards;

        public Highscores(string timeString, double timeDouble, int amountOfTurns, int amountOfCards)
        {
            TimeString = timeString;
            TimeDouble = timeDouble;
            AmountOfCards = amountOfCards;
            
            //((Aantal kaarten)^2 / (Tijd in seconden * aantal pogingen)) * 1000
            Score = Math.Floor(((amountOfCards*amountOfCards) / (timeDouble*amountOfTurns))*1000);
        }

        //save the highscore in a .txt file
        public static void SaveHighscore(string time, double score, int amountOfTurns, int amountOfCards)
        {
            using var writer = new StreamWriter(Path, true);
            writer.WriteLine($"{score} | {time} | {amountOfTurns} turns | {amountOfCards/2} sets", Environment.NewLine);
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
            var sortedList = Sort(list);
            return sortedList;
        }

        private static List<string> Sort(List<string> list)
        {
            //remove duplicate entries
            var sortedList = list.Distinct().OrderByDescending(x => x, new SemiNumericComparer()).ToList();
            return sortedList;
        }
    }
}
