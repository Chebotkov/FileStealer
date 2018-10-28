using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class Manager
    {
        public static ObservableCollection<DriveInfo> GetListOfDrivers()
        {
            return GetDrives(false);
        }

        public static ObservableCollection<DriveInfo> GetListOfRemovableDrivers()
        {
            return (GetDrives(true));
        }

        private static ObservableCollection<DriveInfo> GetDrives(bool isRemovable)
        {
            ObservableCollection<DriveInfo> Drives = new ObservableCollection<DriveInfo>();
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady && drive.DriveType == DriveType.Removable && isRemovable)
                {
                    Drives.Add(drive);
                }
                else if (drive.IsReady && drive.DriveType != DriveType.Removable && !isRemovable)
                {
                    Drives.Add(drive);
                }
            }

            return Drives;
        }

        public static void OpenFile(string pathToFile)
        {
            if (pathToFile is null)
            {
                throw new ArgumentNullException(String.Format("{0} is null"), nameof(pathToFile));
            }

            if (!File.Exists(pathToFile))
            {
                throw new FileNotFoundException(String.Format("File {0} doesn't exists."), pathToFile);
            }

            System.Diagnostics.Process.Start(pathToFile);
        }
    }
}
