using System;
using System.IO;
using jafra_xamarin.iOS.Data;
using SQLite;
using TranslationNTT.Data;
using TranslationNTT.Data.Constants;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteManager))]
namespace jafra_xamarin.iOS.Data
{
    public class SQLiteManager : ISQLiteManager
    {
        public SQLiteConnection GetConnection()
        {
            var fileName = AppConstants.DB_NAME;
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentPath, "..", "Library");
            var path = Path.Combine(libraryPath, fileName);

            var connection = new SQLiteConnection(path);
            return connection;

        }
    }
}
