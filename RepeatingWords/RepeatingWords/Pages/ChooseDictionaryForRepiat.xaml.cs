using RepeatingWords.Model;
using System;
using System.Linq;
using Xamarin.Forms;

namespace RepeatingWords.Pages
{
    public partial class ChooseDictionaryForRepiat : ContentPage
    {
        const string NameDbForContinued = "ContinueDictionary";
        const string NameDbForContinuedLearn = "ContinueDictionary-learning";
        public ChooseDictionaryForRepiat()
        {
            InitializeComponent();
            this.BindingContext = App.Db.GetDictionarys().Where(x => x.Name != NameDbForContinued && x.Name != NameDbForContinuedLearn).OrderBy(x => x.Name);
        }


        //вызов главной страницы и чистка стека страниц
        private async void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
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
            string ModalActList = Resource.ModalActList;

            if (words.Any())
            {
                var action = await DisplayActionSheet(ModalChooseLang, "", "", ModalActFromFtoTr, ModalActFromTrtoF, ModalActList);
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
                        if (action == ModalActList)
                        {
                            Dictionary d = App.Db.GetDictionary(((Dictionary)e.SelectedItem).Id);
                            AddWord ad = new AddWord(d);
                            await Navigation.PushAsync(ad);
                        }
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

    }
}