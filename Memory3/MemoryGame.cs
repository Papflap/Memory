using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;

namespace Memory3;

public class MemoryGame
{
    private readonly int[] _cards;
    private int _conditionCounter = 0;
    private Button[] _buttons;
    private int _counter = 0;
    private int[] _clicked = new int[2];
    private bool[] _opened;

    private static double lastScore;
    

    public MemoryGame(Button[] buttons, int[] cards, int amountOfCards)
    {
        _cards = new int[amountOfCards];
        _opened = new bool[amountOfCards];
        _buttons = buttons;
        _cards = cards;
        InitCards(buttons);
    }

    private void InitCards(Button?[] buttons)
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            buttons[i]!.Opacity = 1;
            buttons[i]!.Background = SetIcon();
        }
    }

    private ImageBrush SetIcon(CardNames.CardIcons cardIcons)
    {

        var brush = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri(@"C:\Users\Tom\RiderProjects\Memory3\Memory3\Assets\"+cardIcons+".jpg", UriKind.Relative))
        };

        return brush;
    }
    private ImageBrush SetIcon()
    {
        var brush = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri(@"C:\Users\Tom\RiderProjects\Memory3\Memory3\Assets\questionmark.jpg", UriKind.Relative))
        };

        return brush;
    }
    public void ShowCard(Button[] buttons, int index, TimeSpan _time, DispatcherTimer _timer)
    {
        if (_counter == 2 || _opened[index] || _counter == 1 && _clicked[0] == index)
            return;

        _buttons[index].Background = SetIcon((CardNames.CardIcons)_cards[index]);

        _clicked[_counter] = index;
        _counter++;
        if (_counter == 2)
        {
            ButtonCompare(buttons, _clicked[0], _clicked[1], _time, _timer);
        }
    }

    private void ButtonCompare(Button[] box, int check1, int check2, TimeSpan _time, DispatcherTimer _timer)
    {

        if (_cards[check1] == _cards[check2])
        {
            _conditionCounter++;
            _opened[check1] = true;
            _opened[check2] = true;
            if (_conditionCounter == _cards.Length / 2)
            {
                _timer.Stop();
                
                //convert _time into usable vars
                var timeString = _time.ToString();
                var timeSeconds = _time.TotalSeconds;
                
                SetLastScore(timeSeconds);
                
                Highscores _highScore = new Highscores(timeString, timeSeconds);
                
                SqliteDataAccess.SaveHighscore(_highScore);
                
                MessageBox.Show($"YOU WIN 100000000 BILION DOLAR. YOUR TIME: {_time}");
            }
            _counter = 0;
        }
        else
        {
            var delayBeforeHidingCard = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            delayBeforeHidingCard.Start();
            delayBeforeHidingCard.Tick += (sender, args) =>
            {
                delayBeforeHidingCard.Stop();
                _buttons[check1]!.Background = SetIcon();
                _buttons[check2]!.Background = SetIcon();
                _counter = 0;
            };
        }
    }
    
    public static double getHighscore()
    {
        List<Highscores> highscoresList = SqliteDataAccess.LoadHighscores();
        double highScore = highscoresList[0].TimeDouble;

        for (int i = 0; i < highscoresList.Count; i++)
        {
            if (highscoresList[i].TimeDouble < highScore)
            {
                highScore = highscoresList[i].TimeDouble;
            }
        }
        return highScore;
    }

    private void SetLastScore(double last)
    {
        lastScore = last;
    }
    public static double GetLastScore()
    {
        return lastScore;
    }
}
