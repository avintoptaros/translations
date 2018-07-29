using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TranslationNTT.Data;
using Xamarin.Forms;

namespace TranslationNTT.Models.DatabaseControllers
{
    class WordTranslateDatabaseController
    {
        protected static object locker = new object();
        protected SQLiteConnection database;

        private static WordTranslateDatabaseController _instance;
        public static WordTranslateDatabaseController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WordTranslateDatabaseController();
                    _instance.Instantiate();
                }

                return _instance;
            }
        }

        private void Instantiate()
        {
            _instance.database = DependencyService.Get<ISQLiteManager>().GetConnection();
            _instance.database.CreateTable<WordTranslate>();
        }

        public void Save(WordTranslate wordTranslate)
        {
            lock (locker)
            {
                if (wordTranslate.Id != 0)
                {
                    database.Update(wordTranslate);
                }
                database.Insert(wordTranslate);
            }
        }

        public void DeleteAll()
        {
            lock (locker)
            {
                database.DeleteAll<WordTranslate>();
            }
        }
    }
}
