using RepeatingWords.Model;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RepeatingWords
{
    public partial class App : Application
    {

        //исходные данные для инициализации БД
        Dictionary dictInit = new Dictionary()
        {
            Id = 0,
            Name = "ExampleDictionary"
        };
         List<Words> lw = new List<Words>()
        {
           new Words() {Id=0,IdDictionary=1,RusWord="словарь", EngWord="dictionary", Transcription= "[ˈdɪkʃəneri]" },
           new Words() { Id = 0, IdDictionary = 1, RusWord = "книга", EngWord = "book", Transcription = "[bʊk]" },
           new Words() { Id = 0, IdDictionary = 1, RusWord = "стол", EngWord = "table", Transcription = "[teɪb(ə)l]" },
         };



        public const string DATABASE_NAME = "repeatwords.db";
        public static DictionaryRepository db;
       
        public static DictionaryRepository Db
        {
            get
            {
                if(db==null)
                {
                    db = new DictionaryRepository(DATABASE_NAME);
                }
                return db;
            }
        }
        public static WordRepositiry Wr = new WordRepositiry(Db.DBConnection);
        public static LastActionRepository LAr = new LastActionRepository(Db.DBConnection);
        //асинхронное соединение с БД 
        public static WordRepositiry WrAsync = new WordRepositiry(Db.DBConnectionAsync);

        public App()
        {
            InitializeComponent();
            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
                InitDb();
            MainPage = new NavigationPage(new MainPage());

        }
        //метод инициализации БД тестовыми данными
        private void InitDb()
        {
            Dictionary dict = Db.GetDictionarys().FirstOrDefault();
            if (dict == null)
            {
                Db.CreateDictionary(dictInit);
                int z = Db.GetDictionarys().FirstOrDefault().Id;
                foreach (var w in lw)
                {
                    w.IdDictionary = z;
                    Wr.CreateWord(w);
                }

            }
        }

        protected override void OnStart()
        {
            //инициализация базы данных
            InitDb();
        }
        protected override void OnSleep()
        { }
        protected override void OnResume()
        { }
    }
}
