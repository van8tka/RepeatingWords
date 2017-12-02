using RepeatingWords.CustomView;
using RepeatingWords.Model;
using RepeatingWords.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.Pages
{
   public class GoogleEnterPage:ContentPage
    {
        string fileNameBackUp;
        public GoogleEnterPage(string fileNameBackUp)
        {
            this.fileNameBackUp = fileNameBackUp;
            Title = "Google Drive";
            BackgroundColor = Color.White;

            var authRequest = "https://accounts.google.com/o/oauth2/v2/auth"
               +"?redirect_uri=" + GoogleServices.RedirectUrl
               +"&prompt = consent"
               +"&response_type=code"
               + "&client_id=" + GoogleServices.ClientId
               + "&scope=https://www.googleapis.com/auth/drive"
               + "&access_type=offline";
             var webView = new CustomWebView
            {
                Source = authRequest,
                HeightRequest = 1,
            };


            webView.Navigated += WebViewOnNavigated;
            Content = webView;
        }




        private static string accessToken;


        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {
            string code = ExtractCodeFromUrl(e.Url);
            if(!string.IsNullOrEmpty(code))
            {
                accessToken = await GoogleServices.GetAccessTokenAsync(code);
                CreateBackUpInGoogleDrive(accessToken);
            }
        }



        //создадим папку в google drive
        private async void CreateBackUpInGoogleDrive(string accessToken)
        {
            string folderName = "CardsOfWordsBackup";
            string idFolderCreate = await GoogleServices.CreateFolderInGDAsync(folderName, accessToken);
            bool succes = await GoogleServices.UploadFileBackupToGDAsync(idFolderCreate,accessToken, fileNameBackUp);
            if (succes)
               await Navigation.PopAsync();
        }







        private string ExtractCodeFromUrl(string url)
        {
            if (url.Contains("code="))
            {
                var attributes = url.Split('&');
                string code = attributes.FirstOrDefault(x => x.Contains("code=")).Split('=')[1];
                return code;
            }
            else
                return string.Empty;
        }
    }
}







