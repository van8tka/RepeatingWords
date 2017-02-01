using RepeatingWords.Model;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace RepeatingWords.Pages
{
    public partial class RepeatWord : ContentPage
    {
        bool FromRus;
        int  iDdictionary;
        bool Turn;
        IEnumerable<Words> words;
        int Count = 0;
        int countW = 1;

        public RepeatWord(int iDdictionary, bool FromRus)
        {
            InitializeComponent();
            this.FromRus = FromRus;
            //переменная для многократного переворота карточки
            Turn = FromRus;
            this.iDdictionary = iDdictionary;

            words = App.Wr.GetWords(iDdictionary);
            //кол во слов и пройденных слов
            LabelCountOfWords.Text = words.Count().ToString() + "/" + (countW + Count).ToString();

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



        public RepeatWord(LastAction la)
        {
            InitializeComponent();
            FromRus = la.FromRus;
            //переменная для многократного переворота карточки
            Turn = FromRus;
            iDdictionary = la.IdDictionary;
            words = App.Wr.GetWords(la.IdDictionary);
                              
            //определяем чему равен Count;
            HowCount(la.IdWord);
            //сколько слов всего и пройдено
            LabelCountOfWords.Text = words.Count().ToString() + "/" + (countW + Count).ToString();

            if (Device.OS == TargetPlatform.Android)
            {
                lang = "en_GB";
                DependencyService.Get<IAdmobInterstitial>().Show("ca-app-pub-5351987413735598/1185308269");
            }
            else if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
            { lang = "en-GB"; }
            UpdateWord(Count, FromRus);
        }



      



        private void HowCount(int idword)
        {
            int c = 0;
            foreach(var i in words)
            {
                if (i.Id == idword)
                {
                    break;
                }
                else
                {
                    c++;
                }
           }
            if (c == words.Count())
                Count = 0;
            else
                Count = c;
        }



        //метод для обновления слова
        private void UpdateWord(int count, bool lang)
        {
            if (lang)
            {
               WordForRepeat.TextColor = Color.Lime;
               WordForRepeat.Text = GetWords(count).RusWord;
                //управление видимостью озвучки
                ButtonVoice.IsVisible = true;
                ButtonVoice.IsEnabled = false;
                picker.IsVisible = true;
                ButtonVoice.Image = "voiceX.png";
            }
            else
            {
                WordForRepeat.TextColor = Color.Yellow;
                Words w = GetWords(count);
                //если транскрипции нет 
                if(w.Transcription=="[]")
                {//выводим только перевод
                    WordForRepeat.Text = w.EngWord;
                }
                else
                {//перевод и транскрипцию
                    WordForRepeat.Text = w.EngWord + "\n" + w.Transcription;
                }
              
                //управление видимостью озвучки
                ButtonVoice.IsVisible = true;
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

                var action = await DisplayActionSheet(ModalAllWordsComplete, "", "", ModalFinish, ModalRestart);

                if (action == ModalFinish)
                {//перреход на главную страницу
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    Count = 0;
                    UpdateWord(Count, FromRus);
                }
            }
            //сколько слов всего и пройдено сколько
            LabelCountOfWords.Text = words.Count().ToString() + "/" + (countW + Count).ToString();

        }

        //переопределение метода обработки события при нажатии кнопки НАЗАД, 
        //для его срабатывания надо в MainActivity тоже переопределить метод OnBackPressed
        protected override bool OnBackButtonPressed()
        {//при нажаттии  на кн выхода сохраняется последнее слово
            LastAction i = new LastAction()
            {
                Id = 0,
                IdDictionary = iDdictionary,
                FromRus = FromRus,
                IdWord = GetWords(Count).Id
            };
            App.LAr.SaveLastAction(i);
            Navigation.PopToRootAsync();
            return true;
        }


        private void TurnAroundWordButtonClick(object sender, EventArgs e)
          {
            UpdateWord(Count, !Turn);
            Turn = !Turn;//перевернули карточку
        }

        string lang;

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
        }


      private void BtnClickSpeech(object sender, EventArgs e)
        {
             DependencyService.Get<ITextToSpeech>().Speak(GetWords(Count).EngWord,lang);
        }
    }
}
