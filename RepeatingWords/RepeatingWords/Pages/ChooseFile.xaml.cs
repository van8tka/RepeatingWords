using RepeatingWords.Model;

using System.Collections.Generic;
using System.Collections;
using Xamarin.Forms;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using XLabs.Forms.Behaviors;
using XLabs.Forms.Controls;


namespace RepeatingWords.Pages
{
    public partial class ChooseFile : ContentPage
    {
        Dictionary dictionary;
        string RootPath;
        bool getFolder;
        //путь для резервоной копии БД
        string filePathDb;
        string fileNameBackUp;

        //для выбора ппапки
        public ChooseFile(string filePathDb, string fileNameBackUp)
        {
            InitializeComponent();
            this.filePathDb = filePathDb;
            this.fileNameBackUp = fileNameBackUp;
            getFolder = true;
            GetFolderList();
            LabelInfo.Opacity = 0;
            CreateFolderButton.IsEnabled = true;
            CreateFolderButton.Opacity = 1;
        }


        //для добавления слов из выбранного файла и выбора файла
        public ChooseFile(Dictionary dictionary=null)
        {
            InitializeComponent();
            getFolder = false;
            this.dictionary = dictionary;
            //GetFileList();
            GetFolderList();
            //загружаем список файлов по пути MyDocuments
            LabelInfo.Opacity = 0;
            CreateFolderButton.IsEnabled = false;
            CreateFolderButton.Opacity = 0;
        }



       
       

        private Task GetFolderList()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var Permission = await UpdateFileList(getFolder);
                    if (Permission)
                    {
                        RootPath = DependencyService.Get<IFolderWorker>().GetRootPath();
                        textPath.Text = RootPath;
                    }                       
                    else
                        textPath.Text = "App doesn't have permission to read data. Please check settings: Settings->Applications->Application Manager and find your app. Permissions must be set.";                 
                }
                catch (Exception er)
                {
                   
                }
            });
        }





        //private Task GetFileList()
        //{
        //    return Task.Run(async () =>
        //    {
        //        try
        //        {
        //            var Permission = await UpdateFileList();
        //            if (Permission)
        //                textPath.Text = DependencyService.Get<IFileWorker>().GetDocsPath().ToString();
        //            else
        //                textPath.Text = "App doesn't have permission to read data. Please check settings: Settings->Applications->Application Manager and find your app. Permissions must be set.";
        //        }
        //        catch (Exception er)
        //        {

        //        }
        //    });
        //}




        //при коротком нажатии на папку или на файл
        private async void FileSelected(string nameItemSelected)
        {
            try
            {
                IsLoading = true;
                if (nameItemSelected == null) return;
                string filename = nameItemSelected;
                var varPath = RootPath + "/" + filename;
                //проверим тапнут file or folder
                if (!DependencyService.Get<IFileWorker>().IsFile(varPath))
                {
                    RootPath = RootPath + "/" + filename;
                    textPath.Text = RootPath;
                    await UpdateFileList(getFolder, RootPath);
                }
                else//если файл 
                {
                    if(dictionary!=null)//если dictionary не нуль
                    {
                        await GetWordsFromFile(varPath);//то получаем список слов
                    }//иначе восстановим бэкап
                    else
                    {

                    }

                  
                }
                IsLoading = false;
            }
            catch (Exception er)
            {
                IsLoading = false;
                await DisplayAlert("Error", er.Message, "Ok");
            }
        }









        private Task GetWordsFromFile(string filePath)
        {
            return Task.Run(async () =>
            {
                try
                {
                    string ModalAddWords = Resource.ModalAddWords;
                    string ModalException = Resource.ModalException;
                    string ModalIncorrectFile = Resource.ModalIncorrectFile;
                    List<string> lines = await DependencyService.Get<IFileWorker>().LoadTextAsync(filePath);
                //проходим по списку строк считанных из файла
                if (lines != null && lines.Count>0)
                    {
                        char[] delim = { '[', ']' };
                        //переменная для проверки добавления слов
                        bool CreateWordsFromFile = false;
                        //проход по списку слов
                        foreach (var i in lines)
                        {//проверка на наличие разделителей, т.е. транскрипции в строке(символы транскрипции и есть разделители)
                            if (i.Contains("[") && i.Contains("]"))
                            {
                                CreateWordsFromFile = true;
                                string[] fileWords = i.Split(delim);
                                Words item = new Words
                                {
                                    Id = 0,
                                    IdDictionary = dictionary.Id,
                                    RusWord = fileWords[0],
                                    Transcription = "[" + fileWords[1] + "]",
                                    EngWord = fileWords[2]
                                };//добавим слово в БД
                                await App.WrAsync.AsyncCreateWord(item);
                            }
                        }

                        if (CreateWordsFromFile)
                        {
                            await DisplayAlert("", ModalAddWords, "Ok");
                            AddWord adw = new AddWord(dictionary);
                            await Navigation.PushAsync(adw);
                        }
                        else
                        {
                            await DisplayAlert(ModalException, ModalIncorrectFile, "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert(ModalException, ModalIncorrectFile, "Ok");
                    }
                }
                catch (Exception er)
                {
                    throw;
                }
            });          
        }





        //обработчик нажатий на папку или на файл(определяет короткий или длинный тап)_
        private async void GesturesContentView_GestureRecognized(object sender, GestureResult e)
        {
            string itemTap = ((Label)((GesturesContentView)sender).Content).Text;
            switch (e.GestureType)
            {
                case GestureType.LongPress:
                    {
                        if(getFolder)
                        {
                          string pathToSaveBackUp = RootPath + "/" + itemTap;
                          CreateBackUp(pathToSaveBackUp, itemTap);
                        }
                         break;
                    }
                case GestureType.SingleTap:
                    {
                        FileSelected(itemTap);
                        break;
                    }                 
                //case GestureType.DoubleTap:
                //    // Add code here
                //    break;
                default:
                    break;
            }
        }




        //создание бэкапап по указанному пути
        private async void CreateBackUp(string pathToSaveBackUp, string folderName)
        {
            try
            {
                const string Yes = "Да";
                const string Cancel = "Нет";
                var action = await DisplayActionSheet("Вы хотите создать резервную копию в " + folderName, Yes, Cancel);
                switch (action)
                {
                    case Yes:
                        {
                            string fullPathBackup = pathToSaveBackUp + "/" + fileNameBackUp;
                            //создаем резервную копию передаем путь к БД и путь для сохранения резервной копиии
                            bool succes = DependencyService.Get<IFileWorker>().WriteFile(filePathDb, fullPathBackup);
                            if (succes)
                            {
                                await DisplayAlert("Успешно", "Резервная копия создана по адресу: " + fullPathBackup, "Ок");
                            }
                            else
                            {
                                await DisplayAlert("Ошибка", "К сожалению во время создания резервной копии произошла ошибка, попробуйте другой способ создания резервной копии.", "Ок");
                            }
                          await Navigation.PopAsync();
                        }
                        break;
                    case Cancel:
                        break;
                    default: break;
                }
            }
            catch(Exception er)
            {

            }
        }

        public async Task<bool> UpdateFileList(bool getFolderList, string folderPath = null)
        {
            try
            {
                IsLoading = true;
                //получим список файлов или папок
                if (!getFolderList)
                {
                    //получаем все файлы
                    var folderL = await DependencyService.Get<IFolderWorker>().GetFoldersAsync(folderPath);
                    
                    var fileL = await DependencyService.Get<IFileWorker>().GetFilesAsync(RootPath);
                    folderL.AddRange(fileL);
                    fileList.ItemsSource = folderL;
                }
                else //или папок
                {
                    fileList.ItemsSource = await DependencyService.Get<IFolderWorker>().GetFoldersAsync(folderPath);
                }
                if (fileList != null)
                {
                    fileList.SelectedItem = null;
                    IsLoading = false;
                    return true;
                }
                else
                {
                    IsLoading = false;
                    return false;
                    //снимаем выделение
                }
               
            }
            catch(Exception er)
            {
                IsLoading = false;
                return false;
            }
        }






        //создание новой папки
        private async void ClickedCreateFolderBtn(object sender, EventArgs e)
        {
            CreateDb cr = new CreateDb(true,RootPath, this);
            await Navigation.PushAsync(cr);
            await UpdateFileList(getFolder, RootPath);
        }





        //показываем снизу сообщение 
        public Task ShowInfoMessage(string message)
        {
            LabelInfo.Text = message;
            return Task.Run(async() => {       
                   await LabelInfo.FadeTo(1, 4000);
              await LabelInfo.FadeTo(0, 4000);
            });
        }









        //вызов главной страницы и чистка стека страниц
        private async void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }




        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                isLoading = value;
                actIndicator3.IsRunning = isLoading;
            }
        }




        //переопределение метода обработки события при нажатии кнопки НАЗАД, 
        //для его срабатывания надо в MainActivity тоже переопределить метод OnBackPressed
        protected override bool OnBackButtonPressed()
        {//метод добавления словаря для возможности продолжения с последнего места
            string rootPath = DependencyService.Get<IFolderWorker>().GetRootPath();
            if(!string.IsNullOrEmpty(RootPath))
            {
                if (RootPath == rootPath)
                {
                    Navigation.PopAsync();
                }
                else
                {
                    RootPath = RootPath.Remove(RootPath.LastIndexOf('/'));
                    textPath.Text = RootPath;
                    UpdateFileList(getFolder, RootPath);
                }
            }            
            else
                Navigation.PopAsync();
            return true;
        }
    }
}