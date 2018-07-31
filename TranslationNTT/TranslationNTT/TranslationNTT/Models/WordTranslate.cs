using SQLiteNetExtensions.Attributes;

namespace TranslationNTT.Models
{
    public class WordTranslate : DatabaseModel
    {
        [ForeignKey(typeof(Word))]
        public int WordId { get; set; } = 0;
        [ForeignKey(typeof(Word))]
        public int ParentWordId { get; set; } = 0;
    }
}
