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

        public void OnTimeChosen()
        {


        }

        public void OnRoleChosen()
        {


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
                MessagingCenter.Send(this, "BlindAmount", BlindAmount.Text);
                MessagingCenter.Send(this, "BlindTime", BlindTimeLabel.Text);
                DisplayAlert("", "Settings Saved", "OK");
            }
            
        }
    }
}