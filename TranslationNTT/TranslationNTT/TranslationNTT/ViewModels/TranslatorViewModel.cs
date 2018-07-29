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

        private ObservableCollection<Word> _words = new ObservableCollection<Word>();
        public ObservableCollection<Word> Words
        {
            get { return _words; }
            set
            {
                _words = value;
                OnPropertyChanged("Words");
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
                if (_suggestionField != null)
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

        public TranslatorViewModel()
        {
            // check if we have the translations saved into database. If not, save them
            // we need a notification system if the translations file was changed
            ReadTranslationsFile();
        }

        public void GetTranslations()
        {
            Word word = WordDatabaseController.Instance.GetByValue(SuggestionField);
            if (word == null)
            {
                SuggestionFieldColor = Color.Red;
                return;
            }
            SuggestionFieldColor = Color.Green;
            SelectedLanguage = "Selected language: " + LanguageDatabaseController.Instance.GetById(word.LanguageId).Name;
            Words = new ObservableCollection<Word>(WordDatabaseController.Instance.GetTranslations(SuggestionField));
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
