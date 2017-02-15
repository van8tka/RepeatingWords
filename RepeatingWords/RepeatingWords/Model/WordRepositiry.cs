using System.Collections.Generic;
using System.Linq;
using SQLite;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace RepeatingWords.Model
{
    public class WordRepositiry
    {
      

        SQLiteConnection database;
        SQLiteAsyncConnection asyncdatabase;

        public WordRepositiry(SQLiteConnection database)
        {
            this.database = database;
        }

        public WordRepositiry(SQLiteAsyncConnection asyncdatabase)
        {
            this.asyncdatabase = asyncdatabase;
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
        //асинхронные методы
        public async Task AsyncCreateWord(Words item)
        {
            if (item.Id == 0)
               await asyncdatabase.InsertAsync(item);
            else
            {
               await asyncdatabase.UpdateAsync(item);              
            }
        }



    }
}
