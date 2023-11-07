using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Memory3;

public class MemoryGame
{
    //variables
    private readonly int[] _cards;
    private int _conditionCounter = 0;
    private Button[] _buttons;
    private int _counter = 0;
    private int[] _clicked = new int[2];
    private bool[] _opened;
    private static double _lastScore;
    

    public MemoryGame(Button[] buttons, int[] cards, int amountOfCards)
    {
        _cards = new int[amountOfCards];
        _opened = new bool[amountOfCards];
        _buttons = buttons;
        _cards = cards;
        InitCards(buttons);
    }

    //set all cards to question mark
    private void InitCards(Button?[] buttons)
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            buttons[i]!.Opacity = 1;
            buttons[i]!.Background = SetIcon();
        }
    }

    //FOR SETTING AN IMAGE, I USED AN ABSOLUTE PATH, BECAUSE A RELATIVE PATH BREAKS WHEN RUNNING IN A VIRTUALIZED WINDOWS ENVIRONMENT
    
    //set card to image --> cardIcons is made optional. If no card icon is being sent, it must be a questionmark
    private static ImageBrush SetIcon(CardNames.CardIcons cardIcons = CardNames.CardIcons.first, string type = "questionmark")
    {
        //check wheter the card needs to be a questionmark or the right image
        var image = "questionmark";
        switch (type)
        {
            case "image":
            {
                image = cardIcons.ToString();
                break;
            }
            case "questionmark":
            {
                image = "questionmark";
                break;
            }
        }
        //create an ImageBrush
        var brush = new ImageBrush
        {
            //set the source of the image based on the switch case
            ImageSource =
                new BitmapImage(new Uri(@"C:\Users\Tom\RiderProjects\Memory3\Data\Assets\" + image + ".jpg",
                    UriKind.Relative))
        };
        //return the brush --> it will be applied to the card where the method is invoked
        return brush;
    }

    //turn card
    public void ShowCard(Button[] buttons, int index, TimeSpan time, DispatcherTimer timer)
    {
        //_counter == 2 -->  2 cards have already been turned
        //_opened[index] --> the card that the player is trying to turn is already turned
        //_counter == 1 && _clicked[0] == index --> I don't remember but when I delete it everything breaks!!!!  
        if (_counter == 2 || _opened[index] || _counter == 1 && _clicked[0] == index)
            return;

        //set the clicked card to the image underneath
        _buttons[index].Background = SetIcon((CardNames.CardIcons)_cards[index], "image");

        //set _clicked to index, with index _counter --> _counter can be 0 or 1, when it's 2 it will enter the if-statement
        _clicked[_counter] = index;
        //increase counter
        _counter++;
        //check if 2 cards are turned
        if (_counter == 2)
        {
            //if so, compare them
            ButtonCompare(buttons, _clicked[0], _clicked[1], time, timer);
        }
    }

    //here the program compares two clicked cards
    private void ButtonCompare(Button[] box, int check1, int check2, TimeSpan time, DispatcherTimer timer)
    {
        //if the images of the two cards are the same then enter the if-statement
        if (_cards[check1] == _cards[check2])
        {
            //increase condition counter --> used to determine if the player has won the game
            _conditionCounter++;
           
            //the images stay on the screen, because the player has found the pair
            _opened[check1] = true;
            _opened[check2] = true;
            
            //check if all pairs are found
            if (_conditionCounter == _cards.Length / 2)
            {
                //stop the timer
                timer.Stop();
                
                //convert _time into usable vars
                var timeString = time.ToString();
                var timeSeconds = time.TotalSeconds;
                
                //set the last score so it can be displayed
                SetLastScore(timeSeconds);
                
                //save highscore
                var highScore = new Highscores(timeString, timeSeconds);
                
                //write to txt
                Highscores.SaveHighscore(highScore.TimeString);
                
                //show player that they have won
                MessageBox.Show($"YOU WIN! YOUR TIME: {time}");
            }
            // if the player didn't win yet, set the counter back to zero
            _counter = 0;
        }
        //if the player didn't choose the correct cards
        else
        {
            //create a timer
            var delayBeforeHidingCard = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            
            //start the timer so that the player can see the cards they chose
            delayBeforeHidingCard.Start();
            delayBeforeHidingCard.Tick += (sender, args) =>
            {
                //stop the timer
                delayBeforeHidingCard.Stop();
                
                //set cards back to the question mark
                _buttons[check1]!.Background = SetIcon();
                _buttons[check2]!.Background = SetIcon();
                
                //set counter back to zero
                _counter = 0;
            };
        }
    }
    
    //set the last score the player achieved
    private static void SetLastScore(double last)
    {
        _lastScore = last;
    }
    //retrieve the last score the player achieved, so the program can display it 
    public static double GetLastScore()
    {
        return _lastScore;
    }
}
