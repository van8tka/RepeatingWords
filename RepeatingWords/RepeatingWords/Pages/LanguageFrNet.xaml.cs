using RepeatingWords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.Pages
{
    public partial class LanguageFrNet : ContentPage
    {
        //создаем класс для работы с WebApi сайта и получения данных
        OnlineDictionaryService onService = new OnlineDictionaryService();
        public LanguageFrNet()
        {
            InitializeComponent();
            actIndicator2.IsRunning = false;
            ListLoad();
        }


        async Task ListLoad()
        {
            actIndicator2.IsRunning = true;
            //получаем данные в формате Json, Диссериализуем их и получаем языки
            IEnumerable<Language> langList = await onService.GetLanguage();
            actIndicator2.IsRunning = false;
            //передаем языки(список) в xaml элементу Listview
            languageNetList.ItemsSource = langList.OrderBy(x => x.NameLanguage);

        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Language lng = (Language)e.SelectedItem;
                DictionarysFrNet dfn = new DictionarysFrNet(lng);
                await Navigation.PushAsync(dfn);        
            }
            catch (Exception er)
            {
               await DisplayAlert("Error", er.Message, "Ok");
            }
        }


    }
}
