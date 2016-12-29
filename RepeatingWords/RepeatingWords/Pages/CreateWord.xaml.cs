using RepeatingWords.Model;
using System;

using Xamarin.Forms;

namespace RepeatingWords
{
    public partial class CreateWord : ContentPage
    {
        int idDiction;
        //первый конструктор для создания
        public CreateWord(int iddiction)
        {
            InitializeComponent();
            idDiction = iddiction;
        }

        //вт констр для изм слова
        public CreateWord(int iddiction, Words changeword)
        {
            InitializeComponent();
            idDiction = iddiction;
            this.BindingContext = changeword;
        }



        private async void CreateWordButtonClick(object sender, System.EventArgs e)
        {
            var words = (Words)BindingContext;

            string ModelWordAdd = Resource.ModelWordAdd;
            string ModelWordChange = Resource.ModelWordChange;
            string ModelNoFillFull = Resource.ModelNoFillFull;
            string ModelForAddingWord = Resource.ModelForAddingWord;

            if (!String.IsNullOrEmpty(words.RusWord)&&!String.IsNullOrEmpty(words.EngWord))
            {
                if (!String.IsNullOrEmpty(words.Transcription))
                {
                    if (!words.Transcription.StartsWith("["))
                        words.Transcription = "[" + words.Transcription;
                    if(!words.Transcription.EndsWith("]"))
                        words.Transcription = words.Transcription+"]";
                }
                else
                {
                    words.Transcription = "[]";
                }

                int z = words.Id;
                words.IdDictionary = idDiction;
                 App.Wr.CreateWord(words);
                if(z==0)
                     await DisplayAlert("", ModelWordAdd, "Ok");
                else
                    await DisplayAlert("", ModelWordChange, "Ok");


                    Dictionary dict = App.Db.GetDictionary(words.IdDictionary);

                AddWord adw = new AddWord(dict);
                await Navigation.PushAsync(adw);
            }
            else
            {
                await DisplayAlert(ModelNoFillFull, ModelForAddingWord, "Ok");
            }          
        }
    }
}
