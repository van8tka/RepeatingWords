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
        public Task<IEnumerable<string>> GetFilesAsync()
        {
            try
            {
       
                IEnumerable<string> filenames = from filepath in Directory.EnumerateFiles(GetDocsPath()) select Path.GetFileName(filepath);
                List<string> fileTxt = new List<string>();

                int z = 0;
                //��������� ������ txt �����
                foreach (var i in filenames)
                {
                    if (i.EndsWith(".txt"))
                    {
                        fileTxt.Add(filenames.ElementAt(z));
                    }
                    z++;
                }

                filenames = (IEnumerable<string>)fileTxt;              
                return Task<IEnumerable<string>>.FromResult(filenames);
            }
           catch(UnauthorizedAccessException er)
           {
                return null;
           }
        }

       

        public async Task<List<string>> LoadTextAsync(string filename)
        {
            string filepath = GetFilePath(filename);
            using (StreamReader reader = File.OpenText(filepath))
            {
                List<string> lines = new List<string>();
                string line;
            while((line= await reader.ReadLineAsync())!=null)
                {
                    lines.Add(line);
                }
               return lines;
            }
        }


        private string GetFilePath(string filename)
        {
              return Path.Combine(GetDocsPath(), filename);;
        }

        //���� � ����� MyDocuments
        public string GetDocsPath()
        {
            try
            {
                //�������� ������ ������ ��������(����� ���������� �� ���� �����������)
                string g = Android.OS.Environment.RootDirectory.ToString();
                //string g = Android.OS.Environment.ExternalStorageDirectory.ToString();
                return g;
            }
            catch(Exception er)
            {
                return null;
            }
        }







        //�������� ����� ��� 
        public string CreateFolder(string folderName, string fileName=null, string filePath=null)
        {
            try
            {//���� �� ���������� ���� � ����� ��� ������� ��������� �����
                if(string.IsNullOrEmpty(filePath))
                {//�� ������� �� ���������
                    filePath = Android.OS.Environment.ExternalStorageDirectory.ToString();
                }
              //���� ���� � �����
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
                    
                //���� ���� � �����
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
                return null;
            }
        }








        public bool WriteFile(string filePathSource, string filePathDestin)
        {
            try
            {
                if (File.Exists(filePathDestin))
                    File.Delete(filePathDestin);
                 File.Copy(filePathSource, filePathDestin);
                return true;
            }
            catch(Exception er)
            {
                Debug.WriteLine("_____custom error___writefileAndroid_______" + er.Message);
                return false;
            }
        }

        //��������� ������ ������ ������
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
                        foreach(var i in list)
                        {
                            var fI = new FileInfo(i);
                            var DateTimeCreate = fI.LastWriteTime;
                            if(DateTimeCreate>tempDateTime)
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
                return null;
            }
            catch (Exception er)
            {
                return null;
            }
        }



    }
}