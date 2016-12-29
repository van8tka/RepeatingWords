using RepeatingWords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace RepeatingWords
{
    public partial class ChooseDb : ContentPage
    {
        public ChooseDb()
        {
            InitializeComponent();
        }
       //обработка перехода на страницу
        protected override void OnAppearing()
        {
            dictionaryList.ItemsSource = App.Db.GetDictionarys();
            base.OnAppearing();
        }
        //обработка нажатия по эл списка
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //получаем нажатый элемент
            AddWords adb = new AddWords((Dictionary)e.SelectedItem);           
            await Navigation.PushAsync(adb);
        }

        private async void AddDictionaryButtonClick(object sender, System.EventArgs e)
        {
            CreateDb cdb = new CreateDb();
            Dictionary dictionary = new Dictionary();
            cdb.BindingContext = dictionary;
            await Navigation.PushAsync(cdb);
        }
    }
}
