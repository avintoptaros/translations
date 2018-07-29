using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranslationNTT.Models
{
    public class Word : DatabaseModel
    {
        public string WordValue { get; set; }
        public int LanguageId { get; set; }
        public int WordId { get; set; }

        public override string ToString()
        {
            return WordValue;
        }
    }
}
