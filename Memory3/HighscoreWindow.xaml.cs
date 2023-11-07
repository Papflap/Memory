using System.Windows;

namespace Memory3;

public partial class HighscoreWindow : Window
{
    public HighscoreWindow()
    {
        InitializeComponent();
        //retrieve highscores from Highscores.txt
        var highscoresList = Highscores.GetHighscores();
        
        //loop through the scores
        for (int i = 0; i < highscoresList.Count; i++)
        {
            //Display the score with the index (+1). 
            //List is already sorted, so it just loops from highest to loweset
            HighScores.Text += $"Nr {i+1}: {highscoresList[i]} seconds!";
            if (i == 0)
            {
                //if it is the first score aka the best score - display so
                HighScores.Text += "  -  BEST SCORE";
            }
            
            //add a new line so the scores are displayed nicely in a list form
            HighScores.Text += "\n";
        }
    }
}