﻿using System;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Poker_dream
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Play : ContentPage
    {
        int timeConverted = 0;
        int timeCounter = 0;
        int originalMinutes = 0;
        int minutes = 0;
        int seconds = 0; 
        bool finishGame = false;

        public Play()
        {
            InitializeComponent();
            Card1.Source = ImageSource.FromFile("Card_6.jpg");
            Card2.Source = ImageSource.FromFile("Card_7.jpg");
            Card3.Source = ImageSource.FromFile("Card_8.jpg");
            Card4.Source = ImageSource.FromFile("Card_9.jpg");
            Card5.Source = ImageSource.FromFile("Card_10.jpg");

            MessagingCenter.Subscribe<Settings, string>(this, "PlayersRole", (sender, e) => { PlayerRole.Text = "Your role: " + e; });
            MessagingCenter.Subscribe<Settings, string>(this, "BlindAmount", (sender, e) => {

                int bigBlind = Convert.ToInt32(e);
                bigBlind = bigBlind * 2;

                BigBlind.Text = "Big Blind: " + bigBlind.ToString();
                
                SmallBlind.Text = "Small Blind: " + e;
                SmallBlind.HorizontalOptions = LayoutOptions.End;

            });

            MessagingCenter.Subscribe<Settings, string>(this, "BlindTime", (sender, e) => {

                string time = Regex.Match(e, @"\d+").Value;

                originalMinutes = Convert.ToInt32(time);
                minutes = originalMinutes;
                timeConverted = originalMinutes * 60;

                BlindTimer.Text = "Next Blind Update: " + originalMinutes + ":00";

            });

        }

        private void Start_Game(object sender, EventArgs e)
        {
            TimeSpan time = new TimeSpan(0, 0, 1);

            Xamarin.Forms.Device.StartTimer(time, updateBlind);
                        
        }

        private bool updateBlind()
        {
            if (timeConverted == timeCounter)
            {
                minutes = originalMinutes;
                timeCounter = 0;

                string small = Regex.Match(SmallBlind.Text, @"\d+").Value;

                int smallConverted = Convert.ToInt32(small);
            
                int smallBlind = smallConverted * 2;

                SmallBlind.Text = "Small Blind: " + smallBlind.ToString();
                SmallBlind.HorizontalOptions = LayoutOptions.End;

                BigBlind.Text = "Big Blind: " + (smallBlind * 2).ToString();
            }
            else
            {
                timeCounter++;

                if (seconds == 0)
                {
                    seconds = 59;
                    minutes--;
                }
                else
                {
                    seconds--;
                }

                if (seconds == 0)
                {
                    BlindTimer.Text = "Next Blind Update: " + minutes + ":00";
                }
                else if (seconds < 10)
                {
                    BlindTimer.Text = "Next Blind Update: " + minutes + ":0" + seconds;
                }
                else
                {
                    BlindTimer.Text = "Next Blind Update: " + minutes + ":" + seconds;
                }
            }
            
            

            if (finishGame == false)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}