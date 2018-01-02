using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

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

        private const int card_suit = 0;
        private const int card_number = 1;
        private const int card_picture = 2;

        private Dictionary<string, string[]> cardInfo = new Dictionary<string, string[]>();

        public Play()
        {
            InitializeComponent();
            
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

            MessagingCenter.Subscribe<Cards, Dictionary<string, string>>(this, "Cards_Pulled", (sender, e) => {

                Dictionary<string, string> selectedCards = e;
                                
                foreach (var card in selectedCards)
                {
                    string[] cardInformation = new string[3];
                    string cardName = card.Value.Replace(".jpg", "");
                    int charLocation = cardName.IndexOf("_", StringComparison.Ordinal);

                    cardInformation [card_suit] = cardName.Substring(0, charLocation);
                    cardInformation [card_number] = cardName.Substring(charLocation + 1);
                    cardInformation[card_picture] = card.Value;

                    if(cardInformation[card_number] == "Jack")
                    {
                        cardInformation[card_number] = "11";
                    }
                    else if (cardInformation[card_number] == "Queen")
                    {
                        cardInformation[card_number] = "12";
                    }
                    else if (cardInformation[card_number] == "King")
                    {
                        cardInformation[card_number] = "13";
                    }
                    else if (cardInformation[card_number] == "Ace")
                    {
                        cardInformation[card_number] = "14";
                    }

                    cardInfo.Add(card.Key, cardInformation);
                }
                bestHand();

            });

        }

        private void Start_Game(object sender, EventArgs e)
        {
            TimeSpan time = new TimeSpan(0, 0, 1);

            Xamarin.Forms.Device.StartTimer(time, updateBlind);
                        
        }

        private void bestHand()
        {
            Dictionary<string, string> cardSuit = new Dictionary<string, string>();
            Dictionary<string, int> cardNumber = new Dictionary<string, int>();

            foreach (var card in cardInfo)
            {
                cardNumber.Add(card.Key, Int32.Parse(card.Value[card_number])); 
            }
            
            if (cardInfo.Count < 3)
            {
                if (Convert.ToInt32(cardInfo["Card_1"][card_number]) > Convert.ToInt32(cardInfo["Card_2"][card_number]))
                {
                    Card1.Source = cardInfo["Card_1"][card_picture];
                    Card2.Source = cardInfo["Card_2"][card_picture];
                }
                else
                {
                    Card1.Source = cardInfo["Card_2"][card_picture];
                    Card2.Source = cardInfo["Card_1"][card_picture];
                }

            }
            else
            {
                var myList = cardNumber.ToList();

                myList.Sort((x, y) => y.Value.CompareTo(x.Value));

                Card1.Source = cardInfo[myList[0].Key][card_picture];
                Card2.Source = cardInfo[myList[1].Key][card_picture];
                Card3.Source = cardInfo[myList[2].Key][card_picture];

                if(myList.ElementAtOrDefault(3).Key != null)
                {
                    Card4.Source = cardInfo[myList[3].Key][card_picture];
                }

                if (myList.ElementAtOrDefault(4).Key != null)
                {
                    Card5.Source = cardInfo[myList[4].Key][card_picture];
                }

            }


            cardInfo = new Dictionary<string, string[]>();


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