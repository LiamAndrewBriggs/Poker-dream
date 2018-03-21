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
        int timeConverted = 600;
        int timeCounter = 0;
        int originalMinutes = 0;
        int minutes = 10;
        int seconds = 0; 
        bool finishGame = false;
        bool otherHand = true;

        private const int card_suit = 0;
        private const int card_number = 1;
        private const int card_picture = 2;

        private List<string> roles = new List<string>();
        private int numberPlayers;
        private int playerRoleNumber;

        private Dictionary<string, string[]> cardInfo = new Dictionary<string, string[]>();

        public Play()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<Settings, string>(this, "PlayerNumber", (sender, e) => { numberPlayers = Convert.ToInt32(e); });

            roles.Add("Dealer");
            roles.Add("Big Blind");
            roles.Add("Small Blind");
            roles.Add("Player");
            roles.Add("Player");
            roles.Add("Player");
            roles.Add("Player");
            roles.Add("Player");
            roles.Add("Player");
            roles.Add("Player");
            roles.Add("Player");
            roles.Add("Player");

            MessagingCenter.Subscribe<Settings, string>(this, "PlayersRole", (sender, e) => {
                PlayerRole.Text = "Your Role: " + e;
                int count = 0;

                foreach(var item in roles)
                {
                    if(item.Equals(e))
                    {
                        playerRoleNumber = count;
                    }
                    else
                    {
                        count++;
                    }
                }

            });

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

                playerRoleNumber++;
                if(playerRoleNumber == numberPlayers)
                {
                    playerRoleNumber = 0;
                }

                PlayerRole.Text = "Your Role: " + roles[playerRoleNumber];

                if (selectedCards.Count == 0)
                {
                    Card1.Source = "Not_Selected.jpg";
                    Card2.Source = "Not_Selected.jpg";
                    Card3.Source = "Not_Selected.jpg";
                    Card4.Source = "Not_Selected.jpg";
                    Card5.Source = "Not_Selected.jpg";

                    BestHand.Text = "Best Hand: N/A";
                    MessagingCenter.Send(this, "BestHand", BestHand.Text);
                }
                else
                {
                    foreach (var card in selectedCards)
                    {
                        if (!(card.Key.Contains("_Number")))
                        {
                            string[] cardInformation = new string[3];
                            string cardName = card.Value.Replace(".jpg", "");
                            int charLocation = cardName.IndexOf("_", StringComparison.Ordinal);

                            cardInformation[card_suit] = cardName.Substring(0, charLocation);
                            cardInformation[card_number] = cardName.Substring(charLocation + 1);
                            cardInformation[card_picture] = card.Value;

                            if (cardInformation[card_number] == "Jack")
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
                    }
                    bestHand();
                }

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
                cardSuit.Add(card.Key, card.Value[card_suit]);
            }

            
            if (cardInfo.Count < 3)
            {
                otherHand = false;

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
            else if (cardInfo.Count > 4)
            {
                var numberList = cardNumber.ToList();
                var suitList = cardSuit.ToList();

                //Sort out the cards into decending order
                numberList.Sort((x, y) => y.Value.CompareTo(x.Value));

                fiveCardCheck(numberList, suitList);
            }
            
            if (otherHand)
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
            otherHand = true;
        }

        private void fiveCardCheck(List<KeyValuePair<string, int>> numberList, List<KeyValuePair<string, string>> suitList)
        {
            List<string> duplicates = new List<string>();
            int suitCount = 0;
            int countNumber = 0;
            int biggestNumber = 0;
            int numInOrder = 1;
            int loopCount = 0;
            int ListPosition = 0;
            
            var pairs = suitList.ToLookup(x => x.Value, x => x.Key).Where(x => x.Count() > 1);
            foreach (var item in pairs)
            {
                duplicates.Add(item.Aggregate("", (s, v) => s + v + ", "));
            }
        
            if(duplicates.Count == 1)
            {
                suitCount = duplicates[0].Length - duplicates[0].Replace(",", "").Length;
            }

            int tempNum = 1;

            foreach (var item in numberList)
            {
                if(item.Value == countNumber - 1)
                {
                    tempNum++;
                }
                else
                {
                    if (tempNum > numInOrder)
                    {
                        numInOrder = tempNum;
                    }
                    else
                    {
                        tempNum = 1;
                        biggestNumber = item.Value;
                        ListPosition = loopCount;
                    }
                    
                }

                countNumber = item.Value;
                loopCount++;
            }

            if (suitCount >= 5 || numInOrder >= 5)
            {
                if (suitCount >= 5 && !(numInOrder >= 5))
                {
                    duplicates[0] = Regex.Replace(duplicates[0], @"\s+", "");
                    List <string> cardNumbers = duplicates[0].Split(',').ToList();

                    Card1.Source = cardInfo[cardNumbers[0]][card_picture];
                    Card2.Source = cardInfo[cardNumbers[1]][card_picture];
                    Card3.Source = cardInfo[cardNumbers[2]][card_picture];
                    Card4.Source = cardInfo[cardNumbers[3]][card_picture];
                    Card5.Source = cardInfo[cardNumbers[4]][card_picture];

                    BestHand.Text = "Best Hand: Flush";
                    MessagingCenter.Send(this, "BestHand", BestHand.Text);
                }
                else if (!(suitCount >= 5) && numInOrder >= 5)
                {
                    Card1.Source = cardInfo[numberList[ListPosition].Key][card_picture];
                    Card2.Source = cardInfo[numberList[ListPosition + 1].Key][card_picture];
                    Card3.Source = cardInfo[numberList[ListPosition + 2].Key][card_picture];
                    Card4.Source = cardInfo[numberList[ListPosition + 3].Key][card_picture];
                    Card5.Source = cardInfo[numberList[ListPosition + 4].Key][card_picture];

                    BestHand.Text = "Best Hand: Straight";
                    MessagingCenter.Send(this, "BestHand", BestHand.Text);
                }
                else if (suitCount >= 5 && numInOrder >= 5 && biggestNumber != 14)
                {
                    Card1.Source = cardInfo[numberList[ListPosition].Key][card_picture];
                    Card2.Source = cardInfo[numberList[ListPosition + 1].Key][card_picture];
                    Card3.Source = cardInfo[numberList[ListPosition + 2].Key][card_picture];
                    Card4.Source = cardInfo[numberList[ListPosition + 3].Key][card_picture];
                    Card5.Source = cardInfo[numberList[ListPosition + 4].Key][card_picture];

                    BestHand.Text = "Best Hand: Straight Flush";
                    MessagingCenter.Send(this, "BestHand", BestHand.Text);
                }
                else if (suitCount >= 5 && numInOrder >= 5 && biggestNumber == 14)
                {
                    Card1.Source = cardInfo[numberList[ListPosition].Key][card_picture];
                    Card2.Source = cardInfo[numberList[ListPosition + 1].Key][card_picture];
                    Card3.Source = cardInfo[numberList[ListPosition + 2].Key][card_picture];
                    Card4.Source = cardInfo[numberList[ListPosition + 3].Key][card_picture];
                    Card5.Source = cardInfo[numberList[ListPosition + 4].Key][card_picture];

                    BestHand.Text = "Best Hand: Royal Flush";
                    MessagingCenter.Send(this, "BestHand", BestHand.Text);
                }


                otherHand = false;
            }
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
                MessagingCenter.Send(this, "BestHand", BestHand.Text);
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
                    MessagingCenter.Send(this, "BestHand", BestHand.Text);
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
                    MessagingCenter.Send(this, "BestHand", BestHand.Text);
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
                    MessagingCenter.Send(this, "BestHand", BestHand.Text);
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
                    MessagingCenter.Send(this, "BestHand", BestHand.Text);
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
            MessagingCenter.Send(this, "BestHand", BestHand.Text);
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

        private void Role_Info_Clicked(object sender, EventArgs e)
        {
            if (PlayerRole.Text == "Your Role: Dealer")
            {
                DisplayAlert("Dealer", "You have to shuffle the cards and deal two to each player one at a time, then line five cards face down on the table", "OK");
            }
            else if (BestHand.Text == "Your Role: Big Blind")
            {
                DisplayAlert("Big Blind", "You have to bet the big blind amount on the first round", "OK");
            }
            else if (BestHand.Text == "Your Role: Small Blind")
            {
                DisplayAlert("Small Blind", "You are to the left of the dealer, and therefore start off the betting. You have to bet the pre arranged small blind amount until it comes back to you, at this point you can choose to match the bet.", "OK");
            }
            else if (BestHand.Text == "Your Role: Player")
            {
                DisplayAlert("Player", "You don't have to bet anything, unless you want to", "OK");
            }
        }

        private void Tip_Clicked(object sender, EventArgs e)
        {
            if (Card3.Source.Equals("Not_Selected.jpg"))
            {
                DisplayAlert("Tip", "Match big blind bet, go as high as you you would like to gamble if you have higher cards or two of the same card", "OK");
            }
            else if (Card4.Source.Equals("Not_Selected.jpg") || Card5.Source.Equals("Not_Selected.jpg"))
            {
                DisplayAlert("Tip", "Go as high as you you would like to gamble. Bet higher if you have same suit or a sequence of numbers start to appear", "OK");
            }
            else if (BestHand.Text.Equals("Best Hand: High Card"))
            {
                DisplayAlert("Tip", "Go as high as you you would like to gamble. Bet higher if you have same suit or a sequence of numbers start to appear", "OK");
            }
            else if (BestHand.Text.Equals("Best Hand: Pair") || BestHand.Text.Equals("Best Hand: Two Pair") || BestHand.Text.Equals("Best Hand: Three of a kind"))
            {
                DisplayAlert("Tip", "A risky bet, depending on how high the bet currently is, the higher the risk it will be", "OK");
            }
            else if (BestHand.Text.Equals("Best Hand: Straight") || BestHand.Text.Equals("Best Hand: Flush"))
            {
                DisplayAlert("Tip", "A relatively safe bet as long as the bet amount does not get too high", "OK");
            }
            else if (BestHand.Text.Equals("Best Hand: Full house") || BestHand.Text.Equals("Best Hand: Four of a kind"))
            {
                DisplayAlert("Tip", "A very high chance of winning, bet anything you are comfortable with", "OK");
            }
            else if (BestHand.Text.Equals("Best Hand: Straight Flush") || BestHand.Text.Equals("Best Hand: Royal Flush"))
            {
                DisplayAlert("Tip", "Incredible hand! Bluff, bet, the choice is yours!!!", "OK");
            }
            else
            {
                DisplayAlert("Tip", "Wait for hand to be drawn", "OK");
            }
        }


        private void Voice_Help_Tip(object sender, EventArgs e)
        {
            if (Card3.Source.Equals("Not_Selected.jpg"))
            {
                DependencyService.Get<IPlaySettingsToSpeech>().Speak("Match big blind bet, go as high as you you would like to gamble if you have higher cards or two of the same card");
            }
            else if (Card4.Source.Equals("Not_Selected.jpg") || Card5.Source.Equals("Not_Selected.jpg"))
            {
                DependencyService.Get<IPlaySettingsToSpeech>().Speak("Go as high as you you would like to gamble. Bet higher if you have same suit or a sequence of numbers start to appear");
            }
            else if (BestHand.Text.Equals("Best Hand: High Card"))
            {
                DependencyService.Get<IPlaySettingsToSpeech>().Speak("Unless the bet is no higher than the big blind there is no point betting, but bet a little higher if you have 4 of same suit or 4 in a sequence of numbers");
            }
            else if (BestHand.Text.Equals("Best Hand: Pair") || BestHand.Text.Equals("Best Hand: Two Pair") || BestHand.Text.Equals("Best Hand: Three of a kind"))
            {
                DependencyService.Get<IPlaySettingsToSpeech>().Speak("A risky bet, depending on how high the bet currently is, the higher the risk it will be");
            }
            else if (BestHand.Text.Equals("Best Hand: Straight") || BestHand.Text.Equals("Best Hand: Flush"))
            {
                DependencyService.Get<IPlaySettingsToSpeech>().Speak("A relatively safe bet as long as the bet amount does not get too high");
            }
            else if (BestHand.Text.Equals("Best Hand: Full house") || BestHand.Text.Equals("Best Hand: Four of a kind"))
            {
                DependencyService.Get<IPlaySettingsToSpeech>().Speak("A very high chance of winning, bet anything you are comfortable with");
            }
            else if (BestHand.Text.Equals("Best Hand: Straight Flush") || BestHand.Text.Equals("Best Hand: Royal Flush"))
            {
                DependencyService.Get<IPlaySettingsToSpeech>().Speak("Incredible hand! Bluff, bet, the choice is yours!!!");
            }
            else
            {
                DependencyService.Get<IPlaySettingsToSpeech>().Speak("Wait for hand to be drawn");
            }
        }

        private void Voice_Help_Role(object sender, EventArgs e)
        {
            if (PlayerRole.Text == "Your Role: Dealer")
            {
                DependencyService.Get<IPlaySettingsToSpeech>().Speak("You are the dealer. You have to shuffle the cards and deal two to each player one at a time. Then line five cards face down on the table");
            }
            else if (BestHand.Text == "Your Role: Big Blind")
            {
                DependencyService.Get<IPlaySettingsToSpeech>().Speak("You are the big blind. You have to bet the big blind amount on the first round");
            }
            else if (BestHand.Text == "Your Role: Small Blind")
            {
                DependencyService.Get<IPlaySettingsToSpeech>().Speak("You are the small blind. You are to the left of the dealer, and therefore start off the betting. You have to bet the pre arranged small blind amount until it comes back to you, at this point you can choose to match the bet.");
            }
            else if (BestHand.Text == "Your Role: Player")
            {
                DependencyService.Get<IPlaySettingsToSpeech>().Speak("You are just a player. You don't have to bet anything, unless you want to");
            }
        }

        public interface IPlaySettingsToSpeech
        {
            void Speak(string text);
        }
    }
}