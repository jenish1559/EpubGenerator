using EPubGenerator.ViewModel;

namespace EPubGenerator.Model
{
    public class SuggestionWordDetail : Observable
    {
        private string _suggestionWord;
        public string SuggestionWord
        {
            get { return _suggestionWord; }
            set
            {
                if (_suggestionWord == value) return;
                _suggestionWord = value;
                OnPropertyChanged();
            }
        }

        private int _computeWordResult;
        public int computeWordResult
        {
            get { return _computeWordResult; }
            set
            {
                if (_computeWordResult == value) return;
                _computeWordResult = value;
                OnPropertyChanged();
            }
        }

    }
}
