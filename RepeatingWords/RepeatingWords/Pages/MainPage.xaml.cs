using RepeatingWords.Model;
using RepeatingWords.Pages;

using Xamarin.Forms;

namespace RepeatingWords
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void ChooseDbButtonClick(object sender, System.EventArgs e)
        {
            ChooseDb chd = new ChooseDb();          
            await Navigation.PushAsync(chd);
        }
        private async void ChooseDictionaryButtonClick(object sender, System.EventArgs e)
        {
            ChooseDictionaryForRepiat chd = new ChooseDictionaryForRepiat();
            await Navigation.PushAsync(chd);
        }
        private async void ReturnButtonClick(object sender, System.EventArgs e)
        {

            LastAction la = App.LAr.GetLastAction();
          
            if (la != null)
            {
                        RepeatWord adwords = new RepeatWord(la);
                        await Navigation.PushAsync(adwords);
             }
            else
            {
               await DisplayAlert(Resource.ModalException, Resource.ModalDictOrWordRemove, "Ok");
            }

          
        }
        private async void SpravkaButtonClick(object sender, System.EventArgs e)
        {
            Spravka spv = new Spravka();
            await Navigation.PushAsync(spv);
        }
                     
    }
}
