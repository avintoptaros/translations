
using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Collections.Generic;
using System.Linq;
using TranslationNTT.Data;
using Xamarin.Forms;

namespace TranslationNTT.Models.DatabaseControllers
{
    public class WordDatabaseController
    {
        protected static object locker = new object();
        protected SQLiteConnection database;

        private static WordDatabaseController _instance;
        public static WordDatabaseController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WordDatabaseController();
                    _instance.Instantiate();
                }

                return _instance;
            }
        }

        private void Instantiate()
        {
            _instance.database = DependencyService.Get<ISQLiteManager>().GetConnection();
            _instance.database.CreateTable<Word>();
        }


        public List<Word> GetAll()
        {
            lock (locker)
            {
                if (database.Table<Word>().Count() == 0)
                {
                    return null;
                }
                else
                {
                    List<Word> words = database.Table<Word>().ToList();
                    return words;
                }
            }
        }

        public Word GetByValue(string wordValue)
        {
            lock (locker)
            {
                if (database.Table<Word>().Count() == 0)
                {
                    return null;
                }
                else
                {
                    return database.Table<Word>().Where(word => word.WordValue.Equals(wordValue)).FirstOrDefault();
                }
            }
        }

        public List<Word> GetByMatch(string searchValue)
        {
            lock (locker)
            {
                if (database.Table<Word>().Count() == 0)
                {
                    return null;
                }
                else
                {
                    return database.Table<Word>().Where(word => word.WordValue.ToLower().Contains(searchValue.ToLower()) && word.WordId == 0).ToList();
                }
            }
        }

        public List<Word> GetTranslations(string wordValue)
        {
            lock (locker)
            {
                if (database.Table<Word>().Count() == 0)
                {
                    return null;
                }
                else
                {
                    Word parentWord = GetByValue(wordValue);
                    if (parentWord == null)
                    {
                        return null;
                    }
                    return database.Table<Word>().Where(word => word.WordId.Equals(parentWord.Id)).ToList();
                }
            }
        }

        public int Save(Word word)
        {
            lock (locker)
            {
                if (word.Id != 0)
                {
                    return database.Update(word);
                }
                return database.Insert(word);
            }
        }

        public int Delete(int id)
        {
            lock (locker)
            {
                return database.Delete<Word>(id);
            }
        }

        public void DeleteAll()
        {
            lock (locker)
            {
                database.DeleteAll<Word>();
            }
        }
    }
}
