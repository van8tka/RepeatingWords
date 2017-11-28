using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RepeatingWords.Model;

namespace RepeatingWords.Services
{
    public class GoogleServices
    {
        public static readonly string ClientId = "197917761798-ult3h0vsuhsbc193nmt1p86m2bs13ts5.apps.googleusercontent.com";
        public static readonly string ClientSecret = "O9nSEs6oJLj0bYKR8pYELCZI";
        public static readonly string RedirectUrl = "http://devprogram.ru";





        public static async Task<string> GetAccessTokenAsync(string code)
        {
            var requestUrl = "https://googleapi.com/ouuth2/v4/token"
                + "?code=" + code
                + "&client_id=" + ClientId
                + "&client_secret=" + ClientSecret
                + "&redirect_uri=" + RedirectUrl
                + "&grant_type=authorization_code";

            HttpClient client = new HttpClient();
            var response = await client.PostAsync(requestUrl, null);
            string json = await response.Content.ReadAsStringAsync();
            string accessToken = JsonConvert.DeserializeObject<JObject>(json).Value<string>("access_token");
            return accessToken;
        }


        //for example getuserprofile

        public static async Task<GoogleProfile> GetUserProfileAsync(string accessToken)
        {
            var requestUrl = "https://googleapi.com/plus/v1/people/me"
                + "?access_token=" + accessToken;
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(requestUrl);
            GoogleProfile googleProfile = JsonConvert.DeserializeObject<GoogleProfile>(response);
            return googleProfile;
        }

    }
}
