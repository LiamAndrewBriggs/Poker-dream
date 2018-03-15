using Plugin.Media;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Poker_dream
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cards : ContentPage
    {
        private Dictionary<string, string> cardToPicture = new Dictionary<string, string>
        {
            { "2 of Hearts", "Heart_2.jpg" }, { "3 of Hearts", "Heart_3.jpg" },
            { "4 of Hearts", "Heart_4.jpg" }, { "5 of Hearts", "Heart_5.jpg" },
            { "6 of Hearts", "Heart_6.jpg" }, { "7 of Hearts", "Heart_7.jpg" },
            { "8 of Hearts", "Heart_8.jpg" }, { "9 of Hearts", "Heart_9.jpg" },
            { "10 of Hearts", "Heart_10.jpg" }, { "Jack of Hearts", "Heart_Jack.jpg" },
            { "Queen of Hearts", "Heart_Queen.jpg" }, { "King of Hearts", "Heart_King.jpg" },
            { "Ace of Hearts", "Heart_Ace.jpg" },
            { "2 of Diamonds", "Diamond_2.jpg" }, { "3 of Diamonds", "Diamond_3.jpg" },
            { "4 of Diamonds", "Diamond_4.jpg" }, { "5 of Diamonds", "Diamond_5.jpg" },
            { "6 of Diamonds", "Diamond_6.jpg" }, { "7 of Diamonds", "Diamond_7.jpg" },
            { "8 of Diamonds", "Diamond_8.jpg" }, { "9 of Diamonds", "Diamond_9.jpg" },
            { "10 of Diamonds", "Diamond_10.jpg" }, { "Jack of Diamonds", "Diamond_Jack.jpg" },
            { "Queen of Diamonds", "Diamond_Queen.jpg" }, { "King of Diamonds", "Diamond_King.jpg" },
            { "Ace of Diamonds", "Diamond_Ace.jpg" },
            { "2 of Clubs", "Club_2.jpg" }, { "3 of Clubs", "Club_3.jpg" },
            { "4 of Clubs", "Club_4.jpg" }, { "5 of Clubs", "Club_5.jpg" },
            { "6 of Clubs", "Club_6.jpg" }, { "7 of Clubs", "Club_7.jpg" },
            { "8 of Clubs", "Club_8.jpg" }, { "9 of Clubs", "Club_9.jpg" },
            { "10 of Clubs", "Club_10.jpg" }, { "Jack of Clubs", "Club_Jack.jpg" },
            { "Queen of Clubs", "Club_Queen.jpg" }, { "King of Clubs", "Club_King.jpg" },
            { "Ace of Clubs", "Club_Ace.jpg" },
            { "2 of Spades", "Spade_2.jpg" }, { "3 of Spades", "Spade_3.jpg" },
            { "4 of Spades", "Spade_4.jpg" }, { "5 of Spades", "Spade_5.jpg" },
            { "6 of Spades", "Spade_6.jpg" }, { "7 of Spades", "Spade_7.jpg" },
            { "8 of Spades", "Spade_8.jpg" }, { "9 of Spades", "Spade_9.jpg" },
            { "10 of Spades", "Spade_10.jpg" }, { "Jack of Spades", "Spade_Jack.jpg" },
            { "Queen of Spades", "Spade_Queen.jpg" }, { "King of Spades", "Spade_King.jpg" },
            { "Ace of Spades", "Spade_Ace.jpg" },
        };

        private Dictionary<string, string> selectedCards = new Dictionary<string, string>();

        private string selectedCard = "";

        public Cards()
        {
            InitializeComponent();

            foreach (var item in cardToPicture)
            {
                Card_List.Items.Add(item.Key);
            }

            MessagingCenter.Subscribe<Play, string>(this, "BestHand", (sender, e) => { bestHand.Text = e; });
        }

        public void New_Game()
        {
            selectedCards = new Dictionary<string, string>();
            Card1.Source = "Not_Selected.jpg";
            Card2.Source = "Not_Selected.jpg";
            Card3.Source = "Not_Selected.jpg";
            Card4.Source = "Not_Selected.jpg";
            Card5.Source = "Not_Selected.jpg";
            Card6.Source = "Not_Selected.jpg";
            Card7.Source = "Not_Selected.jpg";

            MessagingCenter.Send(this, "Cards_Pulled", selectedCards);
        }

        void Card_Changed(object sender, EventArgs args)
        {
            switch (selectedCard)
            {
                case "Card_1":
                    Card1.Source = cardToPicture[Card_List.Items[Card_List.SelectedIndex]];
                    if (selectedCards.ContainsKey("Card_1"))
                    {
                        selectedCards["Card_1"] = cardToPicture[Card_List.Items[Card_List.SelectedIndex]];
                    }
                    else
                    {
                        selectedCards.Add("Card_1", cardToPicture[Card_List.Items[Card_List.SelectedIndex]]);
                    }
                    break;
                case "Card_2":
                    Card2.Source = cardToPicture[Card_List.Items[Card_List.SelectedIndex]];
                    if (selectedCards.ContainsKey("Card_2"))
                    {
                        selectedCards["Card_2"] = cardToPicture[Card_List.Items[Card_List.SelectedIndex]];
                    }
                    else
                    {
                        selectedCards.Add("Card_2", cardToPicture[Card_List.Items[Card_List.SelectedIndex]]);
                    }
                    break;
                case "Card_3":
                    Card3.Source = cardToPicture[Card_List.Items[Card_List.SelectedIndex]];
                    if (selectedCards.ContainsKey("Card_3"))
                    {
                        selectedCards["Card_3"] = cardToPicture[Card_List.Items[Card_List.SelectedIndex]];
                    }
                    else
                    {
                        selectedCards.Add("Card_3", cardToPicture[Card_List.Items[Card_List.SelectedIndex]]);
                    }
                    break;
                case "Card_4":
                    Card4.Source = cardToPicture[Card_List.Items[Card_List.SelectedIndex]];
                    if (selectedCards.ContainsKey("Card_4"))
                    {
                        selectedCards["Card_4"] = cardToPicture[Card_List.Items[Card_List.SelectedIndex]];
                    }
                    else
                    {
                        selectedCards.Add("Card_4", cardToPicture[Card_List.Items[Card_List.SelectedIndex]]);
                    }
                    break;
                case "Card_5":
                    Card5.Source = cardToPicture[Card_List.Items[Card_List.SelectedIndex]];
                    if (selectedCards.ContainsKey("Card_5"))
                    {
                        selectedCards["Card_5"] = cardToPicture[Card_List.Items[Card_List.SelectedIndex]];
                    }
                    else
                    {
                        selectedCards.Add("Card_5", cardToPicture[Card_List.Items[Card_List.SelectedIndex]]);
                    }
                    break;
                case "Card_6":
                    Card6.Source = cardToPicture[Card_List.Items[Card_List.SelectedIndex]];
                    if (selectedCards.ContainsKey("Card_6"))
                    {
                        selectedCards["Card_6"] = cardToPicture[Card_List.Items[Card_List.SelectedIndex]];
                    }
                    else
                    {
                        selectedCards.Add("Card_6", cardToPicture[Card_List.Items[Card_List.SelectedIndex]]);
                    }
                    break;
                case "Card_7":
                    Card7.Source = cardToPicture[Card_List.Items[Card_List.SelectedIndex]];
                    if (selectedCards.ContainsKey("Card_7"))
                    {
                        selectedCards["Card_7"] = cardToPicture[Card_List.Items[Card_List.SelectedIndex]];
                    }
                    else
                    {
                        selectedCards.Add("Card_7", cardToPicture[Card_List.Items[Card_List.SelectedIndex]]);
                    }
                    break;
            }

            if (selectedCards.ContainsKey("Card_1") && selectedCards.ContainsKey("Card_2"))
            {
                MessagingCenter.Send(this, "Cards_Pulled", selectedCards);
            }

        }

        void Card1_Tapped(object sender, EventArgs args)
        {
            Card_List.Focus();
            selectedCard = "Card_1";
        }

        void Card2_Tapped(object sender, EventArgs args)
        {
            Card_List.Focus();
            selectedCard = "Card_2";
        }

        void Card3_Tapped(object sender, EventArgs args)
        {
            Card_List.Focus();
            selectedCard = "Card_3";
        }

        void Card4_Tapped(object sender, EventArgs args)
        {
            Card_List.Focus();
            selectedCard = "Card_4";
        }

        void Card5_Tapped(object sender, EventArgs args)
        {
            Card_List.Focus();
            selectedCard = "Card_5";
        }

        void Card6_Tapped(object sender, EventArgs args)
        {
            Card_List.Focus();
            selectedCard = "Card_6";
        }

        void Card7_Tapped(object sender, EventArgs args)
        {
            Card_List.Focus();
            selectedCard = "Card_7";
        }

        private void More_Info_Clicked(object sender, EventArgs e)
        {
            if (bestHand.Text == "Best Hand: N/A")
            {
                DisplayAlert("Hand Help", "No Cards Selected! Please select the cards you are dealt", "OK");
            }
            else if (bestHand.Text == "Best Hand: High Card")
            {
                DisplayAlert("High Card", "You havent made any hands unfortunately so a high card is played, this is the 10th and the worst hand you could have", "OK");
            }
            else if (bestHand.Text == "Best Hand: Pair")
            {
                DisplayAlert("Pair", "You have two cards of the same number value, this is the 9th best hand to have", "OK");
            }
            else if (bestHand.Text == "Best Hand: Two Pair")
            {
                DisplayAlert("Two Pair", "You have two different pairs of cards of the same number value, this is the 8th best hand to have", "OK");
            }
            else if (bestHand.Text == "Best Hand: Three of a kind")
            {
                DisplayAlert("Three of a kind", "You have three cards of the same number value, this is the 7th best hand to have", "OK");
            }
            else if (bestHand.Text == "Best Hand: Straight")
            {
                DisplayAlert("Straight", "You have five cards in a sequence, but not of the same suit, this is the 6th best hand to have", "OK");
            }
            else if (bestHand.Text == "Best Hand: Flush")
            {
                DisplayAlert("Flush", "You have any five cards of the same suit, but not in a sequence, this is the 5th best hand to have", "OK");
            }
            else if (bestHand.Text == "Best Hand: Full house")
            {
                DisplayAlert("Full house", "Three of a kind (three cards of the same number value) with a pair (two cards of the same number value), this is the 4th best hand to have", "OK");
            }
            else if (bestHand.Text == "Best Hand: Four of a kind")
            {
                DisplayAlert("Four of a kind", "You have four cards of the same number value, this is the 3rd best hand to have", "OK");
            }
            else if (bestHand.Text == "Best Hand: Straight Flush")
            {
                DisplayAlert("Straight Flush", "Five cards in a sequence, all in the same suit, this is the 2nd best hand to have", "OK");
            }
            else if (bestHand.Text == "Best Hand: Royal flush")
            {
                DisplayAlert("Royal flush", "A, K, Q, J, 10, all the same suit. , this is the  best hand you can have!!!", "OK");
            }
        }

        private void Voice_Help(object sender, EventArgs e)
        {
            if (bestHand.Text == "Best Hand: N/A")
            {
                DependencyService.Get<ITextToSpeech>().Speak("You have No Cards Selected! Please select the cards you are dealt");
            }
            else if (bestHand.Text == "Best Hand: High Card")
            {
                DependencyService.Get<ITextToSpeech>().Speak("You have a High Card. You havent made any hands unfortunately so a high card is played, this is the 10th and the worst hand you could have");
            }
            else if (bestHand.Text == "Best Hand: Pair")
            {
                DependencyService.Get<ITextToSpeech>().Speak("You have a Pair. You have two cards of the same number value, this is the 9th best hand to have");
            }
            else if (bestHand.Text == "Best Hand: Two Pair")
            {
                DependencyService.Get<ITextToSpeech>().Speak("You have a Two Pair. You have two different pairs of cards of the same number value, this is the 8th best hand to have");
            }
            else if (bestHand.Text == "Best Hand: Three of a kind")
            {
                DependencyService.Get<ITextToSpeech>().Speak("You have a Three of a kind. You have three cards of the same number value, this is the 7th best hand to have");
            }
            else if (bestHand.Text == "Best Hand: Straight")
            {
                DependencyService.Get<ITextToSpeech>().Speak("You have a Straight. You have five cards in a sequence, but not of the same suit, this is the 6th best hand to have");
            }
            else if (bestHand.Text == "Best Hand: Flush")
            {
                DependencyService.Get<ITextToSpeech>().Speak("You have a Flush. You have any five cards of the same suit, but not in a sequence, this is the 5th best hand to have");
            }
            else if (bestHand.Text == "Best Hand: Full house")
            {
                DependencyService.Get<ITextToSpeech>().Speak("You have a Full. house Three of a kind (three cards of the same number value) with a pair (two cards of the same number value), this is the 4th best hand to have");
            }
            else if (bestHand.Text == "Best Hand: Four of a kind")
            {
                DependencyService.Get<ITextToSpeech>().Speak("You have a Four. of a kind You have four cards of the same value, this is the 3rd best hand to have");
            }
            else if (bestHand.Text == "Best Hand: Straight Flush")
            {
                DependencyService.Get<ITextToSpeech>().Speak("You have a Straight Flush. Five cards in a sequence, all in the same suit, this is the 2nd best hand to have");
            }
            else if (bestHand.Text == "Best Hand: Royal flush")
            {
                DependencyService.Get<ITextToSpeech>().Speak("You have a Royal flush. Ace, King, Queen, Jack, 10, all the same suit, this is the  best hand you can have!!!");
            }
            
        }

        private async void Get_Card(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                SaveToAlbum = false
            });

            if (file == null)
                return;

            Image image = new Image(); 
            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            Card7.Source = image.Source;
        }

        public interface ITextToSpeech
        {
            void Speak(string text);
        }
    }
}