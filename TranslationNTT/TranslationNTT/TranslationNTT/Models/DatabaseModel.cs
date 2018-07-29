using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranslationNTT.Models
{
    public class DatabaseModel
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }
    }
}
