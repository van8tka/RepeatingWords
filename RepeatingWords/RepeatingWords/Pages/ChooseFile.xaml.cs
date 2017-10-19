using RepeatingWords.Model;

using System.Collections.Generic;
using System.Collections;
using Xamarin.Forms;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RepeatingWords.Pages
{
    public partial class ChooseFile : ContentPage
    {
        Dictionary dictionary;
        string RootPath;
        bool getFolder;
        //для выбора ппапки
        public ChooseFile()
        {
            InitializeComponent();
            getFolder = true;
            GetFolderList();
            LabelInfo.Opacity = 0;
        }


        //для добавления слов из выбранного файла и выбора файла
        public ChooseFile(Dictionary dictionary)
        {
            InitializeComponent();
            getFolder = false;
            this.dictionary = dictionary;
            GetFileList();
            //загружаем список файлов по пути MyDocuments
            LabelInfo.Opacity = 0;
        }


      
        void TapOnDirectoryLabel(object sender, EventArgs e)
        {
           
        }




        private Task GetFolderList()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var Permission = await UpdateFileList(false);
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





        private Task GetFileList()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var Permission = await UpdateFileList();
                    if (Permission)
                        textPath.Text = DependencyService.Get<IFileWorker>().GetDocsPath().ToString();
                    else
                        textPath.Text = "App doesn't have permission to read data. Please check settings: Settings->Applications->Application Manager and find your app. Permissions must be set.";

                }
                catch (Exception er)
                {

                }
            });
        }





        private async void FileSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                IsLoading = true;
                if (e.SelectedItem == null) return;
                string filename = (string)e.SelectedItem;
                if (getFolder)
                {
                    RootPath = RootPath + "/" + filename;
                    textPath.Text = RootPath;
                    await UpdateFileList(false, RootPath);
                }
                else
                {
                  await GetWordsFromFile(filename);
                }
                IsLoading = false;

            }
            catch (Exception er)
            {
                IsLoading = false;
                await DisplayAlert("Error", er.Message, "Ok");
            }
        }









        private Task GetWordsFromFile(string filename)
        {
            return Task.Run(async () =>
            {
                try
                {
                    List<string> lines = await DependencyService.Get<IFileWorker>().LoadTextAsync(filename);

                    //проходим по списку строк считанных из файла

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


                    string ModalAddWords = Resource.ModalAddWords;
                    string ModalException = Resource.ModalException;
                    string ModalIncorrectFile = Resource.ModalIncorrectFile;

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
                catch (Exception er)
                {
                    throw;
                }
            });          
        }






     public async Task<bool> UpdateFileList(bool getFilesList = true, string folderPath = null)
        {
            try
            {
                IsLoading = true;
                //получим список файлов или папок
                if (getFilesList)
                {
                    //получаем все файлы
                    fileList.ItemsSource = await DependencyService.Get<IFileWorker>().GetFilesAsync();
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
            await UpdateFileList(false, RootPath);
        }

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
                    UpdateFileList(false, RootPath);
                }
            }            
            else
                Navigation.PopAsync();
            return true;
        }
    }
}