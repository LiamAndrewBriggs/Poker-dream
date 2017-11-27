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
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
            MyLocalImage.Source = ImageSource.FromFile("Pair_of_Aces.jpg");
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Test", "Hello World", "OK");
        }
    }
}