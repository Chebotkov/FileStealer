using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FIleStealer.ViewModels
{
    public class SelectedDriveVM : INotifyPropertyChanged
    {
        private DriveInfo selectedDrive;
        private DriveInfo availableDrive;
        private DriveInfo availableRemovableDrive;

        public ObservableCollection<DriveInfo> AvailableDrivesList { get; set; }
        public ObservableCollection<DriveInfo> SelectedDrivesList { get; set; }
        public ObservableCollection<DriveInfo> AvailableRemovableDrivesList { get; set; }

        public SelectedDriveVM()
        {
            AvailableDrivesList = Manager.GetListOfDrivers();
            SelectedDrivesList = new ObservableCollection<DriveInfo>();
            AvailableRemovableDrivesList = Manager.GetListOfRemovableDrivers();
        }

        #region Commands
        private ButtonCommand addCommand;
        private ButtonCommand removeCommand;
        private ButtonCommand startCommand;
        public ButtonCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new ButtonCommand(obj =>
                  {
                      DriveInfo drive = obj as DriveInfo;
                      if (drive != null)
                      {
                          if (!SelectedDrivesList.Contains(drive))
                          {
                              SelectedDrivesList.Add(drive);
                          }
                      }
                      SelectedDrive = drive;
                  }));
            }
        }

        public ButtonCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new ButtonCommand(obj =>
                  {
                      DriveInfo drive = obj as DriveInfo;
                      if (drive != null)
                      {
                          SelectedDrivesList.Remove(drive);
                      }
                      SelectedDrive = SelectedDrivesList.FirstOrDefault();
                  },
                  (obj) => SelectedDrivesList.Count > 0));
            }
        }

        public ButtonCommand StartScanning
        {
            get
            {
                return startCommand ??
                  (startCommand = new ButtonCommand(obj =>
                  {
                      Stealer stealer = new Stealer(AvailableRemovableDrive, SelectedDrivesList.ToArray(), WriteInfo);
                  }, 
                  (obj) =>
                  {
                      if (AvailableRemovableDrive is null) return false;
                      if (SelectedDrivesList.Count == 0) return false;
                      return true;
                  }
                  ));
            }
        }
        #endregion

        #region Properties
        public DriveInfo SelectedDrive
        {
            get { return selectedDrive; }
            set
            {
                selectedDrive = value;
                OnPropertyChanged("SelectedDrive");
            }
        }

        public DriveInfo AvailableDrive
        {
            get { return availableDrive; }
            set
            {
                availableDrive = value;
                OnPropertyChanged("AvailableDrive");
            }
        }
        public DriveInfo AvailableRemovableDrive
        {
            get { return availableRemovableDrive; }
            set
            {
                availableRemovableDrive = value;
                OnPropertyChanged("AvailableRemovableDrive");
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

        #region Necessary method
        public void WriteInfo(string info)
        {

        }
        #endregion
    }
}
