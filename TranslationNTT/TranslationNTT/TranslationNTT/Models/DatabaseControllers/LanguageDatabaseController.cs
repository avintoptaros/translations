using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Collections.Generic;
using System.Linq;
using TranslationNTT.Data;
using Xamarin.Forms;

namespace TranslationNTT.Models.DatabaseControllers
{
    public class LanguageDatabaseController
    {
        protected static object locker = new object();
        protected SQLiteConnection database;

        private static LanguageDatabaseController _instance;
        public static LanguageDatabaseController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LanguageDatabaseController();
                    _instance.Instantiate();
                }

                return _instance;
            }
        }

        private void Instantiate()
        {
            _instance.database = DependencyService.Get<ISQLiteManager>().GetConnection();
            _instance.database.CreateTable<Language>();
        }

        public List<Language> GetAll()
        {
            lock (locker)
            {
                if (database.Table<Language>().Count() == 0)
                {
                    return null;
                }
                else
                {
                    List<Language> languages = database.Table<Language>().ToList();
                    return languages;
                }
            }
        }


        public List<Language> GetAllWithChildren()
        {
            lock (locker)
            {
                if (database.Table<Language>().Count() == 0)
                {
                    return null;
                }
                else
                {
                    List<Language> languages = database.GetAllWithChildren<Language>();
                    return languages;
                }
            }
        }

        public Language GetById(int id)
        {
            lock (locker)
            {
                if (database.Table<Language>().Count() == 0)
                {
                    return null;
                }
                else
                {
                    return database.Table<Language>().FirstOrDefault(language => language.Id.Equals(id));
                }
            }
        }

        public Language GetByCulture(string culture)
        {
            lock (locker)
            {
                if (database.Table<Language>().Count() == 0)
                {
                    return null;
                }
                else
                {
                    return database.GetAllWithChildren<Language>(l => l.Culture.Equals(culture)).FirstOrDefault();
                    //return database.Table<Language>().Where(language => language.Culture.Equals(culture)).FirstOrDefault();
                }
            }
        }

        public int Save(Language language)
        {
            lock (locker)
            {
                if (language.Id != 0)
                {
                    return database.Update(language);
                }
                return database.Insert(language);
            }
        }

        public void UpdateWithChildren(Language language, Word word)
        {
            lock (locker)
            {
                if (language.Id != 0)
                {
                    language.Words.Add(word);
                    database.UpdateWithChildren(language);
                }
            }
        }

        public int Delete(int id)
        {
            lock (locker)
            {
                return database.Delete<Language>(id);
            }
        }

        public void DeleteAll()
        {
            lock (locker)
            {
                database.DeleteAll<Language>();
            }
        }

    }
}
