using SQLite;

namespace RepeatingWords.Model
{
    [Table("Dictionary")]
   public class Dictionary
    {      
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Table("Words")]
    public class Words
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public int IdDictionary { get; set; }
        public string RusWord { get; set; }
        public string EngWord { get; set; }
        public string Transcription { get; set; }            
    }

    [Table("LastAction")]
    public class LastAction
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int IdDictionary { get; set; }
        public int IdWord { get; set; }
        public bool FromRus { get; set; }
    }
}
