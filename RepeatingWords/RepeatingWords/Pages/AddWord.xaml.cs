using RepeatingWords.Model;
using RepeatingWords.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;

namespace RepeatingWords
{
    public partial class AddWord : ContentPage
    {
        Dictionary dict;
        public AddWord(Dictionary dictionary)
        {
            InitializeComponent();
            dict = dictionary;
            int ws = App.Wr.GetWords(dictionary.Id).Count();
            //для отображения имени словаря в заголовке
            DictionaryName.Text = dict.Name + " (" + ws.ToString() + ")";
            this.BindingContext = App.Wr.GetWords(dictionary.Id);
        }

        //вызов главной страницы и чистка стека страниц
        private async void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        //обработка перехода на страницу Rendering
        protected override void OnAppearing()
        {
            int ws = App.Wr.GetWords(dict.Id).Count();
            //для отображения имени словаря в заголовке
            DictionaryName.Text = dict.Name + " (" + ws.ToString() + ")";
            wordsList.ItemsSource = App.Wr.GetWords(dict.Id);
            //полуаем размеры ширины столбцов grid
            base.OnAppearing();
        }
        //обработка нажатия по эл списка
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //получаем нажатый элемент
            Words ws = (Words)e.SelectedItem;
            string ModalChooceAction = Resource.ModalChooceAction;
            string ModalCancel = Resource.ModalActCancel;
            string ModalChange = Resource.ModalActChange;
            string ModalRemove = Resource.ModalRemoveAct;
            string ModalWord = Resource.ModalWord;
            string ModalWordRemove = Resource.ModalWordRemove;
            //создаем меню с действиями
            var action = await DisplayActionSheet(ModalChooceAction, "", ModalCancel, ModalChange, ModalRemove);
            //выбор действия
            if (action == ModalChange)
            {
                CreateWord cw = new CreateWord(dict.Id, ws);
                await Navigation.PushAsync(cw);
            }
            if (action == ModalRemove)
            {
                App.Wr.DeleteWord(ws.Id);
                if (App.LAr.GetLastAction() != null)
                {
                    if (App.LAr.GetLastAction().IdWord == ws.Id)
                        App.LAr.DelLastAction();
                }
                await DisplayAlert(ModalWord + " " + ws.RusWord + " " + ModalWordRemove, "", "Ok");
                OnAppearing();
            }
        }
        //обр клика по кн создания слова
        private async void CreateWordButtonClick(object sender, System.EventArgs e)
        {
            CreateWord cw = new CreateWord(dict.Id);
            Words word = new Words();
            cw.BindingContext = word;
            await Navigation.PushAsync(cw);
        }

        private async void RepeatWordsButtonClick(object sender, System.EventArgs e)
        {
            IEnumerable<Words> wor = App.Wr.GetWords(dict.Id);
            string ModalChooseLang = Resource.ModalChooseLang;
            string ModalActFromFtoTr = Resource.ModalActFromFtoTr;
            string ModalActFromTrtoF = Resource.ModalActFromTrtoF;
            string ModalException = Resource.ModalException;
            string ModalNoWord = Resource.ModalNoWord;
            if (wor.Any())
            {
                bool FromRussia = false;
                var action = await DisplayActionSheet(ModalChooseLang, "", "", ModalActFromFtoTr, ModalActFromTrtoF);
                if (action == ModalActFromFtoTr)
                {
                    FromRussia = true;
                    RepeatWord rw = new RepeatWord(dict.Id, FromRussia);
                    await Navigation.PushAsync(rw);
                }
                else
                {
                    if (action == ModalActFromTrtoF)
                    {
                        FromRussia = false;
                        RepeatWord rw = new RepeatWord(dict.Id, FromRussia);
                        await Navigation.PushAsync(rw);
                    }
                }
            }
            else
                await DisplayAlert(ModalException, ModalNoWord, "Ок");
        }
    }
}