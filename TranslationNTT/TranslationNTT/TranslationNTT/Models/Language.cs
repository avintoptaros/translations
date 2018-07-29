using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranslationNTT.Models
{
    public class Language : DatabaseModel
    {
        public string Name { get; set; }
        [MaxLength(8), Unique]
        public string Culture { get; set; }

    }
}
