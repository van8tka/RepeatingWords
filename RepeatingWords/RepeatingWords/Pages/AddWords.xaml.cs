using RepeatingWords.Model;
using RepeatingWords.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace RepeatingWords
{
    public partial class AddWords : ContentPage
    {
        int ws;
        string d;
        public AddWords(Dictionary dictionary)
        {
         
            InitializeComponent();
             actIndicator4.IsVisible = false;
            ws = App.Wr.GetWords(dictionary.Id).Count();
            //для отображения имени словаря в заголовке
            d = dictionary.Name;
            DictionaryName.Text = dictionary.Name + " (" + ws.ToString()+")";
            this.BindingContext = dictionary;          
        }


        protected override void OnAppearing()
        {
            actIndicator4.IsRunning = false;
            DictionaryName.Text = d + " (" + ws.ToString() + ")";
            base.OnAppearing();
        }

       


        protected async void AddWordButtonClick(object sender, System.EventArgs e)
        {
            var dictionary = (Dictionary)BindingContext;
          
            CreateWord cr = new CreateWord(dictionary.Id);
            Words word1 = new Words();
            cr.BindingContext = word1;
            await Navigation.PushAsync(cr);
          

        }
       protected async void DelleteDbButtonClick(object sender, System.EventArgs e)
        { 
            var dictionary = (Dictionary)BindingContext;
            //удаляем словарь
            App.Db.DeleteDictionary(dictionary.Id);
            //удаляем слова этого словаря
            App.Wr.DeleteWords(dictionary.Id);
            if(App.LAr.GetLastAction()!=null&& App.LAr.GetLastAction().IdDictionary==dictionary.Id)
            {
                App.LAr.DelLastAction();
            }

          
            string ModalDict = Resource.ModalDict;
            string ModalRemove = Resource.ModalRemove;
            await DisplayAlert(null, ModalDict+" "+dictionary.Name+" "+ModalRemove, "Ок");

            await Navigation.PopAsync();
        }


        //обр нажатия добавления слова из файла
        protected async void AddWordsFromFileButtonClick(object sender, System.EventArgs e)
        {
            try
            {
                var dictionary = (Dictionary)BindingContext;
                if (Device.OS == TargetPlatform.Android)
                {                 
                    ChooseFile ch = new ChooseFile(dictionary);
                    await Navigation.PushAsync(ch);
                }
                if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
                {
                    //должны вызвать диалоговое окно file picker для windows и вернуть 
                    List<string> lines = await DependencyService.Get<IWindOpenFilePicker>().LoadTextFrWindowsAsync();
                    if (lines != null)
                    {
                        actIndicator4.IsVisible = true;
                        BtnEnable(false);
                       //метод добавления к БД строк из файла
                        await AddWordsFromFileUWP(lines, dictionary);
                       
                    }
                    else
                    {
                        await DisplayAlert("Error", "", "Ok");
                    }
                }
            }
           catch(Exception er)
            {
                await DisplayAlert("Error", er.Message, "Ok");
            }
        }

        private void BtnEnable(bool b)
        {
            BtAddWord.IsEnabled=b;
            BtAddWordFrFile.IsEnabled=b;
            BtRemove.IsEnabled=b;
            BtShowWords.IsEnabled=b;
        }






        //метод добавления слов из файла для UWP
        private async Task AddWordsFromFileUWP(List<string> lines, Dictionary dictionary)
        {
            try
            {
               //проходим по списку строк считанных из файла

                char[] delim = { '[', ']' };
                //переменная для проверки добавления слов
                bool CreateWordsFromFile = false;
                //проход по списку слов
                foreach (var i in lines)
                {//проверка на наличие разделителей, т.е. транскрипции в строке(символы транскрипции и есть разделители)
                    if (i.Contains("[") && i.Contains("]"))
                    {
                        CreateWordsFromFile = true;
                        string[] fileWords = i.Split(delim);
                        Words item = new Words
                        {
                            Id = 0,
                            IdDictionary = dictionary.Id,
                            RusWord = fileWords[0],
                            Transcription = "[" + fileWords[1] + "]",
                            EngWord = fileWords[2]
                        };//добавим слово в БД
                        await App.WrAsync.AsyncCreateWord(item);
                    }
                }


                string ModalAddWords = Resource.ModalAddWords;
                string ModalException = Resource.ModalException;
                string ModalIncorrectFile = Resource.ModalIncorrectFile;

                if (CreateWordsFromFile)
                {
                    BtnEnable(true);
                    actIndicator4.IsVisible = false;
                    await DisplayAlert("", ModalAddWords, "Ok");
                    AddWord adw = new AddWord(dictionary);
                    await Navigation.PushAsync(adw);
                }
                else
                {
                    BtnEnable(true);
                    actIndicator4.IsVisible = false;
                    await DisplayAlert(ModalException, ModalIncorrectFile, "Ok");
                }
            }
            catch(Exception er)
            {
                await DisplayAlert("Error", er.Message, "Ok");
            }
        }






        protected async void ShowWordsDbButtonClick(object sender,System.EventArgs e)
        {
            var dictionary = (Dictionary)BindingContext;
            AddWord adw = new AddWord(dictionary);
            await Navigation.PushAsync(adw);
        }

     

    }
}
