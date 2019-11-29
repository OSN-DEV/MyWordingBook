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
using MyWordingBook.Data;
using MyWordingBook.Util;

namespace MyWordingBook {
    /// <summary>
    /// main window
    /// </summary>
    public partial class MainWindow : Window {
        #region Declaration
        private AppRepository _settings;
        #endregion


        #region Constructor
        public MainWindow() {
            InitializeComponent();
            this.Initialize();
        }
#endregion


        #region Event   
        /// <summary>
        /// window keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            switch(e.Key) {
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
            if (0<= pos) {
                this.Left = pos;
            }
            pos = Common.GetWindowPosition(this._settings.Pos.Y, this._settings.Size.W, SystemParameters.VirtualScreenWidth);
            if (0 <= pos) {
                this.Top = pos;
            }

            // restore window size
            this.Width = Common.GetWindowSize(this.Width, this._settings.Size.W, SystemParameters.WorkArea.Width);
            this.Height = Common.GetWindowSize(this.Height, this._settings.Size.H, SystemParameters.WorkArea.Height);
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
        #endregion
    }
}
