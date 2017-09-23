using RepeatingWords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.Pages
{
    public partial class DictionarysFrNet : ContentPage
    {
        //создаем класс для работы с WebApi сайта и получения данных
        OnlineDictionaryService onService = new OnlineDictionaryService();
        public DictionarysFrNet(Language lang)
        {
            InitializeComponent();
            NameLanguage.Text = lang.NameLanguage;
            actIndicator2.IsRunning = false;
            //метод загрузки списка словаерй выбранного языка по ID языка
            ListLoad(lang.Id);
        }

        //вызов главной страницы и чистка стека страниц
        private async void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
        private async Task ListLoad(int idLang)
        {
            actIndicator2.IsRunning = true;
            //получаем данные в формате Json, Диссериализуем их и получаем словари
            IEnumerable<Dictionary> dictionaryList = await onService.GetLanguage(idLang);
            actIndicator2.IsRunning = false;
            //передаем словари(список) в xaml элементу Listview
            dictionaryNetList.ItemsSource = dictionaryList.OrderBy(x => x.Name);

        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Dictionary dictionaryNet = (Dictionary)e.SelectedItem;
                int id = dictionaryNet.Id;

                //создаем этот словарь локально
                App.Db.CreateDictionary(dictionaryNet);
                actIndicator2.IsRunning = true;
                //получаем список слов выбранного словаря в интеренете
                IEnumerable<Words> wordsList = await onService.Get(id);



                //получаем последний словарь(который только что создали)
                int idLast = App.Db.GetDictionarys().LastOrDefault().Id;
                //проходим по списку слов и создаем слова для этого словаря
                await GreateWords(idLast, wordsList);

                actIndicator2.IsRunning = false;

                AddWord adw = new AddWord(dictionaryNet);
                await Navigation.PushAsync(adw);
            }
            catch (Exception er)
            {
                await DisplayAlert("Error", er.Message, "Ok");
            }
        }
        //асинхронный метод добавления слов в ыбранный словарь
        private async Task GreateWords(int idLast, IEnumerable<Words> wordsList)
        {
            foreach (var i in wordsList)
            {
                Words newW = new Words()
                {
                    Id = 0,
                    IdDictionary = idLast,
                    RusWord = i.RusWord,
                    Transcription = i.Transcription,
                    EngWord = i.EngWord
                };
                await App.WrAsync.AsyncCreateWord(newW);
            }
        }
    }
}