using RepeatingWords.Model;
using RepeatingWords.Pages;
using System;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;


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



        string filePathDb = App.Db.DBConnection.DatabasePath;
        string fileNameBackupDef = "backupcardsofwords";
        const string folderNameBackUp = "CardsOfWordsBackup";
        //пока только для ведроида
        //создание backup DB






        private async void BackUpButtonCkick(object sender, EventArgs e)
        {
            try
            {
                const string setFolder = "Создание в указанной папке";
                const string googleDriveFolder = "Создание на Google диск";
                //создание имени файла резервной копии
                string fileNameBackup = string.Format(fileNameBackupDef + DateTime.Now.ToString("ddMMyyyy") + ".dat");

                var action = await DisplayActionSheet("Выберите способ создания резервной копии", "Отмена", null, setFolder, googleDriveFolder);
                switch (action)
                {
                   case setFolder:
                        {
                            CreateBackUpIntoSetFolder(fileNameBackup);
                            break;
                        }
                    case googleDriveFolder:
                        {
                            CreateBackUpIntoGoogleDrive(fileNameBackup);
                            break;
                        }
                    default: break;
                }
            }   
            catch(Exception er)
            {
                Debug.WriteLine("------custom er---BackUpButtonCkick-----" + er.Message + "----tracer----" + er.StackTrace);
            }
        
        }

      



      
        //создание в указанной папке
        private async void CreateBackUpIntoSetFolder(string fileNameBackup)
        {
            ChooseFile ch = new ChooseFile(filePathDb, fileNameBackup);
             await  Navigation.PushAsync(ch);
        }



        private async void CreateBackUpIntoGoogleDrive(string fileNameBackup)
        {
            GoogleEnterPage gp = new GoogleEnterPage(fileNameBackup);
            await Navigation.PushAsync(gp);
        }

        








        //восстановление из backup
        private async void RestoreFromBackUpButtonCkick(object sender, EventArgs e)
        {
            const string setFolder = "Указать путь к резервной копии";
            const string googleDriveFolder = "Поиск резервной копии на Google диск";
            //создание имени файла резервной копии
            string fileNameBackup = string.Format(fileNameBackupDef + DateTime.Now.ToString("ddMMyyyy") + ".dat");

            var action = await DisplayActionSheet("Поиск резервной копии", "Отмена", null, setFolder, googleDriveFolder);
            switch (action)
            {
               case setFolder:
                    {
                        ChooseFile ch = new ChooseFile(null);
                        await Navigation.PushAsync(ch);
                        break;
                    }
                case googleDriveFolder:
                    {
                        GetItemsFromGoogle();
                        break;
                    }
                default: break;
            }



           
               
        }

        private void GetItemsFromGoogle()
        {
            try
            {
              //var items = DependencyService.Get<IGoogleDriveWorker>().GetGoogleItems();
             }
            catch(Exception er)
            {
                ErrorHandler.getError(er, "MainPage.GetItemsFromGoogle");
            }
        }
    }
}