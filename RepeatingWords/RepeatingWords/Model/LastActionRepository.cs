using SQLite;
using System.Threading.Tasks;

namespace RepeatingWords.Model
{
    public class LastActionRepository
    {
        SQLiteConnection database;

        public LastActionRepository(SQLiteConnection database)
        {
            this.database = database;
        }


        public LastAction GetLastAction()
        {
            LastAction la = (from i in database.Table<LastAction>() select i).FirstOrDefault();
            return la;
        }
        public int SaveLastAction(LastAction item)
        {
            if (GetLastAction() != null)
            {
                item.Id = GetLastAction().Id;
                database.Update(item);
                return item.Id;
            }
            else
            {
                return database.Insert(item);
            }
        }
        public void DelLastAction()
        {
            database.Delete<LastAction>(GetLastAction().Id);
        }
    }
}