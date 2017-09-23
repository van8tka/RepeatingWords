using RepeatingWords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.Pages
{
    public partial class RepeatWord : ContentPage
    {
        //для определения языка
        bool FromRus;
        //получения ID словаря
        int iDdictionary;
        //для определения поворота карточки
        bool Turn;
        //для списка слов
        IEnumerable<Words> words;
        //номер слова в списке IEnumerable<words>
        int Count = 0;
        List<Words> TurnedWords = new List<Words>();
        int countW = 1;
        //язык озвучки
        string lang;
        //кол-во перевернутых(невыученных слов)
        int countTurned = 0;
        string TextTurned = Resource.LabelCountTurned;
        const string NameDbForContinued = "ContinueDictionary";


        public RepeatWord(int iDdictionary, bool FromRus)
        {
            InitializeComponent();
            this.FromRus = FromRus;
            //переменная для многократного переворота карточки
            Turn = FromRus;
            this.iDdictionary = iDdictionary;
            //получаем карточки и сортируем их рандомно

            words = App.Wr.GetWords(iDdictionary);
            RandomWordList();
            //кол во слов и пройденных слов
            LabelCountOfWords.Text = words.Count().ToString() + "/" + (countW + Count).ToString() + "/" + countTurned.ToString();
            //LabelCountOfWordsTurn.Text = TextTurned+" "+ countTurned.ToString();
            // код для Android
            if (Device.OS == TargetPlatform.Android)
            {
                lang = "en_GB";
                DependencyService.Get<IAdmobInterstitial>().Show("ca-app-pub-5351987413735598/1185308269");
            }
            else
                 if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
            { lang = "en-GB"; }

            UpdateWord(Count, FromRus);

        }





        //конструктор при продолжении изучения словаря
        public RepeatWord(LastAction la)
        {
            InitializeComponent();
            FromRus = la.FromRus;
            //переменная для многократного переворота карточки
            Turn = FromRus;
            iDdictionary = la.IdDictionary;
            words = App.Wr.GetWords(la.IdDictionary);
            //определяем чему равен Count;
            Count = la.IdWord;
            //сколько слов всего и пройдено
            LabelCountOfWords.Text = words.Count().ToString() + "/" + (countW + Count).ToString() + "/" + countTurned.ToString();
            if (Device.OS == TargetPlatform.Android)
            {
                lang = "en_GB";
                DependencyService.Get<IAdmobInterstitial>().Show("ca-app-pub-5351987413735598/1185308269");
            }
            else if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
            { lang = "en-GB"; }
            UpdateWord(Count, FromRus);
        }

        //вызов главной страницы и чистка стека страниц
        private async void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            SaveLastWorld();
            //выход на главную страницу
            ComeBack();
        }



        //метод смешивания слов перед выводом
        private void RandomWordList()
        {
            List<Words> tempWords = (List<Words>)words;
            Random rng = new Random();
            int n = words.Count();
            //пока не первое слово
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Words value = tempWords[k];
                tempWords[k] = tempWords[n];
                tempWords[n] = value;
            }
            words = null;
            words = (IEnumerable<Words>)tempWords;
        }

        //метод для обновления слова
        private void UpdateWord(int count, bool lang)
        {
            if (lang)
            {
                WordForRepeat.TextColor = (Color)Application.Current.Resources["ColorWB"];
                WordForRepeat.Text = GetWords(count).RusWord;
                //управление видимостью озвучки               
                ButtonVoice.IsEnabled = false;
                picker.IsVisible = true;
                ButtonVoice.Image = "voiceX.png";

            }
            else
            {
                WordForRepeat.TextColor = (Color)Application.Current.Resources["ColorBlGr"];
                Words w = GetWords(count);
                //если транскрипции нет 
                if (w.Transcription == "[]")
                {//выводим только перевод
                    WordForRepeat.Text = w.EngWord;
                }
                else
                {//перевод и транскрипцию
                    WordForRepeat.Text = w.EngWord + "\n" + w.Transcription;
                }

                //управление видимостью озвучки

                ButtonVoice.IsEnabled = true;
                ButtonVoice.Image = "voice.png";
                picker.IsVisible = true;

            }

        }




        //для получения слова
        Words GetWords(int index)
        {
            Words item = words.ElementAt(index);
            return item;
        }





        private async void NextWordButtonClick(object sender, EventArgs e)
        {//для сброса количества перево 
            try
            {
                Turn = FromRus;

                int z = words.Count() - 1;
                if (z > Count)
                {
                    Count++;
                    UpdateWord(Count, FromRus);
                }
                else
                {
                    string ModalAllWordsComplete = Resource.ModalAllWordsComplete;
                    string ModalFinish = Resource.ModalFinish;
                    string ModalRestart = Resource.ModalRestart;
                    string ModalLerningTurnedWords = Resource.ModalLerningTurnedWords;


                    var action = await DisplayActionSheet(ModalAllWordsComplete, "", "", ModalFinish, ModalRestart, ModalLerningTurnedWords);

                    if (action == ModalFinish)
                    {
                        //вызовем метод для проверки есть ли в списке перевернутые слова
                        //если есть то создадим БД и добавим в нее слова
                        await CreateTurnedDB();
                        //перреход на главную страницу
                        await Navigation.PopToRootAsync();
                    }
                    //если начать заново словарь 
                    if (action == ModalRestart)
                    {
                        //то список перевернутых слов обновляем
                        TurnedWords.Clear();
                        //и количество перевернутых слов обнуляем
                        countTurned = 0;
                        //индекс слова обновляем
                        Count = 0;
                        //метод смешивания слов
                        RandomWordList();
                        //вызываем метод обновления слова
                        UpdateWord(Count, FromRus);
                    }
                    //если выбрали повторение невыученных в этом словаре     
                    if (action == ModalLerningTurnedWords)
                    {
                        //вызовем метод для проверки есть ли в списке перевернутые слова
                        //если есть то создадим БД и добавим в нее слова
                        await CreateTurnedDB();
                        //обнулим индекс слов
                        Count = 0;
                        //обнулим кол-во перевернутых слов
                        countTurned = 0;
                        LabelCountOfWords.Text = words.Count().ToString() + "/" + (countW + Count).ToString() + "/" + countTurned.ToString();
                        //LabelCountOfWordsTurn.Text = TextTurned + " " + countTurned.ToString();
                        //получим последнюю БД и очистим список перевернутых слов
                        TurnedWords.Clear();
                        Dictionary di = App.Db.GetDictionarys().LastOrDefault();
                        iDdictionary = di.Id;//получим новый список слов из БД
                        words = App.Wr.GetWords(di.Id);//обновим экран
                        UpdateWord(Count, FromRus);
                    }

                }
                //сколько слов всего и пройдено сколько
                LabelCountOfWords.Text = words.Count().ToString() + "/" + (countW + Count).ToString() + "/" + countTurned.ToString();
            }
            catch (Exception er)
            {
                await DisplayAlert(Resource.ModalException, er.Message.ToString(), "Ok");
            }
        }




        //===============================метод создания словаря перевернутых слов(невыученных)=================
        private async Task CreateTurnedDB()
        {
            try
            {
                if (TurnedWords.Any())
                {
                    string NameD = App.Db.GetDictionary(iDdictionary).Name;
                    string NameDictionary;
                    //проверим это словарь который уже изучали или нет(содержит приставку lerning)
                    if (NameD.Contains("-learning"))
                    {
                        NameDictionary = NameD;
                    }
                    else
                    {
                        NameDictionary = App.Db.GetDictionary(iDdictionary).Name + "-learning";
                    }
                    Dictionary deldict = App.Db.GetDictionarys().Where(x => x.Name == NameDictionary).FirstOrDefault();
                    if (deldict != null)
                    {
                        App.Wr.DeleteWords(deldict.Id);
                        App.Db.DeleteDictionary(deldict.Id);

                    }

                    Dictionary dict = new Dictionary()
                    {
                        Id = 0,
                        Name = NameDictionary
                    };
                    App.Db.CreateDictionary(dict);
                    int IdLastDictionary = App.Db.GetDictionarys().LastOrDefault().Id;
                    foreach (var i in TurnedWords)
                    {
                        i.Id = 0;
                        i.IdDictionary = IdLastDictionary;
                        App.Wr.CreateWord(i);
                    }
                }
            }
            catch (Exception er)
            {
                await DisplayAlert(Resource.ModalException, er.Message.ToString(), "Ok");
            }
        }




        //переопределение метода обработки события при нажатии кнопки НАЗАД, 
        //для его срабатывания надо в MainActivity тоже переопределить метод OnBackPressed
        protected override bool OnBackButtonPressed()
        {//метод добавления словаря для возможности продолжения с последнего места
            SaveLastWorld();
            //выход на главную страницу
            ComeBack();
            return true;
        }

        private void SaveLastWorld()
        {
            //проверим наличие словаря для продолжения изучения слов
            if (!App.Db.GetDictionarys().Where(x => x.Name == NameDbForContinued).Any())
            {//если нету то создадим
                App.Db.CreateDictionary(new Dictionary() { Id = 0, Name = NameDbForContinued });
            }
            //обновим слова(или добавим)
            int IdContinueDict = App.Db.GetDictionarys().Where(x => x.Name == NameDbForContinued).FirstOrDefault().Id;
            App.Wr.DeleteWords(IdContinueDict);
            foreach (Words w in words)
            {
                w.Id = 0;
                w.IdDictionary = IdContinueDict;
                App.Wr.CreateWord(w);
            }
            //при нажатии  на кн выхода сохраняется последнее слово
            LastAction i = new LastAction()
            {
                Id = 0,
                IdDictionary = IdContinueDict,
                FromRus = FromRus,
                IdWord = Count
            };
            App.LAr.SaveLastAction(i);
        }


        //метод возврата на главную страницу
        private void ComeBack()
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }








        int idTurn = -1;
        private void TurnAroundWordButtonClick(object sender, EventArgs e)
        {
            if (Count != idTurn)
            {
                countTurned++;
                idTurn = Count;
                //добавим перевернутое слово в список для дальнейшего добавления в БД
                TurnedWords.Add(GetWords(Count));
            }
            UpdateWord(Count, !Turn);
            Turn = !Turn;//перевернули карточку         
        }


        private void BtnClickSpeech(object sender, EventArgs e)
        {
            DependencyService.Get<ITextToSpeech>().Speak(GetWords(Count).EngWord, lang);
        }





        #region ChoseLanguage
        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (picker.Items[picker.SelectedIndex].ToString())
            {
                case "English":
                    {

                        if (Device.OS == TargetPlatform.Android)
                        {
                            lang = "en_GB";
                        }
                        else if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                        {
                            lang = "en-GB";
                        }

                        break;
                    }
                case "French":
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            lang = "fr_FR";
                        }
                        else if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                        {
                            lang = "fr-FR";
                        }
                        break;
                    }
                case "German":
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            lang = "de_DE";
                        }
                        else if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                        {
                            lang = "de-DE";
                        }

                        break;
                    }
                case "Polish":
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            lang = "pl_PL";
                        }
                        else if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                        {
                            lang = "pl-PL";
                        }

                        break;
                    }
                case "Ukrainian":
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            lang = "uk_UK";
                        }
                        else if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                        {
                            lang = "uk-UK";
                        }

                        break;
                    }
                case "Italian":
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            lang = "it_IT";
                        }
                        else if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                        {
                            lang = "it-IT";
                        }

                        break;
                    }
                case "Русский":
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            lang = "ru_RU";
                        }
                        else if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                        {
                            lang = "ru-RU";
                        }

                        break;
                    }
                case "Chinese":
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            lang = "zh_CN";
                        }
                        else if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                        {
                            lang = "zh-CN";
                        }

                        break;
                    }
                case "Japanese":
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            lang = "ja_JP";
                        }
                        else if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                        {
                            lang = "ja-JP";
                        }

                        break;
                    }
                case "Portuguese(Brazil)":
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            lang = "pt_BR";
                        }
                        else if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                        {
                            lang = "pt-BR";
                        }

                        break;
                    }
                case "Spanish":
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            lang = "es_ES";
                        }
                        else if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                        {
                            lang = "es-ES";
                        }

                        break;
                    }
                default: break;
            }
            #endregion

        }
    }
}