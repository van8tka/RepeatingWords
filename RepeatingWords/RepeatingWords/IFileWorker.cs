using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepeatingWords
{
    public interface IFileWorker
    {
        string GetDocsPath();//путь к папке
        Task<List<string>> LoadTextAsync(string filename);//загр текста из файла
        Task<IEnumerable<string>> GetFilesAsync();//получение файлов из опредго каталога
        string CreateFolder(string folderName, string fileName=null);// создание папки 
        bool WriteFile(string filePathsource, string filepathDest);//запись файла
        Task<string> GetBackUpFilesAsync(string folder);//получение файла бэкапа из опредго каталога
    }
}
