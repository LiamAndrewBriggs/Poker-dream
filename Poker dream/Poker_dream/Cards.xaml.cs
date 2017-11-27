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
            Card1.Source = ImageSource.FromFile("Card_7.jpg");
            Card2.Source = ImageSource.FromFile("Card_10.jpg");
            Card3.Source = ImageSource.FromFile("Ace.jpg");
            Card4.Source = ImageSource.FromFile("Card_8.jpg");
            Card5.Source = ImageSource.FromFile("Card_3.jpg");
            Card6.Source = ImageSource.FromFile("Card_9.jpg");
            Card7.Source = ImageSource.FromFile("Card_6.jpg");
        }
	}
}