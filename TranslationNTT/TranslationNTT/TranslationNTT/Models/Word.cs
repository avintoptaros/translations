using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranslationNTT.Models
{
    public sealed class Word : DatabaseModel
    {
        public string WordValue { get; set; }

        [ForeignKey(typeof(Language))]
        public int LanguageId { get; set; }
        //public int WordId { get; set; }

        [ManyToOne]
        public Language Language { get; set; }

        [ManyToMany(typeof(WordTranslate))]
        public List<Word> Words { get; set; } = new List<Word>();

        public override string ToString()
        {
            return WordValue;
        }
    }
}
