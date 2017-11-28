using System.Collections.Generic;
using System.Linq;
using System.IO;
using Xamarin.Forms;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

[assembly: Dependency(typeof(RepeatingWords.Droid.FileWorker))]

namespace RepeatingWords.Droid
{
    public class FileWorker : IFileWorker
    {
 



        public async Task<List<string>> LoadTextAsync(string filepath)
        {
            try
            {
                using (StreamReader reader = File.OpenText(filepath))
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
          catch(Exception er)
            {
                throw;
            }
        }


     
      




        //создание папки для 
        public string CreateFolder(string folderName, string fileName = null, string filePath = null)
        {
            try
            {//если не передается путь к папке где создать резервную копию
                if (string.IsNullOrEmpty(filePath))
                {//то создаем по умолчанию
                    filePath = Android.OS.Environment.ExternalStorageDirectory.ToString();
                }
                //созд путь к папке
                string pathToDir = Path.Combine(filePath, folderName);
                if (!Directory.Exists(pathToDir))
                {
                    Directory.CreateDirectory(pathToDir);
                }
                else
                {
                    if (string.IsNullOrEmpty(fileName))
                        return "exist";
                }

                //созд путь к файлу
                if (!string.IsNullOrEmpty(fileName))
                {
                    string pathToFile = Path.Combine(pathToDir, fileName);
                    return pathToFile;
                }
                else
                    return pathToDir;
            }
            catch (Exception er)
            {
                Debug.WriteLine("_____________________custom error__________" + er.Message);
                throw;
            }
        }







        //запись файла резервной копии
        public bool WriteFile(string filePathSource, string filePathDestin)
        {
            try
            {
                if (File.Exists(filePathDestin))
                    File.Delete(filePathDestin);
                File.Copy(filePathSource, filePathDestin);
                return true;
            }
            catch (Exception er)
            {                
                Debug.WriteLine("_____custom error___writefileAndroid_______" + er.Message);
                throw;
            }
        }

        //получение списка файлов бэкапа
        public Task<string> GetBackUpFilesAsync(string folder)
        {
            try
            {
                return Task.Run(() =>
                  {
                      string path = Android.OS.Environment.ExternalStorageDirectory.ToString();
                      string pathToDir = Path.Combine(path, folder);
                      if (Directory.Exists(pathToDir))
                      {
                          var list = Directory.GetFiles(pathToDir);
                          string lastFile = string.Empty;

                          DateTime tempDateTime = DateTime.MinValue;
                          foreach (var i in list)
                          {
                              var fI = new FileInfo(i);
                              var DateTimeCreate = fI.LastWriteTime;
                              if (DateTimeCreate > tempDateTime)
                              {
                                  tempDateTime = DateTimeCreate;
                                  lastFile = i;
                              }
                          }

                          return lastFile;
                      }
                      else
                          return null;
                  });

            }
            catch (UnauthorizedAccessException er)
            {
                throw;
            }
            catch (Exception er)
            {
                throw;
            }
        }


        //получение списка файлов по указанному пути
        public Task<IEnumerable<string>> GetFilesAsync(string pathFolder)
        {
            return Task.Run(() =>
            {
                try
                {
                    if(string.IsNullOrEmpty(pathFolder))
                        pathFolder = Android.OS.Environment.ExternalStorageDirectory.ToString();
                    IEnumerable<string> filenames = from filepath in Directory.EnumerateFiles(pathFolder) select Path.GetFileName(filepath);
                    return filenames;
                }
                catch (Exception er)
                {
                    throw;
                }
            });          
         }


        //isfile???
        public bool IsFile(string path)
        {
            if (File.Exists(path))
                return true;
            else
                return false;
        }
    }





}