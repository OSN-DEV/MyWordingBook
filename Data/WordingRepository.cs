﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLib.File;
using System.Collections.ObjectModel;

namespace MyWordingBook.Data {
    /// <summary>
    /// operate wording model
    /// </summary>
    internal class WordingRepository {
        #region Declaration
        private string _filePath;
        private string _fileName;

        private const char Separator = '\t';
        internal ObservableCollection<WordingModel> _searchDataContext = new ObservableCollection<WordingModel>();
        #endregion

        #region Public Property
        /// <summary>
        /// data file path
        /// </summary>
        internal string DataFile {
            set {
                this._filePath = value;
                this._fileName = new FileOperator(value).Name;
            }
            get { return this._filePath; }
        }

        /// <summary>
        /// data file name
        /// </summary>
        internal string FileName {
            get { return this._fileName; }
        }

        /// <summary>
        /// data list
        /// </summary>
        internal ObservableCollection<WordingModel> DataContext { private set; get; } = new ObservableCollection<WordingModel>();

        public bool IsSearchMode { private set; get; } = false;
        #endregion

        #region Public Method
        /// <summary>
        /// load data file
        /// </summary>
        /// <returns>true:success, false:otherwise</returns>
        internal bool Load() {
            bool result = false;
            this.DataContext.Clear();

            try {
                using (var file = new FileOperator(this._filePath)) {
                    file.OpenForRead();
                    while (!file.Eof) {
                        var item = file.ReadLine().Split(Separator);
                        Array.Resize(ref item, 2);
                        this.DataContext.Add(new WordingModel() {
                            Word = item[0],
                            Note = item[1]
                        });
                    }
                    result = true;
                }
            } catch {
            }

            return result;
        }

        /// <summary>
        /// sort by word
        /// </summary>
        internal void Sort() {
            this.Copy(new ObservableCollection<WordingModel>(this.DataContext.OrderBy(n => n.Word)));
        }

        /// <summary>
        /// save data file
        /// </summary>
        /// <returns>true:success, false:otherwise</returns>
        internal bool Save() {
            bool result = false;
            using (var file = new FileOperator(this._filePath)) {
                try {
                    file.Delete();
                    file.OpenForWrite();
                    foreach (var model in this.DataContext) {
                        file.WriteLine(model.Word + Separator + model.Note);
                    }
                    result = true;
                } catch {
                }
            }
            return result;
        }

        /// <summary>
        /// delete item and save
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal bool Delete(WordingModel model) {
            bool result = false;
            using (var file = new FileOperator(this._filePath)) {
                try {
                    this.DataContext.Remove(model);
                    if (this.IsSearchMode) {
                        this._searchDataContext.Remove(model);
                    }
                    result = this.Save();
                } catch {
                }
            }
            return result;
        }

        /// <summary>
        /// add wording model
        /// </summary>
        /// <param name="model"></param>
        internal void Add(WordingModel model) {
            this.DataContext.Add(model);
        }

        /// <summary>
        /// start search mode
        /// </summary>
        /// <param name="searchWord">search keyword</param>
        /// <returns>search result</returns>
        internal ObservableCollection<WordingModel> StartSearchMode(string searchWord) {
            this.IsSearchMode = true;
            this._searchDataContext.Clear();

            foreach (var model in this.DataContext) {
                if (-1 < model.Word.IndexOf(searchWord) || -1 < model.Note.IndexOf(searchWord)) {
                    this._searchDataContext.Add(model);
                }
            }
            return this._searchDataContext;
        }

        /// <summary>
        /// stop search mode
        /// </summary>
        internal void EndSearchMode() {
            this.IsSearchMode = false;
            this._searchDataContext.Clear();
        }
        #endregion

        #region Private Method
        /// <summary>
        /// deep copy
        /// </summary>
        /// <param name="srcCollection">source model</param>
        private void Copy(ObservableCollection<WordingModel> srcCollection) {
            this.DataContext.Clear();
            foreach (var src in srcCollection) {
                this.DataContext.Add(((WordingModel)src).Clone());
            }
        }
        #endregion
    }
}
