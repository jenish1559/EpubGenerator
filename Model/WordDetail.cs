using EPubGenerator.ViewModel;

namespace EPubGenerator.Model
{
    public class WordDetail : Observable
    {
        private string _incorrectWord;
        public string IncorrectWord
        {
            get { return _incorrectWord; }
            set
            {
                if (_incorrectWord == value) return;
                _incorrectWord = value;
                OnPropertyChanged();
            }
        }

        private string _correctWord;
        public string CorrectWord
        {
            get { return _correctWord; }
            set
            {
                if (_correctWord == value) return;
                _correctWord = value;
                OnPropertyChanged();
            }
        }

        private string _wordImageName;
        public string WordImageName
        {
            get { return _wordImageName; }
            set
            {
                if (_wordImageName == value) return;
                _wordImageName = value;
                OnPropertyChanged();
            }
        }

        

    }
}
