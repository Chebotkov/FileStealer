using Logic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FIleStealer.ViewModels
{
    public class SelectedDriveVM : INotifyPropertyChanged
    {
        private DriveInfo selectedDrive;
        private DriveInfo availableDrive;
        private DriveInfo availableRemovableDrive;
        private string chosenFile;
        private ulong countFiles;
        private ulong totalCountFiles;
        private Stealer stealer;
        private bool isScanning = false;

        public ObservableCollection<DriveInfo> AvailableDrivesList { get; private set; }
        public ObservableCollection<DriveInfo> SelectedDrivesList { get; private set; }
        public ObservableCollection<DriveInfo> AvailableRemovableDrivesList { get; private set; }
        public ObservableCollection<string> FoundedFiles { get; private set; }
        public ObservableCollection<string> ScanInfo { get; private set; }

        public SelectedDriveVM()
        {
            AvailableDrivesList = Manager.GetListOfDrivers();
            SelectedDrivesList = new ObservableCollection<DriveInfo>();
            AvailableRemovableDrivesList = Manager.GetListOfRemovableDrivers();
            ScanInfo = new ObservableCollection<string>();
            FoundedFiles = new ObservableCollection<string>();
        }

        #region Commands
        private ButtonCommand addCommand;
        private ButtonCommand removeCommand;
        private ButtonCommand startCommand;
        private ButtonCommand refreshCommand;
        private ButtonCommand stopCommand;
        private ButtonCommand openFileCommand;
        private ButtonCommand refreshDriveTypes;

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
                      //Clear all
                      FoundedFiles.Clear();
                      ScanInfo.Clear();
                      CountFiles = 0;
                      TotalCountFiles = 0;
                      isScanning = true;

                      stealer = new Stealer(AvailableRemovableDrive, SelectedDrivesList.ToArray(), Manager.GetUsersExtensions(), WriteInfo);
                      stealer.DriveChanged += DriveInfoChanged;
                      stealer.CountFilesChanged += CountChanged;
                      stealer.TotalCountFilesChanged += TotalCountChanged;
                      stealer.StealAsync();
                  }, 
                  (obj) =>
                  {
                      if (AvailableRemovableDrive is null) return false;
                      if (SelectedDrivesList.Count == 0) return false;
                      return !isScanning;
                  }
                  ));
            }
        }

        public ButtonCommand RefreshCommand
        {
            get
            {
                return refreshCommand ??
                  (refreshCommand = new ButtonCommand(obj =>
                  {
                      AvailableRemovableDrivesList.Clear();
                      foreach (DriveInfo drive in Manager.GetListOfRemovableDrivers())
                      {
                          AvailableRemovableDrivesList.Add(drive);
                      }
                  }));
            }
        }

        public ButtonCommand StopCommand
        {
            get
            {
                return stopCommand ??
                  (stopCommand = new ButtonCommand(obj =>
                  {
                      stealer.StopSteal();
                      isScanning = false;
                  },
                  (obj) =>
                  {
                      return isScanning;
                  }
                  ));
            }
        }

        public ButtonCommand OpenFileCommand
        {
            get
            {
                return openFileCommand ??
                  (openFileCommand = new ButtonCommand(obj =>
                  {
                      Manager.OpenFile(obj.ToString());
                  },
                  (obj) =>
                  {
                      return !(ChosenFile is null);
                  }
                  ));
            }
        }

        public ButtonCommand RefreshDriveTypes
        {
            get
            {
                return refreshDriveTypes ??
                  (refreshDriveTypes = new ButtonCommand(obj =>
                  {
                      if (obj is DriveTypes)
                      {
                          Manager.ChosenDriveType = (DriveTypes)obj;
                          AvailableDrivesList.Clear();
                          foreach (var AD in Manager.GetListOfDrivers())
                          {
                              AvailableDrivesList.Add(AD);
                          }
                      }
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

        public string ChosenFile
        {
            get { return chosenFile; }
            set
            {
                chosenFile = value;
                OnPropertyChanged("ChosenFile");
            }
        }

        public ulong CountFiles
        {
            get { return countFiles; }
            set
            {
                countFiles = value;
                OnPropertyChanged("CountFiles");
            }
        }

        public ulong TotalCountFiles
        {
            get { return totalCountFiles; }
            set
            {
                totalCountFiles = value;
                OnPropertyChanged("TotalCountFiles");
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
            FoundedFiles.Add(info);
        }

        private void DriveInfoChanged(object sender, DriveInformationEventArgs e)
        { 
            ScanInfo.Add(e.Information);
        }

        private void CountChanged(object sender, CountFilesEventArgs e)
        {
            CountFiles = e.Count;
        }

        private void TotalCountChanged(object sender, CountFilesEventArgs e)
        {
            TotalCountFiles = e.Count;
        }
        #endregion
    }
}
