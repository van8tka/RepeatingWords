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
        public DictionarysFrNet()
        {
            InitializeComponent();
            ListLoad();
        }
        async Task<bool> ListLoad()
        {          
            //получаем данные в формате Json, Диссериализуем их и получаем словари
            IEnumerable<Dictionary> dictionaryList = await onService.Get();
            //передаем словари(список) в xaml элементу Listview
            dictionaryNetList.ItemsSource = dictionaryList;
            return true;
        }
        
       private async void OnItemSelected(object sender,SelectedItemChangedEventArgs e)
        {
            try
            {//получаем список слов выбранного словаря в интеренете
                IEnumerable<Words> wordsList = await onService.Get(((Dictionary)e.SelectedItem).Id);
                //создаем этот словарь локально
                App.Db.CreateDictionary((Dictionary)e.SelectedItem);
                //получаем последний словарь
                int idLast = App.Db.GetDictionarys().LastOrDefault().Id;
                //проходим по списку слов и создаем слова для этого словаря
                foreach(var i in wordsList)
                {
                    Words newW = new Words()
                    {
                        Id = 0,
                        IdDictionary = idLast,
                        RusWord = i.RusWord,
                        Transcription = i.Transcription,
                        EngWord = i.EngWord
                    };
                    App.Wr.CreateWord(newW);
                }
                AddWord adw = new AddWord((Dictionary)e.SelectedItem);
                await Navigation.PushAsync(adw);
            }
            catch (Exception er)
            {
                DisplayAlert("Error", er.Message, "Ok");
            }
        }
    }
}
