using System.Collections.Generic;
using System.Linq;
using System.IO;
using Xamarin.Forms;
using System.Threading.Tasks;

[assembly: Dependency(typeof(RepeatingWords.Droid.FileWorker))]

namespace RepeatingWords.Droid
{
    public class FileWorker : IFileWorker
    {



        public Task<IEnumerable<string>> GetFilesAsync()
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

       

        public async Task<List<string>> LoadTextAsync(string filename)
        {
            string filepath = GetFilePath(filename);
            using (StreamReader reader = File.OpenText(filepath))
            {
                List<string> lines = new List<string>();
                string line;
                while((line=reader.ReadLine())!=null)
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
            //�������� ������ ������ ��������(����� ���������� �� ���� �����������)
            string g = Android.OS.Environment.ExternalStorageDirectory.ToString();          
            return g;
        }

    }
}