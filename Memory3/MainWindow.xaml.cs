using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Controls;


namespace Memory3
{
    public partial class MainWindow : Window
    {
        DispatcherTimer _timer;
        TimeSpan _time;
        public const int AmountOfCards = 12;
        public MainWindow()
        {
            InitializeComponent();

        }
       
        private MemoryGame _currentGame;
        private readonly Button?[] _buttons = new Button?[AmountOfCards];
        private readonly Random _randomGenerate = new Random();
        private readonly int[] _inputList = new int[AmountOfCards];
        private bool _playing;
        

        
        private void Card_OnClick(object sender, RoutedEventArgs e)
        {
            if (_currentGame == null) return;
            var buttonName = int.Parse((sender as Button).Name.Substring(4))-1;
            _currentGame.ShowCard(_buttons, buttonName, _time, _timer);
        }


       //handler for three buttons
        private void Button_Onclick(Object sender, RoutedEventArgs e)
        {
            //start button
           
            if (sender.Equals(Start) && !_playing)
            {
                StartGame();
                _playing = true;
            }
  
            //restart button
            else if (sender.Equals(Restart) && _playing)
            {
                _timer.Stop();
                var Result = MessageBox.Show("Are you sure you want to restart?", "Restart", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (!Result.Equals(MessageBoxResult.Yes))
                {
                    _timer.Start();
                    return;
                }

                StartGame();
            }
            //stop button
            else if (sender.Equals(Stop) && _playing)
            {
                _timer.Stop();
                var Result = MessageBox.Show("Are you sure you want to exit?", "EXIT", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (Result.Equals(MessageBoxResult.Yes))
                {
                    Application.Current.Shutdown();
                }
                _timer.Start();
            }
        }

        private void StartTimer()
        {
            DisplayTimer.Text = "00:00:00";
            _time = TimeSpan.Zero;
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {

                _time = _time.Add(TimeSpan.FromSeconds(1));
                DisplayTimer.Text = _time.ToString();

            }, Application.Current.Dispatcher);
            _timer.Start();
        }

        private void StartGame()
        {
            StartTimer();

            var Icon = Enumerable.Range(1, AmountOfCards).OrderBy(x => _randomGenerate.Next()).ToArray();
            for (int i = 0; i < AmountOfCards; i++)
            {
                _buttons[i] = WrapPanel.Children[i] as Button;
                _inputList[i] = (Icon[i] - 1) % AmountOfCards / 2 + 1;
            }

            HighScore.Text = SqliteDataAccess.LoadHighscores().ElementAt(0).ToString();

            _currentGame = new MemoryGame(_buttons, _inputList, AmountOfCards);
        }
    }
}