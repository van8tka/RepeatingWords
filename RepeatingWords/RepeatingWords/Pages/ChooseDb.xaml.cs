using RepeatingWords.Model;
using RepeatingWords.Pages;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System;
using System.Threading.Tasks;

namespace RepeatingWords
{
    public partial class ChooseDb : ContentPage
    {
        //имя словаря для продолжения повторения слов(не должно отображаться)
        const string NameDbForContinued = "ContinueDictionary";
        const string NameDbForContinuedLearn = "ContinueDictionary-learning";
        public ChooseDb()
        {
            InitializeComponent();
            actIndicator4.IsVisible = false;
        }

        //вызов главной страницы и чистка стека страниц
        private async void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        //обработка перехода на страницу
        protected override void OnAppearing()
        {
            dictionaryList.ItemsSource = App.Db.GetDictionarys().Where(x => x.Name != NameDbForContinued && x.Name != NameDbForContinuedLearn).OrderBy(x => x.Name);
            actIndicator4.IsVisible = false;
            base.OnAppearing();
        }

        private async void AddDictionaryButtonClick(object sender, System.EventArgs e)
        {
            CreateDb cdb = new CreateDb();
            Dictionary dictionary = new Dictionary();
            cdb.BindingContext = dictionary;
            await Navigation.PushAsync(cdb);
        }

        //обр нажатия добавления словарей из интернета
        protected async void AddWordsFromNetButtonClick(object sender, System.EventArgs e)
        {
            //проверим состояние сети.. вкл или выкл
            bool isConnect = DependencyService.Get<ICheckConnect>().CheckTheNet();
            if (!isConnect)
            {
                await DisplayAlert(Resource.ModalException, Resource.ModalCheckNet, "Ok");
            }
            else
            {
                LanguageFrNet lng = new LanguageFrNet();
                await Navigation.PushAsync(lng);
            }
        }

        //обработка нажатия по эл списка
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                //получаем нажатый элемент
             
                string ButtonAddWord = Resource.ButtonAddWord;
                string ButtonAddWorFrFile = Resource.ButtonAddWorFrFile;
                string ButtonRemove = Resource.ButtonRemove;
                string ButtonShowWords = Resource.ButtonShowWords;
                string BtCancel = Resource.ModalActCancel;

                Dictionary dic = (Dictionary)e.SelectedItem;

                var action = await DisplayActionSheet(null, BtCancel, null, ButtonShowWords, ButtonAddWord, ButtonAddWorFrFile, ButtonRemove);
                if (action == ButtonAddWord)
                {
                    AddWordDictionary(dic);
                }
                if (action == ButtonAddWorFrFile)
                {
                    AddWordsFrFileToDictionary(dic);
                }
                if (action == ButtonShowWords)
                {
                    AddWord adw = new AddWord(dic);
                    await Navigation.PushAsync(adw);
                }
                if (action == ButtonRemove)
                {
                    RemoveDictionary(dic);
                }
            }
            catch { }

        }

        private async void RemoveDictionary(Dictionary dic)
        {
            try
            {
                //удаляем словарь
                App.Db.DeleteDictionary(dic.Id);
                //удаляем слова этого словаря
                App.Wr.DeleteWords(dic.Id);
                if (App.LAr.GetLastAction() != null && App.LAr.GetLastAction().IdDictionary == dic.Id)
                {
                    App.LAr.DelLastAction();
                }


                string ModalDict = Resource.ModalDict;
                string ModalRemove = Resource.ModalRemove;
                await DisplayAlert(null, ModalDict + " " + dic.Name + " " + ModalRemove, "Ок");
                OnAppearing();
            }
            catch { }
        }


        //метод добавения слова
        private async void AddWordDictionary(Dictionary dic)
        {
            try
            {
                CreateWord cr = new CreateWord(dic.Id);
                Words word1 = new Words();
                cr.BindingContext = word1;
                await Navigation.PushAsync(cr);
            }
            catch { }
        }



        //метод добавления слов из файла

        private async void AddWordsFrFileToDictionary(Dictionary dic)
        {
            try
            {
                if (Device.OS == TargetPlatform.Android)
                {
                    ChooseFile ch = new ChooseFile(dic);
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
                        await AddWordsFromFileUWP(lines, dic);
                    }
                    else
                    {
                        await DisplayAlert("Error", "", "Ok");
                    }
                }
            }
            catch (Exception er)
            {
                await DisplayAlert("Error", er.Message + "Probably app doesn't have permission to read data. Please, check settings: Settings->Applications->Application Permission->Storage and find your app. Permissions must be set.", "Ok");
            }
        }

        //метод делает неактивными элементы страницы
        private void BtnEnable(bool b)
        {
            BtAddDict.IsEnabled = b;
            BtAddWorFrNet.IsEnabled = b;
            dictionaryList.IsEnabled = b;
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
            catch (Exception er)
            {
                await DisplayAlert("Error", er.Message, "Ok");
            }
        }



    }
}