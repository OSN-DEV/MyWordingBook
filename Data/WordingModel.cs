using System.ComponentModel;
using MyWordingBook.Component;

namespace MyWordingBook.Data {
    /// <summary>
    /// wording model
    /// </summary>
    public class WordingModel : INotifyPropertyChanged {

        #region Declaration
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Public Property
        private string _word = "";
        public string Word {
            get => this._word;
            set {
                if (PropertyChanged.RaiseIfSet(() => Word, ref _word, value))
                    PropertyChanged.Raise(() => _word);
            }
        }

        private string _note = "";
        public string Note {
            get => this._note;
            set {
                if (PropertyChanged.RaiseIfSet(() => Note, ref _note, value))
                    PropertyChanged.Raise(() => _note);
            }
        }

        // use only search mode
        public int Index { set; get; }
        #endregion

        #region Public Method
        /// <summary>
        /// clone instance
        /// </summary>
        /// <returns></returns>
        public WordingModel Clone() {
            var model = new WordingModel();
            model.Word = this.Word;
            model.Note = this.Note;
            model.Index = this.Index;
            return model;
        }
        #endregion

        #region Protecte Method
        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

    }
}
