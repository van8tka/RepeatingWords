using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Xamarin.Forms;
using System.Threading.Tasks;
using Windows.Storage;

[assembly: Dependency(typeof(RepeatingWords.UWP.FileWorker))]

namespace RepeatingWords.UWP
{
    public class FileWorker : IFileWorker
    {
        public string GetDocsPath()
        {
            //получаем корень памяти телефона(может отличаться на разл устройствах)
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            return localFolder.Name;
        }

        public async Task<IEnumerable<string>> GetFilesAsync()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            IEnumerable<string> filenames = from filepath in await localFolder.GetFilesAsync() select filepath.Name;
            List<string> fileTxt = new List<string>();

            int z = 0;
            //фильтруем только txt файлы
            foreach (var i in filenames)
            {
                if (i.EndsWith(".txt"))
                {
                    fileTxt.Add(filenames.ElementAt(z));
                }
                z++;
            }

            filenames = (IEnumerable<string>)fileTxt;

            return filenames;
        }



        public async Task<List<string>> LoadTextAsync(string filename)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            // получаем файл
            StorageFile filepath = await localFolder.GetFileAsync(filename);

            IList<string> lines = await FileIO.ReadLinesAsync(filepath);
             
                return (List<string>)lines;
          
        }

    }
}