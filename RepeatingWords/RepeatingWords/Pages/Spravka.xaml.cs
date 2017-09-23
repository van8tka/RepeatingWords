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
            if (Device.OS == TargetPlatform.Windows || Device.OS ==TargetPlatform.WinPhone)
            {
                BtPolicy.IsVisible = true;
                BtPolicy.IsEnabled = true;
            }
            else
            {
                BtPolicy.IsVisible = false;
                BtPolicy.IsEnabled = false;
            }
        }


        //вызов главной страницы и чистка стека страниц
        private async void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
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
        //about app
        
        private async void ClickAboutButton(object sender, EventArgs e)
        {
            About about = new About();
            await Navigation.PushAsync(about);
        }






        //policy for Windows
        private void ClickPolicyButton(object sender, EventArgs e)
        {
               Device.OpenUri(new Uri("https://devprogram.ru/privacy.html"));
        }
    }
}
