using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepeatingWords
{
    public interface IFileWorker
    {
        //string GetDocsPath();//путь к папке

        Task<List<string>> LoadTextAsync(string filepath);//загр текста из файла

        //Task<IEnumerable<string>> GetFilesAsync();//получение файлов из опредго каталога
        Task<IEnumerable<string>> GetFilesAsync(string folderPath);//получение файлов из указанного каталога

        string CreateFolder(string folderName, string fileName=null, string filePath=null);// создание папки 

        bool WriteFile(string filePathsource, string filepathDest);//запись файла

        Task<string> GetBackUpFilesAsync(string folder);//получение файла бэкапа из опредго каталога

        bool IsFile(string path);//для определения папка или файл
    }
}
