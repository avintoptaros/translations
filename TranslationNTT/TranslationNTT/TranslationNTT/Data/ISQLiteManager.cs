using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranslationNTT.Data
{
    public interface ISQLiteManager
    {
        SQLiteConnection GetConnection();
    }
}
