using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Xamarin.Forms;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Foundation.Collections;

[assembly: Dependency(typeof(RepeatingWords.UWP.FileWorker))]

namespace RepeatingWords.UWP
{
    public class FileWorker : IFileWorker
    {
        public string GetDocsPath()
        {
            //получаем корень памяти телефона(может отличаться на разл устройствах)
                      // название папки для сохранения файлов(в UWP не в корень а впапку Documents)

            string doc = "/Documents/";
            return doc;
        }

        public async Task<IEnumerable<string>> GetFilesAsync()
        {

            IReadOnlyList<StorageFile> files = await KnownFolders.DocumentsLibrary.GetFilesAsync();
         
            List<string> filesList = new List<string>();
            foreach (StorageFile file in files)
            {
                //фильтруем только txt файлы
                if (file.Name.EndsWith(".txt"))
                {
                    filesList.Add(file.Name);
                }
            }        
            return (IEnumerable<string>)filesList;
        }



        public async Task<List<string>> LoadTextAsync(string filename)
        {
            StorageFile filepath =await KnownFolders.DocumentsLibrary.GetFileAsync(filename);
            using (StreamReader reader = new StreamReader(await filepath.OpenStreamForReadAsync()))
            {
                List<string> lines = new List<string>();
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
                return lines;
            }
        }

    }
}