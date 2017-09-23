
using System;
using Xamarin.Forms;

namespace RepeatingWords.Pages
{
    public partial class HowCreateOneWord : ContentPage
    {
        public HowCreateOneWord()
        {
            InitializeComponent();
        }

        //вызов главной страницы и чистка стека страниц
        private async void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}
