using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;
using TranslationNTT.Models.DatabaseControllers;

namespace TranslationNTT.Models
{
    [XmlRoot(ElementName = "TRANSLATIONS")]
    public class TranslatorParser
    {
        [XmlElement(ElementName = "RECORD")]
        public Record[] Records { get; set; }

        public void ParseElements()
        {
            LanguageDatabaseController.Instance.DeleteAll();
            WordDatabaseController.Instance.DeleteAll();

            foreach (Record record in Records)
            {
                if (!Save(record))
                {
                    // we had a problem saving the first node, skip the childrens and check the logs
                    // don't show an error to the user, because this list is provided or it can be downloaded over an API
                    continue;
                }
                foreach (Record link in record.Links)
                {
                    if (!Save(link, record))
                    {
                        // we didn't saved the children, check the logs
                        // don't show an error to the user, because this list is provided or it can be downloaded over an API
                    }
                }
            }
        }

        private bool Save(Record record, Record parentRecord = null)
        {
            try
            {
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo(record.Culture);
                // check if language exists, if not, save it to database
                Language language = LanguageDatabaseController.Instance.GetByCulture(cultureInfo.Name);
                if (language == null)
                {
                    language = new Language();
                    language.Culture = record.Culture;
                    language.Name = cultureInfo.DisplayName;
                    LanguageDatabaseController.Instance.Save(language);
                }                

                // Save word
                Word word = new Word();
                word.LanguageId = language.Id;
                word.WordValue = record.Word;

                if (parentRecord != null)
                {
                    // it's a translation, so get the parent word
                    Word parentWord = WordDatabaseController.Instance.GetByValue(parentRecord.Word);
                    if (parentWord != null)
                    {
                        word.WordId = parentWord.Id;
                    }
                }

                WordDatabaseController.Instance.Save(word);

                return true;
            }
            catch (ArgumentNullException ex)
            {
                // we need to treat exception, log it into a file
                return false;
            }
            catch (CultureNotFoundException ex)
            {
                // we need to treat exception, log it into a file
                return false;
            }
            catch (SQLite.SQLiteException ex)
            {
                // we need to treat exception, maybe it's an unique constraint or another exception
                return false;
            }
        }
    }

    [XmlRoot(ElementName = "RECORD")]
    public class Record
    {
        [XmlAttribute(AttributeName = "word")]
        public string Word { get; set; }
        [XmlAttribute(AttributeName = "culture")]
        public string Culture { get; set; }
        [XmlElement(ElementName = "LINK")]
        public Record[] Links { get; set; }
    }

    [XmlRoot(ElementName = "LINK")]
    public class Link : Record
    { 
    }

}
