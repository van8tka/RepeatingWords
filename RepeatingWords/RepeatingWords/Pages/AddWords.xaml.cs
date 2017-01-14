using RepeatingWords.Model;
using RepeatingWords.Pages;
using System.Linq;
using Xamarin.Forms;


namespace RepeatingWords
{
    public partial class AddWords : ContentPage
    {
        int ws;
        string d;
        public AddWords(Dictionary dictionary)
        {           
            InitializeComponent();
            ws = App.Wr.GetWords(dictionary.Id).Count();
            //для отображения имени словаря в заголовке
            d = dictionary.Name;
            DictionaryName.Text = dictionary.Name + " (" + ws.ToString()+")";
            this.BindingContext = dictionary;
        }


        protected override void OnAppearing()
        {
            DictionaryName.Text = d + " (" + ws.ToString() + ")";
            base.OnAppearing();
        }

       


        protected async void AddWordButtonClick(object sender, System.EventArgs e)
        {
            var dictionary = (Dictionary)BindingContext;
          
            CreateWord cr = new CreateWord(dictionary.Id);
            Words word1 = new Words();
            cr.BindingContext = word1;
            await Navigation.PushAsync(cr);
          

        }
       protected async void DelleteDbButtonClick(object sender, System.EventArgs e)
        { 
            var dictionary = (Dictionary)BindingContext;
            //удаляем словарь
            App.Db.DeleteDictionary(dictionary.Id);
            //удаляем слова этого словаря
            App.Wr.DeleteWords(dictionary.Id);
            if(App.LAr.GetLastAction()!=null&& App.LAr.GetLastAction().IdDictionary==dictionary.Id)
            {
                App.LAr.DelLastAction();
            }

          
            string ModalDict = Resource.ModalDict;
            string ModalRemove = Resource.ModalRemove;
            await DisplayAlert(null, ModalDict+" "+dictionary.Name+" "+ModalRemove, "Ок");

            await Navigation.PopAsync();
        }


        //обр нажатия добавления слова из файла
        protected async void AddWordsFromFileButtonClick(object sender, System.EventArgs e)
        {
            var dictionary = (Dictionary)BindingContext;
            ChooseFile ch = new ChooseFile(dictionary);
            await Navigation.PushAsync(ch);
        }


       protected async void ShowWordsDbButtonClick(object sender,System.EventArgs e)
        {
            var dictionary = (Dictionary)BindingContext;
            AddWord adw = new AddWord(dictionary);
            await Navigation.PushAsync(adw);
        }

     

    }
}
