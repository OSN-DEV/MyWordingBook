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
using MyWordingBook.Component;

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

            // create data file if file is not found or invalid.
            var dataFile = new FileOperator(this._settings.LastDataFile);
            if (!dataFile.Exists()) {
                var result = this.ShowFileDialog(false, dataFile.Name);
                if (!result.select) {
                    this.Close();
                    return;
                }
                this._settings.LastDataFile = result.fileName;
                this._settings.Save();
                dataFile.FilePath = result.fileName;
                dataFile.OpenForWrite();
                dataFile.Close();
            }

            // show wording list
            this.OpenDataFile(dataFile.FilePath);
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
                // Show Edit dialog with new mode.
                case Key.A:
                    e.Handled = true;
                    this.ShowEditDialog(null);
                    break;
                case Key.E:
                    if (this.cWordingList.SelectedItem != null) {
                        e.Handled = true;
                        this.ShowEditDialog(this.cWordingList.SelectedItem as WordingModel);
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
            // initialize variables
            this._settings = AppRepository.Init(Constant.SettingFile);
            this._wording = new WordingRepository();

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
            this.cWord.Width = (this.Width - 50) / 2;
            this.cNote.Width = this.cWord.Width;

            //
            this.cWordingList.DataContext = this._wording.DataContext;
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
        /// show file dialog
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

        /// <summary>
        /// open data file
        /// </summary>
        /// <param name="dataFile"></param>
        private void OpenDataFile(string dataFile) {
            this._wording.DataFile = dataFile;
            if (!this._wording.Load()) {
                this.ShowErrorMsg(ErrorMessages.FailToLoad);
                return;
            }

            // update title
            var fullname = typeof(App).Assembly.Location;
            var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(fullname);
            this.Title = $"MyWordingBook({info.FileVersion}) -  {this._wording.FileName}";

        }

        /// <summary>
        /// show error message
        /// </summary>
        /// <param name="message">message</param>
        private void ShowErrorMsg(string message) {
            MessageBox.Show(this, message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// show edit dialog
        /// </summary>
        /// <param name="model"></param>
        private void ShowEditDialog(WordingModel model) {
            var dialog = new EditWord(this, model);
            if (true != dialog.ShowDialog()) {
                return;
            }
            if (null ==model) {
                this._wording.Add(dialog.Model);
            } else {
                model.Word = dialog.Model.Word;
                model.Note = dialog.Model.Note;
            }
        }

        /// <summary>
        /// Get selected item model
        /// </summary>
        /// <returns>selected item</returns>
        private WordingModel GetSelectedModel() {
            if (!(this.cWordingList.GetItemAt(Mouse.GetPosition(this.cWordingList))?.DataContext is WordingModel model)) {
                return null;
            } else {
                return model;
            }
        }
        #endregion

    }
}
