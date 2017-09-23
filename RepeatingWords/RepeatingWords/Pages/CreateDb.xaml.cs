using RepeatingWords.Model;
using System;
using Xamarin.Forms;

namespace RepeatingWords
{
    public partial class CreateDb : ContentPage
    {
        public CreateDb()
        {
            InitializeComponent();
        }


        //вызов главной страницы и чистка стека страниц
        private async void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        private async void CreateDbButtonClick(object sender, System.EventArgs e)
        {
            var dictionary = (Dictionary)BindingContext;
            if (!String.IsNullOrEmpty(dictionary.Name))
            {
                dictionary.Id = 0;
                App.Db.CreateDictionary(dictionary);
            }

            await Navigation.PopAsync();
        }
    }
}