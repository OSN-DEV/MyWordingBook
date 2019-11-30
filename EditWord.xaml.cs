using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyWordingBook.Data;
using System.Windows.Input;

namespace MyWordingBook {
    /// <summary>
    /// Add or edit word.
    /// </summary>
    public partial class EditWord : Window {

        #region Public Property
        public WordingModel Model { private set; get; }
        #endregion

        #region Constructor
        public EditWord() {
            InitializeComponent();
        }

        public EditWord(Window owner, WordingModel model) {
            InitializeComponent();
            this.Initialize(owner, model);
        }
        #endregion

        #region Event
        /// <summary>
        /// close window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e) {
            InputMethod.Current.ImeState = InputMethodState.Off;
        }

        /// <summary>
        /// ok button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Click(object sender, RoutedEventArgs e) {
            this.Model.Word = this.cWord.Text;
            this.Model.Note = this.cNote.Text;
            this.DialogResult = true;
        }

        /// <summary>
        /// change file url. update icon file if need.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Word_TextChanged(object sender, TextChangedEventArgs e) {
            this.cOK.IsEnabled = (0 < this.cWord.Text.Length);
        }
        #endregion

        #region Private Method
        /// <summary>
        /// initialize window
        /// </summary>
        /// <param name="model"></param>
        private void Initialize(Window owner,WordingModel model) {
            this.Owner = owner;
            this.Model = (null == model) ? new WordingModel() : model.Clone();
            this.cWord.Text = this.Model.Word;
            this.cNote.Text = this.Model.Note;
            this.cOK.IsEnabled = (0 < this.Model.Word.Length);
            this.cWord.Focus();

        }
        #endregion

    }
}
