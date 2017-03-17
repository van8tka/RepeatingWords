using RepeatingWords.Model;
using System;
using RepeatingWords.Pages;
using Xamarin.Forms;


namespace RepeatingWords
{
    public partial class CreateWord : ContentPage
    {
        int idDiction;
        //1 конструктор для создания
        public CreateWord(int iddiction)
        {
            InitializeComponent();
            idDiction = iddiction;
            //переменная для опеределения количества установок курсора на поле транскрипции
            FocusCoutTransc = 1;
        }

        //2 констр для изм слова
        public CreateWord(int iddiction, Words changeword)
        {
            InitializeComponent();
            idDiction = iddiction;
            this.BindingContext = changeword;
            FocusCoutTransc = 1;
        }


        //3 констр при вводе транскрипции со спец клавы
        public CreateWord(int iddiction,int idWord ,string rus, string eng, string transc)
        {
            InitializeComponent();
            idDiction = iddiction;
            Words wr = new Words
            {
                Id= idWord,
                IdDictionary = idDiction,
                RusWord = rus,
                EngWord = eng,
                Transcription = transc
            };
            this.BindingContext = wr;
        
            FocusCoutTransc = 1;
        }



        private async void CreateWordButtonClick(object sender, System.EventArgs e)
        {
            try
            {
                var words = (Words)BindingContext;

                string ModelWordAdd = Resource.ModelWordAdd;
                string ModelWordChange = Resource.ModelWordChange;
                string ModelNoFillFull = Resource.ModelNoFillFull;
                string ModelForAddingWord = Resource.ModelForAddingWord;
             
                if (!String.IsNullOrEmpty(words.RusWord) && !String.IsNullOrEmpty(words.EngWord))
                {
                    if (!String.IsNullOrEmpty(words.Transcription))
                    {
                        if (!words.Transcription.StartsWith("["))
                            words.Transcription = "[" + words.Transcription;
                        if (!words.Transcription.EndsWith("]"))
                            words.Transcription = words.Transcription + "]";
                    }
                    else
                    {
                        words.Transcription = "[]";
                    }

                    int z = words.Id;
                    words.IdDictionary = idDiction;
                    App.Wr.CreateWord(words);
                    if (z == 0)
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
            catch { }
        }

        int FocusCoutTransc = 1;

        private async void EntryTransc_Focused(object sender, FocusEventArgs e)
        {
            try
            {
                const string TrKeyboard = "TrKeyboard";
                const string showKeyboard = "true";
                object propTrKeyb;
                //проверяем если первый клик то предлогать выбор клав если 2 и более то только сист клавиат
                if (FocusCoutTransc == 1)
                {//получаем значение из настроек сохраненных в приложении
                    App.Current.Properties.TryGetValue(TrKeyboard, out propTrKeyb);
                    //если настроено на показ то показываем окно выбора клавиатуры
                    if (propTrKeyb.Equals(showKeyboard))
                    {
                        FocusCoutTransc++;

                        string ModalActChooseKeyboard = Resource.ModalActChooseKeyboard;
                        string ModalActCancel = Resource.ModalActCancel;
                        string ModalActFromTrtoF = Resource.ModalActFromTrtoF;
                        string ModalActSysKeyboard = Resource.ModalActSysKeyboard;
                        string ModalActTranscKeyboard = Resource.ModalActTranscKeyboard;

                        var action = await DisplayActionSheet(ModalActChooseKeyboard, ModalActCancel, null, ModalActSysKeyboard, ModalActTranscKeyboard);
                        if (action == ModalActTranscKeyboard)
                        {
                            var words = (Words)BindingContext;
                            EntryTranscription tr = new EntryTranscription(idDiction, words.Id, words.RusWord, words.EngWord, words.Transcription);
                            await Navigation.PushAsync(tr);
                        }
                    }

                }
            }
           catch { }
        }
    }

}
