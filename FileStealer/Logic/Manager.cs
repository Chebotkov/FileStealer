using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Logic
{
    public static class Manager
    {
        private static string userFileName = "UsersExtensions.txt";
        private static string fileName = "Extensions.txt";
        private static string[] extensions = { ".jpg", ".jpeg", ".bmp", ".mp3", ".wav", ".mp4", ".avi", ".exe", ".doc", ".docx", ".txt", ".rar", ".zip" };

        #region Properties
        public static string UserFileName
        {
            get
            {
                return String.Copy(userFileName);
            }
            set
            {
                if (value is null)
                    throw new ArgumentNullException();

                userFileName = String.Copy(value);
            }
        }
        
        public static string FileName
        {
            get
            {
                return String.Copy(fileName);
            }
            set
            {
                if (value is null)
                    throw new ArgumentNullException();

                fileName = String.Copy(value);
            }
        }

        public static string[] Extensions
        {
            get
            {
                return extensions;
            }
            set
            {
                if (value is null)
                    throw new ArgumentNullException();

                extensions = value;
            }
        }
        #endregion
        static Manager()
        {
            string file = GetFilePath() + "\\" + FileName;
            if (!File.Exists(file))
            {
                using (StreamWriter writer = new StreamWriter(file, false, System.Text.Encoding.Default))
                {
                    foreach (string extension in Extensions)
                    {
                        writer.WriteLine(extension);
                    }
                }
            }
        }

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
        
        public static void SaveUsersExtensions(string[] extensions)
        {
            using (StreamWriter writer = new StreamWriter(GetFilePath() + "\\" + UserFileName, false, System.Text.Encoding.Default))
            {
                foreach (string extension in extensions)
                {
                    writer.WriteLine(extension);
                }
            }
        }

        public static string[] GetExtensions()
        {
            return LoadExtensions(GetFilePath() + "\\" + FileName);
        }

        public static string[] GetUsersExtensions()
        {
            return LoadExtensions(GetFilePath() + "\\" + UserFileName);
        }

        public static string[] LoadExtensions(string FilePath)
        {
            List<string> extensions = new List<string>();

            if (File.Exists(FilePath))
            {
                using (StreamReader reader = new StreamReader(FilePath, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        extensions.Add(line);
                    }
                }
            }
            return extensions.ToArray();
        }

        private static string GetFilePath()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
