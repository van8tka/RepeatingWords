using RepeatingWords.Model;
using RepeatingWords.Pages;
using System;
using Xamarin.Forms;

namespace RepeatingWords
{
    public partial class CreateDb : ContentPage
    {
        public CreateDb()
        {
            InitializeComponent();
        }


        //для создания директории 
        bool IsCreateFolder;
        string rootPath;
        ChooseFile chFilePage;
        public CreateDb(bool IsCreateFolder, string rootPath, ChooseFile chFilePage)
        {
            InitializeComponent();
            this.IsCreateFolder = IsCreateFolder;
            this.rootPath = rootPath;
            this.chFilePage = chFilePage;
            LabelName.Text = "Введите название каталога";
            EnterName.Placeholder = "Введите название каталога";
            EnterName.Text = "Новая папка";
        }




        //вызов главной страницы и чистка стека страниц
        private async void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        private async void CreateDbButtonClick(object sender, System.EventArgs e)
        {
            if (!IsCreateFolder)
            {
                var dictionary = (Dictionary)BindingContext;
                if (!String.IsNullOrEmpty(dictionary.Name))
                {
                    dictionary.Id = 0;
                    App.Db.CreateDictionary(dictionary);
                    await Navigation.PopAsync();
                }
            }
            else
            {
                string folderPathCreate = DependencyService.Get<IFileWorker>().CreateFolder(EnterName.Text, null, rootPath);
                await Navigation.PopAsync();
                //обновим список файлов.папок
                await chFilePage.UpdateFileList(false, rootPath);
                if(string.IsNullOrEmpty(folderPathCreate))
                     await chFilePage.ShowInfoMessage("При создании каталога произошла ошибка!");
                else
                    if(folderPathCreate=="exist")
                        await chFilePage.ShowInfoMessage("Каталог с таким именем уже существует!");
                    else
                        await chFilePage.ShowInfoMessage("Новый каталог создан!");

            }
           
        }
    }
}