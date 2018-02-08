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

            bool duplicateNumbers = false;
            List<string> duplicates = new List<string>();

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

                //check for any duplicates
                var pairs = cardNumber.ToLookup(x => x.Value, x => x.Key).Where(x => x.Count() > 1);
                foreach (var item in pairs)
                {
                    duplicateNumbers = true;
                    duplicates.Add(item.Aggregate("", (s, v) => s + v + ", "));
                }

                if (duplicateNumbers)
                {
                    pairHands(myList, duplicates);
                }
                else
                {
                    highCard(myList);
                }

            }


            cardInfo = new Dictionary<string, string[]>();
        }

        private void pairHands(List<KeyValuePair<string,int>> myList, List<string> pairs)
        {
            List <string> cardNumbers = new List<string>();
            int initialCount = 0;
            int fullHousePosition = 0;
            int loopCount = 0;

            foreach (string length in pairs)
            {
                int newCount = length.Length - length.Replace(",", "").Length;

                if(newCount > initialCount)
                {
                    initialCount = newCount;
                    fullHousePosition = loopCount;
                }

                loopCount++;
            }

            if (initialCount == 4)
            {
                pairs[fullHousePosition] = Regex.Replace(pairs[fullHousePosition], @"\s+", "");
                cardNumbers = pairs[fullHousePosition].Split(',').ToList();

                Card1.Source = cardInfo[cardNumbers[0]][card_picture];

                char theLastCharacter = cardNumbers[0][cardNumbers[0].Length - 1];
                int number = (int)Char.GetNumericValue(theLastCharacter);
                myList.RemoveAt(number - 1);

                Card2.Source = cardInfo[cardNumbers[1]][card_picture];

                theLastCharacter = cardNumbers[1][cardNumbers[1].Length - 1];
                number = (int)Char.GetNumericValue(theLastCharacter);
                myList.RemoveAt(number - 2);

                Card3.Source = cardInfo[cardNumbers[2]][card_picture];

                theLastCharacter = cardNumbers[2][cardNumbers[2].Length - 1];
                number = (int)Char.GetNumericValue(theLastCharacter);
                myList.RemoveAt(number - 3);

                Card4.Source = cardInfo[cardNumbers[3]][card_picture];

                theLastCharacter = cardNumbers[3][cardNumbers[3].Length - 1];
                number = (int)Char.GetNumericValue(theLastCharacter);
                myList.RemoveAt(number - 4);

                //Sort out the cards into decending order
                myList.Sort((x, y) => y.Value.CompareTo(x.Value));

                if (myList.ElementAtOrDefault(0).Key != null)
                {
                    Card5.Source = cardInfo[myList[0].Key][card_picture];
                }

                BestHand.Text = "Best Hand: Four of a Kind";
            }
            else if (pairs.Count == 1)
            {
                int count = pairs[0].Length - pairs[0].Replace(",", "").Length;
                pairs[0] = Regex.Replace(pairs[0], @"\s+", "");
                cardNumbers = pairs[0].Split(',').ToList();

                if (count == 2)
                {
                    Card1.Source = cardInfo[cardNumbers[0]][card_picture];

                    char theLastCharacter = cardNumbers[0][cardNumbers[0].Length - 1];
                    int number = (int)Char.GetNumericValue(theLastCharacter);
                    myList.RemoveAt(number - 1);

                    Card2.Source = cardInfo[cardNumbers[1]][card_picture];

                    theLastCharacter = cardNumbers[1][cardNumbers[1].Length - 1];
                    number = (int)Char.GetNumericValue(theLastCharacter);
                    myList.RemoveAt(number - 2);

                    //Sort out the cards into decending order
                    myList.Sort((x, y) => y.Value.CompareTo(x.Value));

                    Card3.Source = cardInfo[myList[0].Key][card_picture];

                    if (myList.ElementAtOrDefault(1).Key != null)
                    {
                        Card4.Source = cardInfo[myList[1].Key][card_picture];
                    }

                    if (myList.ElementAtOrDefault(2).Key != null)
                    {
                        Card5.Source = cardInfo[myList[2].Key][card_picture];
                    }

                    BestHand.Text = "Best Hand: Pair";
                }
                else if (count == 3)
                {
                    Card1.Source = cardInfo[cardNumbers[0]][card_picture];

                    char theLastCharacter = cardNumbers[0][cardNumbers[0].Length - 1];
                    int number = (int)Char.GetNumericValue(theLastCharacter);
                    myList.RemoveAt(number - 1);

                    Card2.Source = cardInfo[cardNumbers[1]][card_picture];

                    theLastCharacter = cardNumbers[1][cardNumbers[1].Length - 1];
                    number = (int)Char.GetNumericValue(theLastCharacter);
                    myList.RemoveAt(number - 2);

                    Card3.Source = cardInfo[cardNumbers[2]][card_picture];

                    theLastCharacter = cardNumbers[2][cardNumbers[2].Length - 1];
                    number = (int)Char.GetNumericValue(theLastCharacter);
                    myList.RemoveAt(number - 3);

                    //Sort out the cards into decending order
                    myList.Sort((x, y) => y.Value.CompareTo(x.Value));

                    if (myList.ElementAtOrDefault(0).Key != null)
                    {
                        Card4.Source = cardInfo[myList[0].Key][card_picture];
                    }

                    if (myList.ElementAtOrDefault(1).Key != null)
                    {
                        Card5.Source = cardInfo[myList[1].Key][card_picture];
                    }

                    BestHand.Text = "Best Hand: Three of a Kind";
                }
            }
            else if (pairs.Count == 2)
            {
                int count = pairs[0].Length - pairs[0].Replace(",", "").Length;
                int count2 = pairs[1].Length - pairs[1].Replace(",", "").Length;

                pairs[0] = Regex.Replace(pairs[0], @"\s+", "");
                pairs[1] = Regex.Replace(pairs[1], @"\s+", "");

                if (count == 2 && count2 == 2)
                {
                    pairs[0] = Regex.Replace(pairs[0], @"\s+", "");
                    pairs[1] = Regex.Replace(pairs[1], @"\s+", "");

                    cardNumbers = pairs[0].Split(',').ToList();
                    char theLastCharacter = cardNumbers[0][cardNumbers[0].Length - 1];
                    int number = (int)Char.GetNumericValue(theLastCharacter);

                    int highNumber = myList[number - 1].Value;

                    cardNumbers = pairs[1].Split(',').ToList();
                    theLastCharacter = cardNumbers[1][cardNumbers[1].Length - 1];
                    number = (int)Char.GetNumericValue(theLastCharacter);

                    int compareNumber = myList[number - 1].Value;

                    if (highNumber < compareNumber)
                    {
                        Card1.Source = cardInfo[cardNumbers[0]][card_picture];
                        Card2.Source = cardInfo[cardNumbers[1]][card_picture];
                    }
                    else
                    {
                        Card3.Source = cardInfo[cardNumbers[0]][card_picture];
                        Card4.Source = cardInfo[cardNumbers[1]][card_picture];
                    }

                    theLastCharacter = cardNumbers[0][cardNumbers[0].Length - 1];
                    number = (int)Char.GetNumericValue(theLastCharacter);
                    myList.RemoveAt(number - 1);

                    theLastCharacter = cardNumbers[1][cardNumbers[1].Length - 1];
                    number = (int)Char.GetNumericValue(theLastCharacter);
                    myList.RemoveAt(number - 2);

                    cardNumbers = pairs[0].Split(',').ToList();

                    if (highNumber > compareNumber)
                    {
                        Card1.Source = cardInfo[cardNumbers[0]][card_picture];
                        Card2.Source = cardInfo[cardNumbers[1]][card_picture];
                    }
                    else
                    {
                        Card3.Source = cardInfo[cardNumbers[0]][card_picture];
                        Card4.Source = cardInfo[cardNumbers[1]][card_picture];
                    }

                    theLastCharacter = cardNumbers[0][cardNumbers[0].Length - 1];
                    number = (int)Char.GetNumericValue(theLastCharacter);
                    myList.RemoveAt(number - 1);

                    theLastCharacter = cardNumbers[1][cardNumbers[1].Length - 1];
                    number = (int)Char.GetNumericValue(theLastCharacter);

                    if (myList.Count == 1)
                    {
                        myList.RemoveAt(0);
                    }
                    else if (myList.Count == 2)
                    {
                        myList.RemoveAt(number - 2);
                    }
                    else if (myList.Count == 3)
                    {
                        myList.RemoveAt(number - 3);
                    }


                    //Sort out the cards into decending order
                    myList.Sort((x, y) => y.Value.CompareTo(x.Value));

                    if (myList.ElementAtOrDefault(0).Key != null)
                    {
                        Card5.Source = cardInfo[myList[0].Key][card_picture];
                    }

                    BestHand.Text = "Best Hand: Two Pair";
                }
                else
                {
                    if (count == 3)
                    {
                        pairs[0] = Regex.Replace(pairs[0], @"\s+", "");
                        cardNumbers = pairs[0].Split(',').ToList();
                    }
                    else if (count2 == 3)
                    {
                        pairs[1] = Regex.Replace(pairs[1], @"\s+", "");
                        cardNumbers = pairs[1].Split(',').ToList();
                    }

                    Card1.Source = cardInfo[cardNumbers[0]][card_picture];
                    Card2.Source = cardInfo[cardNumbers[1]][card_picture];
                    Card3.Source = cardInfo[cardNumbers[2]][card_picture];

                    if (count == 2)
                    {
                        pairs[0] = Regex.Replace(pairs[0], @"\s+", "");
                        cardNumbers = pairs[0].Split(',').ToList();
                    }
                    else if (count2 == 2)
                    {
                        pairs[1] = Regex.Replace(pairs[1], @"\s+", "");
                        cardNumbers = pairs[1].Split(',').ToList();
                    }

                    Card4.Source = cardInfo[cardNumbers[0]][card_picture];
                    Card5.Source = cardInfo[cardNumbers[1]][card_picture];

                    BestHand.Text = "Best Hand: Full House";
                }
            }
        }

        private void highCard(List<KeyValuePair<string, int>> myList)
        {
            //Sort out the cards into decending order
            myList.Sort((x, y) => y.Value.CompareTo(x.Value));

            Card1.Source = cardInfo[myList[0].Key][card_picture];
            Card2.Source = cardInfo[myList[1].Key][card_picture];
            Card3.Source = cardInfo[myList[2].Key][card_picture];

            if (myList.ElementAtOrDefault(3).Key != null)
            {
                Card4.Source = cardInfo[myList[3].Key][card_picture];
            }

            if (myList.ElementAtOrDefault(4).Key != null)
            {
                Card5.Source = cardInfo[myList[4].Key][card_picture];
            }

            BestHand.Text = "Best Hand: High Card";
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