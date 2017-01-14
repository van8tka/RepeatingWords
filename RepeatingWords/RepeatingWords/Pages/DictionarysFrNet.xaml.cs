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
            actIndicator2.IsRunning = false;
            ListLoad();
        }

              
        async Task ListLoad()
        {
            actIndicator2.IsRunning = true;
            //получаем данные в формате Json, Диссериализуем их и получаем словари
            IEnumerable<Dictionary> dictionaryList = await onService.Get();
            actIndicator2.IsRunning = false;
            //передаем словари(список) в xaml элементу Listview
            dictionaryNetList.ItemsSource = dictionaryList;
           
        }
        
       private async void OnItemSelected(object sender,SelectedItemChangedEventArgs e)
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
                            
                //проходим по списку слов и создаем слова для этого словаря
                  actIndicator2.IsRunning = false;

                //получаем последний словарь(который только что создали)
                int idLast = App.Db.GetDictionarys().LastOrDefault().Id;
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
                    App.Wr.CreateWord(newW);
                }

                AddWord adw = new AddWord(dictionaryNet);
                await Navigation.PushAsync(adw);
            }
            catch (Exception er)
            {
                DisplayAlert("Error", er.Message, "Ok");
            }
        }
             
     
    }
}
