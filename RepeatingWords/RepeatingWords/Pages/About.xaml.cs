
using System;
using Xamarin.Forms;

namespace RepeatingWords.Pages
{

    public partial class About : ContentPage
    {
        public About()
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
