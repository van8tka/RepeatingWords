using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using RepeatingWords.Model;

namespace RepeatingWords
{
    public class OnlineDictionaryService
    {
        const string Url = "http://devprogram.ru/api/data/";
        const string UrlLang = "http://devprogram.ru/api/langdata/";
        //настройки клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }
        //получаем список словарей
        public async Task<IEnumerable<Dictionary>>Get()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IEnumerable<Dictionary>>(result);
        }
        //получаем список слов выбранного словаря
        public async Task<IEnumerable<Words>> Get(int idDict)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url+idDict.ToString());
            return JsonConvert.DeserializeObject<IEnumerable<Words>>(result);
        }
       //---------------------------обновление с выбором языковой среды-------------------------
       //получаем список языков
       public async Task<IEnumerable<Language>>GetLanguage()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(UrlLang);
            return JsonConvert.DeserializeObject<IEnumerable<Language>>(result);
        }
        // получаем список словарей выбранного языка
        public async Task<IEnumerable<Dictionary>>GetLanguage(int idLang)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(UrlLang + idLang);
            return JsonConvert.DeserializeObject<IEnumerable<Dictionary>>(result);
        }
    }
}
