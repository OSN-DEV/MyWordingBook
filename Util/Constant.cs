using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLib.Util;

namespace MyWordingBook.Util {
    /// <summary>
    /// 
    /// </summary>
    internal class Constant {
        /// <summary>
        /// setting files
        /// </summary>
        public static readonly string SettingFile = Common.GetAppPath() + @"app.settings";

        /// <summary>
        /// use file dialog
        /// </summary>
        public static readonly string FilterName = "Wording Data";
        public static readonly string FilterExt = "*.mwb";
        public static readonly string NewFile = "wording.mwb";

    }
}
