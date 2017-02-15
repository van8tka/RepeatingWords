using System;
using RepeatingWords.Droid;
using System.IO;
using Xamarin.Forms;



[assembly: Dependency(typeof(SQLite_Android))]
namespace RepeatingWords.Droid
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android() { }
        public string GetDatabasePath(string filename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);          
            string path = Path.Combine(documentsPath, filename);
            return path;
        }

    }
 }