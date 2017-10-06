using RepeatingWords.Model;
using RepeatingWords.Pages;
using System;
using System.Linq;
using Xamarin.Forms;
using System.IO;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
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
            string fileNameBackup = string.Format(fileNameBackupDef+DateTime.Now.ToString("ddMMyyyy") + ".dat");
            //сохранить резервную копию бд 1 в папку по умолчанию,2 в пользовательскую папку,3 на googledrive
            string filePathDefault = DependencyService.Get<IFileWorker>().CreateFolder(folderNameBackUp, fileNameBackup);
           bool succes = DependencyService.Get<IFileWorker>().WriteFile(filePathDb,filePathDefault);  
           if(succes)
            {

            }
            //FileData fileData = new FileData();
            //fileData = await CrossFilePicker.Current.PickFile();
            //byte[] data = fileData.DataArray;
            //string name = fileData.FileName;
            //string filePathToSave = fileData.FilePath;

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