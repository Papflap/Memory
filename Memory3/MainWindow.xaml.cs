﻿using System;
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
        private const int AmountOfCards = 12;
        public MainWindow()
        {
            InitializeComponent();
        }
        
        // private void StartTimer()
        // {
        //     DispatcherTimer timer = new DispatcherTimer();
        //     timer.Tick += new EventHandler(UpdateTimer_Tick!);
        //     timer.Interval = new TimeSpan(0, 0, 1);
        //     timer.Start();
        // }
        // private void UpdateTimer_Tick(object sender, EventArgs e)
        // {
        //     DisplayTimer.Text = DateTime.Now.ToString();
        // }

        private MemoryGame _currentGame;
        private readonly Button?[] _buttons = new Button?[AmountOfCards];
        private readonly Random _randomGenerate = new Random();
        private readonly int[] _inputList = new int[AmountOfCards];
        private bool _playing;
        
        private enum CardIcons
        {
            first = 1,
            second = 2,
            third = 3,
            fourth = 4,
            fifth = 5,
            sixth = 6,
        }

        private class MemoryGame
        {
            private readonly int[] _cards = new int[AmountOfCards];
            private int _conditionCounter = 0;
            private Button[] _buttons;
            private int _counter = 0;
            private int[] _clicked = new int[2];
            private bool[] _opened = new bool[AmountOfCards];

            public MemoryGame(Button?[] buttons, int[] cards)
            {
                _buttons = buttons;
                _cards = cards;
                InitCards(buttons);
            }

            private void InitCards(Button?[] buttons)
            {
                for (int i = 0; i < AmountOfCards; i++)
                {
                    buttons[i]!.Opacity = 1;
                    buttons[i]!.Background = SetIcon();
                }
            }

            private ImageBrush SetIcon(CardIcons cardIcons)
            {
                var brush = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(@"C:\Users\Tom\RiderProjects\Memory3\Memory3\Assets\"+cardIcons+".jpg", UriKind.RelativeOrAbsolute))
                };

                return brush;
            }
            private ImageBrush SetIcon()
            {
                var brush = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(@"C:\Users\Tom\RiderProjects\Memory3\Memory3\Assets\questionmark.jpg", UriKind.RelativeOrAbsolute))
                };

                return brush;
            }
            public void ShowCard(Button[] buttons, int index)
            {
                if (_counter == 2 || _opened[index] || _counter == 1 && _clicked[0] == index)
                    return;
                
                _buttons[index].Background = SetIcon((CardIcons)_cards[index]);

                _clicked[_counter] = index;
                _counter++;
                if (_counter == 2)
                {
                    ButtonCompare(buttons, _clicked[0], _clicked[1]);
                }
            }
            
            private void ButtonCompare(Button[] box, int check1, int check2)
            {
                
                if (_cards[check1] == _cards[check2])
                {
                    _conditionCounter++;
                    _opened[check1] = true;
                    _opened[check2] = true;
                    if (_conditionCounter == AmountOfCards/2) {
                        MessageBox.Show("You Won!!!!!");
                    }
                    _counter = 0;
                }
                else {
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
            
        }
        
        
        //button and card handlers
        private void Card_OnClick(object sender, RoutedEventArgs e)
        {
            if (_currentGame == null) return;
            var buttonName = int.Parse((sender as Button).Name.Substring(4))-1;
            _currentGame.ShowCard(_buttons, buttonName);
        }

        private void Button_Onclick(Object? sender, RoutedEventArgs e)
        {
            if (!_playing)
            {
                if (sender.Equals(Start))
                {
                    var Icon = Enumerable.Range(1, AmountOfCards).OrderBy(x => _randomGenerate.Next()).ToArray();
                    for (int i = 0; i < AmountOfCards; i++)
                    {
                        _buttons[i] = WrapPanel.Children[i] as Button;
                        _inputList[i] = (Icon[i] - 1) % AmountOfCards / 2 + 1;
                    }

                    _currentGame = new MemoryGame(_buttons, _inputList);
                }

                _playing = true;
            }
            else if (sender.Equals(Restart))
            {
                var Result = MessageBox.Show("Are you sure you want to restart?", "Restart", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (!Result.Equals(MessageBoxResult.Yes)) return;
                
                var Icon = Enumerable.Range(1, AmountOfCards).OrderBy(x => _randomGenerate.Next()).ToArray();
                for (int i = 0; i < AmountOfCards; i++)
                {
                    _buttons[i] = WrapPanel.Children[i] as Button;
                    _inputList[i] = (Icon[i] - 1) % AmountOfCards / 2 + 1;
                }

                _currentGame = new MemoryGame(_buttons, _inputList);
            }
            else if (sender.Equals(Stop))
            {
                var Result = MessageBox.Show("Are you sure you want to exit?", "EXIT", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (Result.Equals(MessageBoxResult.Yes))
                {
                    Application.Current.Shutdown();
                }
            }
        }
    }
}