using RepeatingWords.Model;
using RepeatingWords.Pages;
using System;
using System.Linq;
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
                Dictionary dic = App.Db.GetDictionarys().Where(x => x.Id == la.IdDictionary).FirstOrDefault();
                if (dic != null)
                {
                    RepeatWord adwords = new RepeatWord(la);
                    await Navigation.PushAsync(adwords);
                }
                else
                {
                    await DisplayAlert(Resource.ModalException, Resource.ModalDictOrWordRemove, "Ok");
                }
            }
            else
            {
                await DisplayAlert(Resource.ModalException, Resource.ModalDictOrWordRemove, "Ok");
            }
        }
        //вызов страницы настроек
        private async void ClickedToolsButton(object sender, EventArgs e)
        {
            ToolsPage ta = new ToolsPage();
            await Navigation.PushAsync(ta);
        }
        //оставить отзыв о программе
        private async void ClickedLikeButton(object sender, EventArgs e)
        {
            string MessagePleaseReview = Resource.MessagePleaseReview;
            string ButtonSendReview = Resource.ButtonSendReview;
            string ButtonCancel = Resource.ModalActCancel;
            bool action = await DisplayAlert("", MessagePleaseReview, ButtonSendReview, ButtonCancel);
            if (action)
            {
                if (Device.OS == TargetPlatform.Android)
                    Device.OpenUri(new Uri("https://play.google.com/store/apps/details?id=cardsofwords.cardsofwords"));
                if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
                {
                    Device.OpenUri(new Uri("https://www.microsoft.com/store/apps/9n55bwkgshnf"));
                }
            }
        }
        private async void ClickedHelpButton(object sender, EventArgs e)
        {
            Spravka spv = new Spravka();
            await Navigation.PushAsync(spv);
        }
    }
}