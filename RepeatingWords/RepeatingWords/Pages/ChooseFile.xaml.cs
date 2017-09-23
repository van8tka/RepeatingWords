﻿using RepeatingWords.Model;
using System.Collections.Generic;
using Xamarin.Forms;
using System;
using System.Threading.Tasks;

namespace RepeatingWords.Pages
{
    public partial class ChooseFile : ContentPage
    {
        Dictionary dictionary;
        public ChooseFile(Dictionary dictionary)
        {
            InitializeComponent();
            IsLoading = false;
            this.dictionary = dictionary;
            var Permission = UpdateFileList().Result;
            if (Permission)
                textPath.Text = DependencyService.Get<IFileWorker>().GetDocsPath().ToString();
            else
                textPath.Text = "App doesn't have permission to read data. Please check settings: Settings->Applications->Application Manager and find your app. Permissions must be set.";
            //загружаем список файлов по пути MyDocuments

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



        private async void FileSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                IsLoading = true;

                if (e.SelectedItem == null) return;
                string filename = (string)e.SelectedItem;
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
                    IsLoading = false;
                    await DisplayAlert("", ModalAddWords, "Ok");
                    AddWord adw = new AddWord(dictionary);
                    await Navigation.PushAsync(adw);
                }
                else
                {
                    IsLoading = false;
                    await DisplayAlert(ModalException, ModalIncorrectFile, "Ok");
                }

            }
            catch (Exception er)
            {
                await DisplayAlert("Error", er.Message, "Ok");
            }
        }


        async Task<bool> UpdateFileList()
        {
            //получаем все файлы
           
            fileList.ItemsSource = await DependencyService.Get<IFileWorker>().GetFilesAsync();
            if (fileList != null)
            {
                fileList.SelectedItem = null;
                return true;
            }
               
            else
            {
                return false;
                //снимаем выделение
            }


        }
    }
}