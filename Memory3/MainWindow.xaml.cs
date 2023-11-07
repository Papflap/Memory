using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;


namespace Memory3
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private TimeSpan _time;
        private const int AmountOfCards = 12;
        public MainWindow()
        {
            InitializeComponent();
        }
       
        private MemoryGame _currentGame;
        private readonly Button?[] _buttons = new Button?[AmountOfCards];
        private readonly Random _randomGenerate = new Random();
        private readonly int[] _inputList = new int[AmountOfCards];
        private bool _playing;

        //handles clicking on a card
        private void Card_OnClick(object sender, RoutedEventArgs e)
        {
            //defines the button name as an integer - names for all the cards are the same - "Card1", "Card2" etc.
            //subtracts 1 so buttonName can be used as an index
            var buttonName = int.Parse((sender as Button).Name.Substring(4))-1;
            
            //tell the program to show the image at the index of the clicked card
            //pass in timer _time and _timer to access it later on
            _currentGame.ShowCard(_buttons, buttonName, _time, _timer);
        }

       //handler for the buttons
        private void Button_Onclick(Object sender, RoutedEventArgs e)
        {
            //start button
            //only works if _playing is set to false (aka no game is active)
            if (sender.Equals(Start) && !_playing)
            {
                StartGame();
                _playing = true;
            }
  
            //restart button
            //only works if _playing is set to true (aka a game is active)
            else if (sender.Equals(Restart) && _playing)
            {
                //stop the timer
                _timer.Stop();
                
                //ask the player if they are sure they want to restart
                var result = MessageBox.Show("Are you sure you want to restart?", "Restart", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                //if the player says no:
                if (!result.Equals(MessageBoxResult.Yes))
                {
                    //start the timer back up
                    _timer.Start();
                    //and return back
                    return;
                }
                //if the player says yes:
                //display the last score
                LastScore.Text = "Last score: \n" + MemoryGame.GetLastScore() + " seconds";
                
                //start the game
                StartGame();
            }
            //stop button
            //only works if _playing is set to true (aka a game is active)
            else if (sender.Equals(Stop) && _playing)
            {
                //stop the timer
                _timer.Stop();
                
                //ask the player if they are sure they want to stop
                var result = MessageBox.Show("Are you sure you want to exit?", "EXIT", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                
                //if the player says yes:
                if (result.Equals(MessageBoxResult.Yes))
                {
                    //close the program
                    Application.Current.Shutdown();
                }
                
                //if the player says no:
                //start the timer back up
                _timer.Start();
            }
        }

        //starts the game timer
        private void StartTimer()
        {
            //set the text to 00:00:00 for continuity purposes
            DisplayTimer.Text = "Time: \n 00:00:00";
            
            //set the format
            _time = TimeSpan.Zero;
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                //add 1 second, each second
                _time = _time.Add(TimeSpan.FromSeconds(1));
                
                //update the timer to display the accurate time
                DisplayTimer.Text = "Time: \n"+_time.ToString();

            }, Application.Current.Dispatcher); //I have no idea what this does but I had to include it

            //starting the timer
            _timer.Start();
        }

        //starts the game
        private void StartGame()
        {
            //first start the timer
            StartTimer();

            //randomly assign images to cards
            var Icon = Enumerable.Range(1, AmountOfCards).OrderBy(x => _randomGenerate.Next()).ToArray();
            
            //loop through all the cards 
            for (int i = 0; i < AmountOfCards; i++)
            {
                //determine how to address the buttons
                _buttons[i] = WrapPanel.Children[i] as Button;
                _inputList[i] = (Icon[i] - 1) % AmountOfCards / 2 + 1;
            }
            //create a new game
            _currentGame = new MemoryGame(_buttons, _inputList, AmountOfCards);
        }

        //opens highscore window
        private void Highscore_OnClick(object sender, RoutedEventArgs e)
        {
            var highscoreWindow = new HighscoreWindow();
            highscoreWindow.Show();
        }
    }
}