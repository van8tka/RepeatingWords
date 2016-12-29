using System.Collections.Generic;
using System.Linq;
using SQLite;
using Xamarin.Forms;

namespace RepeatingWords.Model
{
    public class WordRepositiry
    {
      

        SQLiteConnection database;
        public WordRepositiry(SQLiteConnection database)
        {
            this.database = database;
            //инициализация бд
        }

        public IEnumerable<Words> GetWords(int iddiction)
        {
            return (from i in database.Table<Words>().Where(z=>z.IdDictionary==iddiction) select i).ToList();
        }
        public Words GetWord(int id)
        {
            return database.Get<Words>(id);
        }
        public int DeleteWord(int id)
        {
            return database.Delete<Words>(id);
        }

        public int DeleteWords(int iddiction)
        {
            IEnumerable<Words> ListWords = GetWords(iddiction);
            foreach (var i in ListWords)
            {
                DeleteWord(i.Id);
            }
            return 1;
        }
        public int CreateWord(Words item)
        {
            if (item.Id == 0)
                return database.Insert(item);
            else
            {
                database.Update(item);
                return item.Id;
            }

        }
    }
}
