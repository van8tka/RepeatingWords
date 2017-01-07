using RepeatingWords.Model;
using System.Linq;
using Xamarin.Forms;

namespace RepeatingWords.Pages
{
    public partial class ChooseDictionaryForRepiat : ContentPage
    {
      
        public ChooseDictionaryForRepiat()
        {
            InitializeComponent();
            this.BindingContext = App.Db.GetDictionarys();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            bool FromRussia = false;
            var words = App.Wr.GetWords(((Dictionary)e.SelectedItem).Id);

            string ModalChooseLang = Resource.ModalChooseLang;
            string ModalActFromFtoTr = Resource.ModalActFromFtoTr;
            string ModalActFromTrtoF = Resource.ModalActFromTrtoF;
            string ModalException = Resource.ModalException;
            string ModalNoWord = Resource.ModalNoWord;

            if (words.Any())
            {
                var action = await DisplayActionSheet(ModalChooseLang, "", "", ModalActFromFtoTr, ModalActFromTrtoF);
                bool which = true;//для проверки выбрана ли кнопка выбора языка или нет

                if (action == ModalActFromFtoTr)
                {
                    FromRussia = true;
                }
                else
                {
                    if (action == ModalActFromTrtoF)
                    {
                        FromRussia = false;
                    }
                    else
                    {
                        which = false;
                    }
                }
          
                if (which)
                {
                    RepeatWord rw = new RepeatWord(((Dictionary)e.SelectedItem).Id, FromRussia);
                    await Navigation.PushAsync(rw);
                }
            }
            else
                await DisplayAlert(ModalException, ModalNoWord, "Ок");

        }

    }
}
