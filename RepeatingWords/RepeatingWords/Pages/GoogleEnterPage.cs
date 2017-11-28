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
        public GoogleEnterPage()
        {
            Title = "Google Drive";
            BackgroundColor = Color.White;

            var authRequest = "https://accounts.google.com/o/oauth2/v2/auth"
                + "?response_type=code&scope=openid"
                + "&redirect_uri=" + GoogleServices.RedirectUrl
                + "&client_id=" + GoogleServices.ClientId;
            var webView = new CustomWebView
            {
                Source = authRequest,
                HeightRequest = 1,               
            };


            //string ua = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:46.0) Gecko/20100101 Firefox/46.0";
            //webView.G  .getSettings().setUserAgentString(ua);

            webView.Navigated += WebViewOnNavigated;
            Content = webView;

        }

        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {
            string code = ExtractCodeFromUrl(e.Url);
            if(!string.IsNullOrEmpty(code))
            {
                string accessToken = await GoogleServices.GetAccessTokenAsync(code);
                GoogleProfile googleProfle = await GoogleServices.GetUserProfileAsync(accessToken);
            }
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







