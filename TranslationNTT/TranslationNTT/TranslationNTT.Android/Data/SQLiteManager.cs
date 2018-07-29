using System;
using System.IO;
using TranslationNTT.Droid.Data;
using SQLite;
using Xamarin.Forms;
using TranslationNTT.Data;
using TranslationNTT.Data.Constants;

[assembly: Dependency(typeof(SQLiteManager))]
namespace TranslationNTT.Droid.Data
{
    public class SQLiteManager : ISQLiteManager
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFileName = AppConstants.DB_NAME;
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFileName);
            var conn = new SQLiteConnection(path);

            return conn;

        }
    }
}
