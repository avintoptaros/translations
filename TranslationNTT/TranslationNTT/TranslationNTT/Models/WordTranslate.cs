using SQLiteNetExtensions.Attributes;

namespace TranslationNTT.Models
{
    class WordTranslate : DatabaseModel
    {
        [ForeignKey(typeof(Word), Name = "Id")]
        public int WordId { get; set; }
        [ForeignKey(typeof(Word), Name = "Id")]
        public int ParentWordId { get; set; }


    }
}
