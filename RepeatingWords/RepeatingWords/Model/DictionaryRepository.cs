using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using Xamarin.Forms;


namespace RepeatingWords.Model
{
    public class DictionaryRepository
    {
       

        SQLiteConnection database;
        SQLiteAsyncConnection asyncdatabase;
        public DictionaryRepository(string filename)
        {
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(filename);
            database = new SQLiteConnection(databasePath);
            asyncdatabase = new SQLiteAsyncConnection(databasePath);
            DBConnection = database;
            DBConnectionAsync = asyncdatabase;
            database.CreateTable<Dictionary>();
            database.CreateTable<Words>();
            database.CreateTable<LastAction>();         
        }


        public SQLiteAsyncConnection DBConnectionAsync { get; }

        public SQLiteConnection DBConnection { get; }
      

        public IEnumerable<Dictionary> GetDictionarys()
        {
            return (from i in database.Table<Dictionary>() select i).ToList();
        }
        public Dictionary GetDictionary(int id)
        {
            return database.Get<Dictionary>(id);
        }
        public int DeleteDictionary(int id)
        {
            return database.Delete<Dictionary>(id);
        }
        public int CreateDictionary(Dictionary item)
        {
            return database.Insert(item);
        }
    }
}
