using RepeatingWords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            }
            else
            {
                WordForRepeat.TextColor = Color.Yellow;
                Words w = GetWords(count);
                WordForRepeat.Text = w.EngWord + "\n" + w.Transcription;
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
    }
}
