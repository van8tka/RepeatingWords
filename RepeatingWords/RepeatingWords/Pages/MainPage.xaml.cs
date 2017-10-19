using RepeatingWords.Model;
using RepeatingWords.Pages;
using System;
using System.Linq;
using Xamarin.Forms;
using System.IO;
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
                const string localFolder = "Создание в папке по умолчании";
                const string setFolder = "Создание в указанной папке";
                const string googleDriveFolder = "Создание на Google диск";
                //создание имени файла резервной копии
                string fileNameBackup = string.Format(fileNameBackupDef + DateTime.Now.ToString("ddMMyyyy") + ".dat");

                var action = await DisplayActionSheet("Выберите способ создания резервной копии", "Отмена", null, localFolder, setFolder, googleDriveFolder);
                switch (action)
                {
                    case localFolder:
                        {
                            CreateBackUpIntoDefaultFolder(fileNameBackup);
                            break;
                        }
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

        //сохранение в папку по умолчанию
        private async Task CreateBackUpIntoDefaultFolder(string fileNameBackup)
        {
            try
            {//получим путь к папке
                string filePathDefault = DependencyService.Get<IFileWorker>().CreateFolder(folderNameBackUp, fileNameBackup);
                //создаем резервную копию передаем путь к БД и путь для сохранения резервной копиии
                bool succes = DependencyService.Get<IFileWorker>().WriteFile(filePathDb, filePathDefault);
                if (succes)
                {
                    await DisplayAlert("Успешно", "Резервная копия создана в дирректории CardsOfWordsBackup.", "Ок");
                }
                else
                {
                    await DisplayAlert("Ошибка", "К сожалению во время создания резервной копии произошла ошибка, попробуйте другой способ создания резервной копии.", "Ок");
                }
            }
            catch(Exception er)
            {
                Debug.WriteLine("___________customerror__CreateBackUpIntoDefaultFolder___:" + er.Message);
            }
        }

        //создание в указанной папке
        private async void CreateBackUpIntoSetFolder(string fileNameBackup)
        {
            ChooseFile ch = new ChooseFile();
            await Navigation.PushAsync(ch);

            //создаем резервную копию передаем путь к БД и путь для сохранения резервной копиии
            //bool succes = DependencyService.Get<IFileWorker>().WriteFile(filePathDb, filePathDefault);
            //if (succes)
            //{
            //    await DisplayAlert("Успешно", "Резервная копия создана по адресу: "+ filePathDefault, "Ок");
            //}
            //else
            //{
            //    await DisplayAlert("Ошибка", "К сожалению во время создания резервной копии произошла ошибка, попробуйте другой способ создания резервной копии.", "Ок");
            //}

        }



        private void CreateBackUpIntoGoogleDrive(string fileNameBackup)
        {
           
        }

        








        //восстановление из backup
        private async void RestoreFromBackUpButtonCkick(object sender, EventArgs e)
        {
            //для восстановления данных по умолчанию
            //получим последний файл бэкапа
            string fileBackUp = await DependencyService.Get<IFileWorker>().GetBackUpFilesAsync(folderNameBackUp);
            bool succes;
            if (!string.IsNullOrEmpty(fileBackUp))
            {
                succes = DependencyService.Get<IFileWorker>().WriteFile(fileBackUp, filePathDb);
            }
               
        }
    }
}