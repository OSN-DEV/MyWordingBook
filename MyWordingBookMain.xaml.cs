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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyLib.Util;
using MyLib.File;
using MyWordingBook.Data;
using MyWordingBook.Util;

namespace MyWordingBook {
    /// <summary>
    /// main window
    /// </summary>
    public partial class MainWindow : Window {
        #region Declaration
        private AppRepository _settings;
        private WordingRepository _wording;
        #endregion


        #region Constructor
        public MainWindow() {
            InitializeComponent();
            this.Initialize();
        }
        #endregion


        #region Event   
        /// <summary>
        /// window load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e) {

            // create save data if not found.
            var dataFile = new FileOperator(this._settings.LastDataFile);
            if (!dataFile.Exists()) {
                var result = this.ShowFileDialog(false, dataFile.FilePath ?? "hoge");
                if (!result.select) {
                    this.Close();
                    return;
                }
            }
        }


        /// <summary>
        /// window keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                // Ctrl + W : Close App
                case Key.W:
                    if (Common.IsModifierPressed(ModifierKeys.Control)) {
                        e.Handled = true;
                        this.SaveWindowInfo();
                        this.Close();
                    }
                    break;
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// initialize window
        /// </summary>
        private void Initialize() {
            this._settings = AppRepository.Init(Constant.SettingFile);

            // restore window position
            double pos = Common.GetWindowPosition(this._settings.Pos.X, this._settings.Size.W, SystemParameters.VirtualScreenWidth);
            if (0 <= pos) {
                this.Left = pos;
            }
            pos = Common.GetWindowPosition(this._settings.Pos.Y, this._settings.Size.W, SystemParameters.VirtualScreenWidth);
            if (0 <= pos) {
                this.Top = pos;
            }

            // restore window size
            this.Width = Common.GetWindowSize(this.Width, this._settings.Size.W, SystemParameters.WorkArea.Width);
            this.Height = Common.GetWindowSize(this.Height, this._settings.Size.H, SystemParameters.WorkArea.Height);

            // adjust columen width
            this.cWord.Width = (this.Width - 40) / 2;
            this.cNote.Width = this.cWord.Width;
        }


        /// <summary>
        /// save window size and position
        /// </summary>
        private void SaveWindowInfo() {
            this._settings.Pos.X = this.Left;
            this._settings.Pos.Y = this.Top;
            this._settings.Size.W = this.Width;
            this._settings.Size.H = this.Height;
            this._settings.Save();
        }

        /// <summary>
        /// open file dialog
        /// </summary>
        /// <param name="isOpen">true:open dialog, false:save dialog</param>
        /// <param name="fileName">file name</param>
        /// <returns>dialog instance</returns>
        private (bool select, string fileName) ShowFileDialog(bool isOpen, string fileName) {
            var dialog = new FileDialog();
            dialog.Owner = this;
            dialog.Title = isOpen ? Titles.OpenFileDialog : Titles.SaveFileDialog;
            if (string.IsNullOrEmpty(fileName)) {
                dialog.FileName = Constant.NewFile;
            } else {
                dialog.FileName = fileName;
            }
            dialog.FilterName = Constant.FilterName;
            dialog.FilterExt = Constant.FilterExt;
            return isOpen ? dialog.ShowOpen() : dialog.ShowSave();
        }
        #endregion

    }
}
