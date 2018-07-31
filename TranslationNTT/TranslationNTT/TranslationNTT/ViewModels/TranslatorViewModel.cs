using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using TranslationNTT.Models;
using TranslationNTT.Models.DatabaseControllers;
using Xamarin.Forms;

namespace TranslationNTT.ViewModels
{
    public class TranslatorViewModel : INotifyPropertyChanged
    {

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged("SelectedLanguage");
            }
        }

        private ObservableCollection<Word> _translatedWords = new ObservableCollection<Word>();
        public ObservableCollection<Word> TranslatedWords
        {
            get { return _translatedWords; }
            set
            {
                _translatedWords = value;
                TranslationNotFound = _translatedWords.Count == 0;
                OnPropertyChanged("TranslatedWords");
            }
        }

        private ObservableCollection<Word> _matchedWords = new ObservableCollection<Word>();
        public ObservableCollection<Word> MatchedWords
        {
            get { return _matchedWords; }
            set
            {
                _matchedWords = value;
                OnPropertyChanged("MatchedWords");
            }
        }

        private string _suggestionField;
        public string SuggestionField
        {
            get { return _suggestionField; }
            set
            {
                _suggestionField = value;
                if (_suggestionField != null && _suggestionField != string.Empty)
                {
                    MatchedWords = new ObservableCollection<Word>(WordDatabaseController.Instance.GetByMatch(_suggestionField));
                }
                
                OnPropertyChanged("SuggestionField");
            }
        }

        private Color _suggestionFieldColor = Color.BurlyWood;
        public Color SuggestionFieldColor
        {
            get { return _suggestionFieldColor; }
            set
            {
                _suggestionFieldColor = value;
                OnPropertyChanged("SuggestionFieldColor");
            }
        }

        private bool _translationNotFound = false;
        public bool TranslationNotFound 
        {
            get { return _translationNotFound; }
            set {
                _translationNotFound = value;
                OnPropertyChanged("TranslationNotFound");
            }
        }

        public TranslatorViewModel()
        {
            // check if we have the translations saved into database. If not, save them
            // we need a notification system if the translations file was changed
            if (!WordDatabaseController.Instance.HasRecords())
            {
                ReadTranslationsFile();    
            }

        }

        public void GetTranslations()
        {
            Word word = WordDatabaseController.Instance.GetByValue(SuggestionField);
            if (word == null)
            {
                SelectedLanguage = string.Empty;
                SuggestionFieldColor = Color.Red;
                TranslationNotFound = false;
                TranslatedWords.Clear();
                return;
            }
            SuggestionFieldColor = Color.Green;
            SelectedLanguage = "Selected language: " + LanguageDatabaseController.Instance.GetById(word.LanguageId).Name;
            if (word.Words != null)
            {
                TranslatedWords = new ObservableCollection<Word>(word.Words);    
            }
        }

        private async void ReadTranslationsFile()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(TranslatorViewModel)).Assembly;
            using (var stream = assembly.GetManifestResourceStream("TranslationNTT.Assets.Intelligent_translator.xml"))
            {
                await Task.Factory.StartNew(delegate
                {
                    var serializer = new XmlSerializer(typeof(TranslatorParser));
                    TranslatorParser translatorParser = (TranslatorParser)serializer.Deserialize(stream);
                    translatorParser.ParseElements();
                    FillWithData();
                });
            }
                
        }

        private void FillWithData()
        {
            // fill the words list from database
            //Words = new ObservableCollection<Word>(WordDatabaseController.Instance.GetAll());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
