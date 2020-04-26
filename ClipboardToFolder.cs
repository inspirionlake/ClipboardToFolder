using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Runtime.Remoting.Channels;
using Microsoft.Win32;

namespace ClipboardToFolder
{
    class ClipboardToFolder
    {
        static void TransferClipboardToFolder(string currentFolder)
        {
            if (Clipboard.ContainsImage())
            {
                Image img = Clipboard.GetImage();
                try
                {
                    string name = DateTime.Now.ToString("MM dd yyyy HH mm ss tt");
                    img.Save($"{currentFolder}\\{name}.bmp");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
        }

        public static void UnRegisterApp()
        {
            try
            {
                var key = OpenDirectoryKey().OpenSubKey(@"Background\shell", true);
                key.DeleteSubKeyTree("Paste Into File");

                key = OpenDirectoryKey().OpenSubKey("shell", true);
                key.DeleteSubKeyTree("Paste Into File");

                MessageBox.Show("Application has been Unregistered from your system", "Paste Into File", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nPlease run the application as Administrator !", "Paste Into File", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public static void RegisterApp()
        {
            try
            {
                var key = OpenDirectoryKey().CreateSubKey(@"Background\shell").CreateSubKey("Paste Into File");
                key.SetValue("Icon", "\"" + Application.ExecutablePath + "\",0");
                key = key.CreateSubKey("command");
                key.SetValue("", "\"" + Application.ExecutablePath + "\" \"%V\"");

                key = OpenDirectoryKey().CreateSubKey("shell").CreateSubKey("Paste Into File");
                key.SetValue("Icon", "\"" + Application.ExecutablePath + "\",0");
                key = key.CreateSubKey("command");
                key.SetValue("", "\"" + Application.ExecutablePath + "\" \"%1\"");
                MessageBox.Show("Application has been registered with your system", "Paste Into File", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                //throw;
                MessageBox.Show(ex.Message + "\nPlease run the application as Administrator !", "Paste As File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static RegistryKey OpenDirectoryKey()
        {
            return Registry.CurrentUser.CreateSubKey(@"Software\Classes\Directory");
        }

        [STAThread]
        static void Main(string[] args)
        {
            if (args[0] == "/reg")
            {
                RegisterApp();
            }
            else if(args[0] == "/unreg")
            {
                UnRegisterApp();
            }
            else
            {
                string currentFolder = args[0];
                TransferClipboardToFolder(currentFolder);
            }
        }
    }
}
