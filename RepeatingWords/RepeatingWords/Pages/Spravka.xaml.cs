using RepeatingWords.Pages;
using System;
using Xamarin.Forms;

namespace RepeatingWords
{
    public partial class Spravka : ContentPage
    {
        public Spravka()
        {
            InitializeComponent();
        }
        private async void CreateOneWordButtonClick(object sender, EventArgs e)
        {
            HowCreateOneWord hc = new HowCreateOneWord();
            await Navigation.PushAsync(hc);
        }
        private async void CreateFromFileButtonClick(object sender, EventArgs e)
        {
            HowCreateFromFile hf = new HowCreateFromFile();
            await Navigation.PushAsync(hf);
        }
    }
}
