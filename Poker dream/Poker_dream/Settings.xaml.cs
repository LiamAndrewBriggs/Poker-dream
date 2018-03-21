using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Poker_dream
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
        }

         private void PlayerRole_OnTapped(object sender, EventArgs e)
        {
            PlayerRole.Focus();
        }

        private void PlayerRole_List(object sender, EventArgs e)
        {
            PlayerRoleLabel.Text = PlayerRole.Items[PlayerRole.SelectedIndex];
        }

        private void NumPlayers_OnTapped(object sender, EventArgs e)
        {
            NumPlayers.Focus();
        }

        private void NumPlayers_List(object sender, EventArgs e)
        {
            NumPlayersLabel.Text = NumPlayers.Items[NumPlayers.SelectedIndex];
        }

        private void BlindTime_OnTapped(object sender, EventArgs e)
        { 
            BlindTime.Focus();
        }

        private void BlindTime_List(object sender, EventArgs e)
        {
            BlindTimeLabel.Text = BlindTime.Items[BlindTime.SelectedIndex];
        }
        
        private void Save_Button(object sender, EventArgs e)
        {
            if (BlindAmount.Text == null)
            {
                DisplayAlert("Error", "Please Input All Settings", "OK");
            }
            else
            {
                MessagingCenter.Send(this, "PlayersRole", PlayerRoleLabel.Text);
                MessagingCenter.Send(this, "PlayerNumber", NumPlayersLabel.Text);
                MessagingCenter.Send(this, "BlindAmount", BlindAmount.Text);
                MessagingCenter.Send(this, "BlindTime", BlindTimeLabel.Text);
                DisplayAlert("", "Settings Saved", "OK");
            }
            
        }

        private void Setup_Button(object sender, EventArgs e)
        {
            DisplayAlert("Help", "The roles start with the dealer \n\nTo their left is the small blind \n\nTo their left is the big blind \n\nThen player 1 and so on \n\nBlind times and amount are decided amongst players," +
                "the big blind is the minimum you can bet per round \n\nBlind time is a countdown until this amount increases", "OK");
        }

        private void Setup_Speech_Button(object sender, EventArgs e)
        {
            DependencyService.Get<ITextSettingsToSpeech>().Speak("The roles start with the dealer. To their left is the small blind. To their left is the big blind. Then player 1 and so on. Blind times and amount are decided amongst players." +
                "the big blind is the minimum you can bet per round. Blind time is a countdown until this amount increases");
        }

        public interface ITextSettingsToSpeech
        {
            void Speak(string text);
        }
    }
}