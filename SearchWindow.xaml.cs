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

namespace MyWordingBook {
    /// <summary>
    /// SearchWord.xaml の相互作用ロジック
    /// </summary>
    public partial class SearchWindow : Window {

        #region Public Property
        public string SearchWord { private set; get; }
        #endregion

        #region Constructor
        public SearchWindow() {
            InitializeComponent();
        }

        public SearchWindow(Window owner, string searchWord) {
            InitializeComponent();
            this.Initialize(owner, searchWord);
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
            this.SearchWord = this.cSearchWord.Text;
            this.DialogResult = true;
        }

        /// <summary>
        /// change file url. update icon file if need.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchWord_TextChanged(object sender, TextChangedEventArgs e) {
            this.cOK.IsEnabled = (0 < this.cSearchWord.Text.Length);
        }
        #endregion

        #region Private Method
        /// <summary>
        /// initialize window
        /// </summary>
        /// <param name="owner">owner window</param>
        /// <param name="searchWord">search word</param>
        private void Initialize(Window owner, string searchWord) {
            this.Owner = owner;
            this.cSearchWord.Text = searchWord;
            this.cSearchWord.Focus();
            this.cOK.IsEnabled = (0 < this.cSearchWord.Text.Length);
        }
        #endregion
    }
}
