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
	public partial class Cards : ContentPage
	{
        public Cards ()
		{
            InitializeComponent ();
        }

        void Card1_Tapped(object sender, EventArgs args)
        {
            DisplayAlert("Test", "Hello World", "OK");
        }

        void Card2_Tapped(object sender, EventArgs args)
        {
            DisplayAlert("Test", "Hello World", "OK");
        }

        void Card3_Tapped(object sender, EventArgs args)
        {
            DisplayAlert("Test", "Hello World", "OK");
        }

        void Card4_Tapped(object sender, EventArgs args)
        {
            DisplayAlert("Test", "Hello World", "OK");
        }

        void Card5_Tapped(object sender, EventArgs args)
        {
            DisplayAlert("Test", "Hello World", "OK");
        }

        void Card6_Tapped(object sender, EventArgs args)
        {
            DisplayAlert("Test", "Hello World", "OK");
        }

        void Card7_Tapped(object sender, EventArgs args)
        {
            DisplayAlert("Test", "Hello World", "OK");
        }
    }
}