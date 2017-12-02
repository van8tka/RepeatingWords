using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RepeatingWords.Model;
using Xamarin.Forms;
using System.IO;


namespace RepeatingWords.Services
{
    public class GoogleServices
    {
        public static readonly string ClientId = "197917761798-ult3h0vsuhsbc193nmt1p86m2bs13ts5.apps.googleusercontent.com";
        public static readonly string ClientSecret = "KaJvcbueYKPe8ged1YqL8sH8";
        public static readonly string RedirectUrl = "https://drive.google.com/drive/u/0/my-drive";

        //для определения правильности загруженного файла и тудпроверка и назад
        const string boundary = "-657840085769204770684-";
       



        /// <summary>
        /// Получение токена авторизации для дальнейших манипуляций с аккаунтом
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>

        public static async Task<string> GetAccessTokenAsync(string code)
        {
            string accessToken = string.Empty;
            try
            {
               var requestUrl =
              "https://www.googleapis.com/oauth2/v4/token"
              + "?code=" + code
                + "&redirect_uri=" + RedirectUrl
                + "&client_id=" + ClientId
                + "&client_secret=" + ClientSecret
                + "&scope="
               + "&grant_type=authorization_code";
                HttpClient client = new HttpClient();
                var response = client.PostAsync(requestUrl, null).Result;
                string json = await response.Content.ReadAsStringAsync();
                accessToken = JsonConvert.DeserializeObject<JObject>(json).Value<string>("access_token");
                return accessToken;

            }
            catch (Exception er)
            {
                ErrorHandler.getError(er, "GoogleServices.GetAccessTokenAsync");
                return accessToken;
            }
        }






        //создание папки в GoogleDirive
        internal static async Task<string> CreateFolderInGDAsync(string folderName, string accessToken)
        {
            try
            {
                string IdBackUpFolder = GetFolderIdIfExist(folderName, accessToken);
                if (string.IsNullOrEmpty(IdBackUpFolder))
                {
                    string url = "https://www.googleapis.com/drive/v2/files";
                    HttpClient client = new HttpClient();
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.Method = HttpMethod.Post;
                    request.Headers.Add("Authorization", "Bearer " + accessToken);
                    request.RequestUri = new Uri(url);

                    JObject data = new JObject();
                    data.Add("name", folderName);
                    data.Add("mimeType", "application/vnd.google-apps.folder");

                    HttpContent content = new StringContent(data.ToString(), Encoding.UTF8, "application/json");
                    request.Content = content;

                    HttpResponseMessage response = client.SendAsync(request).Result;
                    bool succes = response.IsSuccessStatusCode;
                    if (succes)
                    {
                        string js = response.Content.ReadAsStringAsync().Result;
                        IdBackUpFolder = (JObject.Parse(js))["id"].ToString();
                        return IdBackUpFolder;
                    }
                    else
                        return string.Empty;
                }
                else
                    return IdBackUpFolder;

            }
           catch(Exception er)
            {
                ErrorHandler.getError(er, "GoogleServices.CreateFolderInGDAsync");
                return string.Empty;
            }
          
        }



        //загрузка файла на GoogleDrive

        internal static async Task<bool> UploadFileBackupToGDAsync(string idFolderCreate, string accessToken, string filenameBackup)
        {
            try
            {
                string url = "https://www.googleapis.com/upload/drive/v2/files/";
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.Headers.Add("Authorization", "Bearer " + accessToken);
                request.RequestUri = new Uri(url);
                JObject contentJo = new JObject();
                contentJo.Add("title",filenameBackup);
                contentJo.Add("mimeType", "mime/type");
                contentJo.Add("description","This file is backup of Database in CardsOfWords aplication");
                HttpContent content = new StringContent(contentJo.ToString(), Encoding.UTF8, "application/json");
                request.Content = content;
                var respon = client.SendAsync(request).Result;
                if (respon.IsSuccessStatusCode)
                {
                    string result = respon.Content.ReadAsStringAsync().Result;
                    JObject jo = JObject.Parse(result);
                    string idfileCreate = jo["id"].ToString();
                    string filePathDb = App.Db.DBConnection.DatabasePath;
                    byte[] bytesFile = DependencyService.Get<IFileWorker>().GetByteArrayOfBackUpFile(filePathDb);
                    //Byte[] bytes = File.ReadAllBytes(filePathDb);
                    string fileBase64 = Convert.ToBase64String(bytesFile);
                    string fileBase64WithBoundary = boundary + fileBase64 + boundary;
                    //And correspondingly, read back to file:
                    //Byte[] bytes = Convert.FromBase64String(b64Str);
                    //File.WriteAllBytes(path, bytes);
                    client = new HttpClient();
                    url = "https://www.googleapis.com/upload/drive/v2/files/" + idFolderCreate+"?fileId=" +idfileCreate+ "&uploadType=multipart";
                    request = new HttpRequestMessage();
                    request.RequestUri = new Uri(url);
                    request.Method = HttpMethod.Put;
                    //request.Headers.Add("Content-Type", "multipart/mixed");
                    request.Headers.Add("Authorization", "Bearer " + accessToken);
                    content = new StringContent(fileBase64WithBoundary);
                    request.Content = content;
                    var succes = await client.SendAsync(request);
                    return succes.IsSuccessStatusCode;
                 }
                else
                    return false;
            }
            catch(Exception er)
            {
                ErrorHandler.getError(er, "GoogleServices.UploadFileBackupToGDAsync");
                return false;
            }
        }




        //получение ID существующей папки и если она есть проверка удалена она или нет(в корзине)
        private static string GetFolderIdIfExist(string folderName, string accessToken)
        {
            try
            {
                string idFolder = string.Empty;
                //https://www.googleapis.com/drive/v2/files/root/children?orderBy=createdDate&q=title='CardsOfWordsBackup'
                string url = string.Format("https://www.googleapis.com/drive/v2/files/root/children?orderBy=createdDate&q=title='"+folderName+"'");
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Get;
                request.Headers.Add("Authorization", "Bearer " + accessToken);
                request.RequestUri = new Uri(url);
                var response = client.SendAsync(request).Result;
                JObject foldersJo = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                string stItems = foldersJo["items"].ToString();
                if(!string.IsNullOrEmpty(stItems))
                {
                    var jA = JArray.Parse(stItems);
                  
                    if (jA.Count>0)
                    {
                        idFolder = jA.Last["id"].ToString();
                        //проверим не удалена ли папка("explicitlyTrashed": false, )
                        string urlF = "https://www.googleapis.com/drive/v2/files/" + idFolder;
                        client = new HttpClient();
                        request = new HttpRequestMessage();
                        request.Method = HttpMethod.Get;
                        request.Headers.Add("Authorization", "Bearer " + accessToken);
                        request.RequestUri = new Uri(urlF);
                        var responseF = client.SendAsync(request).Result;
                        var foldersJoF = JObject.Parse(responseF.Content.ReadAsStringAsync().Result);
                        bool isTrashedFolder = (bool)foldersJoF["explicitlyTrashed"];
                        if (isTrashedFolder)
                            idFolder = string.Empty;
                    }
                }
                //получим папку CardsOfWordsBackup и если их несколько то последнюю                   
                return idFolder;
            }
            catch(Exception er)
            {
                ErrorHandler.getError(er, "GoogleServices.GetFolderIdIfExist");
                return null;
            }
        }


       

    }
}
