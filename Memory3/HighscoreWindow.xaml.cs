using System;
using System.Collections.Generic;
using System.Windows;

namespace Memory3;

public partial class HighscoreWindow : Window
{
    public HighscoreWindow()
    {
        InitializeComponent();
        List<Highscores> highscoresList = SqliteDataAccess.LoadHighscores();
        for (int i = 0; i < highscoresList.Count; i++)
        {
            if (i == 0)
            {
                HighScores.Text += $"Nr {i+1}: {highscoresList[i].TimeDouble} seconds - HIGHSCORE!  \n";
            }
            else
            {
                HighScores.Text += $"Nr {i + 1}: {highscoresList[i].TimeDouble} seconds \n";
            }
        }
    
    }
}