using Logic;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FIleStealer.ViewModels
{
    public class SettingsVM : INotifyPropertyChanged
    {
        private string extension;
        private string newExtension;
        private string selectedCommonExtension;

        public ObservableCollection<string> Extensions { get; private set; }
        public ObservableCollection<string> CommonExtensions { get; private set; }

        public SettingsVM()
        {
            Extensions = new ObservableCollection<string>();
            CommonExtensions = new ObservableCollection<string>();
            foreach (string exts in Manager.GetUsersExtensions())
            {
                Extensions.Add(exts);
            }
            foreach (string exts in Manager.GetExtensions())
            {
                CommonExtensions.Add(exts);
            }
        }

        #region Properties
        public string Extension
        {
            get { return extension; }
            set
            {
                extension = value;
                OnPropertyChanged(nameof(Extension));
            }
        }

        public string NewExtension
        {
            get { return newExtension; }
            set
            {
                newExtension = value;
                OnPropertyChanged(nameof(NewExtension));
            }
        }

        public string SelectedCommonExtension
        {
            get { return selectedCommonExtension; }
            set
            {
                selectedCommonExtension = value;
                OnPropertyChanged(nameof(SelectedCommonExtension));
            }
        }
        #endregion

        #region Commands
        private ButtonCommand addCommand;
        private ButtonCommand addFromListCommand;
        private ButtonCommand removeCommand;

        public ButtonCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new ButtonCommand(obj =>
                  {
                      NewExtension = obj.ToString();
                      if (!String.IsNullOrEmpty(NewExtension))
                      {
                          if (!Extensions.Contains(NewExtension))
                          {
                              Extensions.Add(NewExtension);
                          }
                      }

                      extension = Extensions.FirstOrDefault();
                      NewExtension = "";
                  },
                  (obj) =>
                  {
                      return (String.IsNullOrEmpty(NewExtension));
                  }
                  ));
            }
        }

        public ButtonCommand AddFromListCommand
        {
            get
            {
                return addFromListCommand ??
                  (addFromListCommand = new ButtonCommand(obj =>
                  {
                      if (!String.IsNullOrEmpty(SelectedCommonExtension))
                      {
                          if (!Extensions.Contains(SelectedCommonExtension))
                          {
                              Extensions.Add(SelectedCommonExtension);
                          }
                      }

                      SelectedCommonExtension = CommonExtensions.FirstOrDefault();
                  },
                  (obj) =>
                  {
                      return !(String.IsNullOrEmpty(SelectedCommonExtension));
                  }
                  ));
            }
        }

        public ButtonCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new ButtonCommand(obj =>
                  {
                      if (obj != null)
                      {
                          string exst = obj.ToString();
                          if (Extensions.Contains(exst))
                          {
                              Extensions.Remove(exst);
                          }
                          extension = Extensions.FirstOrDefault();
                      }
                  },
                  (obj) =>
                  {
                      return Extensions.Count > 0;
                  }));
            }
        }
        #endregion

        #region Interface implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
