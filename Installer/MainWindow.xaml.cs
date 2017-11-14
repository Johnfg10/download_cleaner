using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly string downloadfolder = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";

        public static readonly string programFolders = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        public static readonly string downloadCleanerFolder = programFolders + @"\johnfg10";
        public static readonly string jsondll = downloadCleanerFolder + @"\Newtonsoft.Json.dll";
        public static readonly string downloadCleanerLocation = downloadCleanerFolder + @"\downloadcleaner.exe";
        public static readonly string registryName = "DownloadCleaner";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists(downloadCleanerLocation))
            {
                errorLabel.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                errorLabel.Content = "Already installed!";
                return;
            }
            Directory.CreateDirectory(downloadCleanerFolder);

            if (System.IO.File.Exists(@".\DownloadCleaner.exe") && System.IO.File.Exists(@".\Newtonsoft.Json.dll"))
            {
                System.IO.File.Copy(@".\DownloadCleaner.exe", downloadCleanerLocation);
                System.IO.File.Copy(@".\Newtonsoft.Json.dll", jsondll);

                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                key.SetValue(registryName, downloadCleanerLocation);

                string path = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine) + downloadCleanerFolder + ";";
                Environment.SetEnvironmentVariable("Path", path, EnvironmentVariableTarget.Machine);
            }
        }

        private void launchSilentCommandWindow(string command)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = command;
            process.StartInfo = startInfo;
            process.Start();
        }

        private void UnInstallButton_Click(object sender, RoutedEventArgs e)
        {
            System.IO.File.Delete(downloadCleanerLocation);
            System.IO.File.Delete(jsondll);
            Directory.Delete(downloadCleanerFolder);
            
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key.DeleteValue(registryName, false);

            string path = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine).Replace(downloadCleanerFolder + ";", "");
            Environment.SetEnvironmentVariable("Path", path, EnvironmentVariableTarget.Machine);

            return;
        }
    }
}
