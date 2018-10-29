using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Logic;

namespace FIleStealer.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string[] extensions = new string[UsersExtensions.Items.Count];
            int i = 0;
            foreach (var value in UsersExtensions.Items)
            {
                extensions[i] = value.ToString();
                i++;
            }

            Manager.SaveUsersExtensions(extensions);

            MainWindow mainWindow = Owner as MainWindow;

            if (RadioAll.IsChecked.Value) mainWindow.SetDriveType(DriveTypes.All);
            if (RadioReady.IsChecked.Value) mainWindow.SetDriveType(DriveTypes.Ready);
            if (RadioRaU.IsChecked.Value) mainWindow.SetDriveType(DriveTypes.ReadyAndUnremovable);
            if (RadioRO.IsChecked.Value) mainWindow.SetDriveType(DriveTypes.Removable);
            if (RadioUO.IsChecked.Value) mainWindow.SetDriveType(DriveTypes.Unremovable);
        }
    }
}
