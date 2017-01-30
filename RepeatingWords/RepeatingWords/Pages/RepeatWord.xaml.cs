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
            UpdateWord(Count,FromRus);
            // код для Android
            if (Device.OS == TargetPlatform.Android)
            {
                 DependencyService.Get<IAdmobInterstitial>().Show("ca-app-pub-5351987413735598/1185308269");
            }
            
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
            UpdateWord(Count, FromRus);
            if (Device.OS == TargetPlatform.Android)
            {
                DependencyService.Get<IAdmobInterstitial>().Show("ca-app-pub-5351987413735598/1185308269");
            }
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

        string lang = "en_GB";

        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (picker.Items[picker.SelectedIndex].ToString())
            {
                case "English":
                    {
                        lang = "en_GB";
                        break;
                    }
                case "French":
                    {
                        lang = "fr_FR";
                        break;
                    }
                case "German":
                    {
                        lang = "de_DE";
                        break;
                    }
                case "Polish":
                    {
                        lang = "pl_PL";
                        break;
                    }
                case "Ukrainian":
                    {
                        lang = "uk_UK";
                        break;
                    }
                case "Italian":
                    {
                        lang = "it_IT";
                        break;
                    }
                case "Русский":
                    {
                        lang = "ru_RU";
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
