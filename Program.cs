﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace GoToMeeting_8_18_Uninstaller
{
    class Program
    {
        static void Main(string[] args)
        {

            //store the uninstall path as a constant
            const String uninstallerPath = "C:\\Program Files (x86)\\GoToMeeting\\";

            //create a list to store the usernames on the computer
            List<String> userList = GetComputerUsers();

            //stop the running GoToMeeting Processes
            StopProcesses();

            //Sleep for 2 seconds to wait for the processes to release file control
            Thread.Sleep(20000);

            //Remove the users AppData for GoToMeeting
            foreach (String user in userList)
            {
                String appDataPath1 = "C:\\Users\\" + user + "\\AppData\\Local\\Temp\\LogMeInLogs\\GoToMeeting";
                String appDataPath2 = "C:\\Users\\" + user + "\\AppData\\Local\\GoToMeeting";
                RemoveDirectory(appDataPath1);
                RemoveDirectory(appDataPath2);
            }

            //Delete the classes root subkeys
            DeleteSubKeyTree("ClassesRoot", ".gotomeeting");
            DeleteSubKeyTree("ClassesRoot", "gotomeeting");
            DeleteSubKeyTree("ClassesRoot", ".gotomeeting8034");
            DeleteSubKeyTree("ClassesRoot", "gotomeeting8034");
            DeleteSubKeyTree("ClassesRoot", ".gtm");
            DeleteSubKeyTree("ClassesRoot", "MIME\\Database\\Content Type\\application/x-gotomeeting");
            DeleteSubKeyTree("ClassesRoot", "\\MIME\\Database\\Content Type\\application/x-gotomeeting8034");
            DeleteSubKeyTree("ClassesRoot", "\\WOW6432Node\\CLSID\\{23f891a4-572b-474a-86da-66cdd3d1ac2e}");
            DeleteSubKeyTree("ClassesRoot", "\\WOW6432Node\\CLSID\\{43B36225-3A02-4097-87F2-B8D89ED5CE02}");
            DeleteSubKeyTree("ClassesRoot", "\\WOW6432Node\\CLSID\\{84B5A313-CD5D-4904-8BA2-AFDC81C1B309}");
            DeleteSubKeyTree("ClassesRoot", "\\CLSID\\{43B36225-3A02-4097-87F2-B8D89ED5CE02}");
            DeleteSubKeyTree("ClassesRoot", "\\CLSID\\{84B5A313-CD5D-4904-8BA2-AFDC81C1B309}");
            DeleteSubKeyTree("ClassesRoot", "\\G2MAddin.OutlookAddin");
            DeleteSubKeyTree("ClassesRoot", "\\LogMeInInc.Collab");
            DeleteSubKeyTree("ClassesRoot", "\\LogMeInInc.Collab.G2M");
            DeleteSubKeyTree("ClassesRoot", "\\LogMeInInc.Collab8034");

            //Delete the local machine registry values
            DeleteLMRegValue("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Run\\", "GoToMeetingInstall8034");

            //TODO Delete User reg values for running and shortcuts

            //Delete the subkey trees for the local machine
            DeleteSubKeyTree("LocalMachine", "\\SOFTWARE\\Classes\\LogMeInInc.Collab");
            DeleteSubKeyTree("LocalMachine", "\\SOFTWARE\\Classes\\LogMeInInc.Collab.G2M");
            DeleteSubKeyTree("LocalMachine", "\\SOFTWARE\\Classes\\CLSID\\{43B36225-3A02-4097-87F2-B8D89ED5CE02}");
            DeleteSubKeyTree("LocalMachine", "\\SOFTWARE\\Classes\\WOW6432Node\\CLSID\\{23f891a4-572b-474a-86da-66cdd3d1ac2e}");
            DeleteSubKeyTree("LocalMachine", "\\SOFTWARE\\Classes\\WOW6432Node\\CLSID\\{43B36225-3A02-4097-87F2-B8D89ED5CE02}");
            DeleteSubKeyTree("LocalMachine", "\\SOFTWARE\\Microsoft\\Internet Explorer\\ProtocolExecute\\gotomeeting");
            DeleteSubKeyTree("LocalMachine", "\\SOFTWARE\\Microsoft\\Internet Explorer\\ProtocolExecute\\gotomeeting8034");
            DeleteSubKeyTree("LocalMachine", "\\SOFTWARE\\Microsoft\\MediaPlayer\\UIPlugins\\{43B36225-3A02-4097-87F2-B8D89ED5CE02}");
            DeleteSubKeyTree("LocalMachine", "\\SOFTWARE\\Classes\\LogMeInInc.Collab8034");
            DeleteSubKeyTree("LocalMachine", "\\SOFTWARE\\Classes\\MIME\\Database\\Content Type\\application/x-gotomeeting8034");
            DeleteSubKeyTree("LocalMachine","\\SOFTWARE\\Microsoft\\Internet Explorer\\ProtocolExecute\\gotomeeting8034");

            //Delete User Keys
            DeleteSubKeyTree("Users", "\\Software\\Classes\\CLSID\\{84B5A313-CD5D-4904-8BA2-AFDC81C1B309}");
            DeleteSubKeyTree("Users", "\\Software\\Classes\\G2MAddin.OutlookAddin");
            DeleteSubKeyTree("Users", "\\Software\\Classes\\WOW6432Node\\CLSID\\{84B5A313-CD5D-4904-8BA2-AFDC81C1B309}");
            DeleteSubKeyTree("Users", "\\Software\\LogMeInInc\\GoToMeeting");
            DeleteSubKeyTree("Users", "\\Software\\LogMeInInc");
            DeleteSubKeyTree("Users", "\\Software\\Microsoft\\Internet Explorer\\Low Rights\\ElevationPolicy\\{5448CFB3-F941-4DCC-A146-565556BA65B3}");
            DeleteSubKeyTree("Users", "\\Software\\Microsoft\\Office\\Excel\\Addins\\G2MAddin.OutlookAddin");
            DeleteSubKeyTree("Users", "\\Software\\Microsoft\\Office\\Outlook\\Addins\\G2MAddin.OutlookAddin");
            DeleteSubKeyTree("Users", "\\Software\\Microsoft\\Office\\PowerPoint\\Addins\\G2MAddin.OutlookAddin");
            DeleteSubKeyTree("Users", "\\Software\\Microsoft\\Office\\Word\\Addins\\G2MAddin.OutlookAddin");

            //Remove the folder for the program files
            RemoveDirectory(uninstallerPath);

            //Remove the product from add or remove programs
            DeleteSubKeyTree("Users", "\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\GoToMeeting");
            DeleteSubKeyTree("LocalMachine", "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{D41C99F3-DF21-4562-9AB8-3538219F39FD}");
            DeleteSubKeyTree("LocalMachine", "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{D41C99F3-DF21-4562-9AB8-3538219F39FD}");
            DeleteSubKeyTree("ClassesRoot", "\\Installer\\Products\\3F99C14D12FD2654A98B538312F993DF\\");

            //printe deleted to show success
            Console.WriteLine("Deleted!");
        }

        //method to check whether a file exists
        static bool FileExists(String filePath)
        {
            if (File.Exists(filePath))
            {
                return true;
            }

            return false;
        }

        //method to check whether a directory exists
        static bool DirectoryExists(String filePath)
        {
            if (Directory.Exists(filePath))
            {
                return true;
            }

            return false;
        }

        //method to remove a directory
        static void RemoveDirectory(String filePath)
        {
            if (DirectoryExists(filePath))
            {
                foreach (string file in Directory.GetFiles(filePath))
                {
                    File.Delete(file);
                }

                foreach (string subDir in Directory.GetDirectories(filePath))
                {
                    RemoveDirectory(subDir);
                }

                Thread.Sleep(1); // This makes the difference between whether it works or not. Sleep(0) is not enough.
                Directory.Delete(filePath);
            }
        }

        //method to get the user folder names on the computer
        static List<string> GetComputerUsers()
        {
            string path = @"C:\\Users\";
            List<String> userFolderList = new List<string>();

            foreach (string s in Directory.GetDirectories(path))
            {
                String value = s.Remove(0, path.Length);

                userFolderList.Add(value);
            }

            return userFolderList;
        }

        //method to delete a subkey tree for a sepcified registry root
        static void DeleteSubKeyTree(String rootName, String keyName)
        {
            

            if (rootName.Equals("ClassesRoot")) {
                Console.WriteLine("Attempting to find key: " + "HKEY_ClassesRoot\\" + keyName);
                var item = Registry.ClassesRoot.OpenSubKey(keyName);
                if (item != null)
                {
                    Console.WriteLine("\t" + keyName + " found");
                    item.Close();
                    Registry.ClassesRoot.DeleteSubKeyTree(keyName);
                    //item.DeleteSubKey(keyName);
                }
            }
            else if(rootName.Equals("LocalMachine"))
            {
                Console.WriteLine("Attempting to find key: " + "HKLM\\" + keyName);
                var item = Registry.LocalMachine.OpenSubKey(keyName);

                if (item != null)
                {
                    Console.WriteLine("\t" + keyName + " found");
                    item.Close();
                    Registry.LocalMachine.DeleteSubKeyTree(keyName);
                    //item.DeleteSubKey(keyName);
                }
            }

            else if (rootName.Equals("Users"))
            {
                
                var item = Registry.Users;

                var sids = item.GetSubKeyNames();

                foreach (var sid in sids)
                {
                    String subKeyName = sid + keyName;

                    Console.WriteLine("Attempting to find key: " + "HKEY_Users\\" + subKeyName);
                    
                    var key = Registry.Users.OpenSubKey(subKeyName);

                    if (key != null)
                    {
                        
                        Console.WriteLine("\t" + subKeyName + " found");
                        key.Close();
                        item.Close();
                            
                        Registry.Users.DeleteSubKeyTree(subKeyName);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Invalid root specified");
            }
            
        }

        //method to delete a specified local machine registry value
        static void DeleteLMRegValue(String keyName, String valueName)
        {
            Console.WriteLine("Attempting to Delete Value: " + keyName + valueName);
            var runSubKey = Registry.LocalMachine.OpenSubKey(keyName);

            if (runSubKey != null)
            {
                if (runSubKey.GetValue(valueName) != null)
                {
                    Console.WriteLine("\tfound value: {0}", valueName);
                    runSubKey.DeleteValue(valueName);
                }
            }
        }

        //method to stop running GoToMeeting Processes
        static void StopProcesses()
        {
            try
            {
                Console.WriteLine("Closing g2mstart process");
                Process[] g2mstart = Process.GetProcessesByName("g2mstart");

                foreach (var proc in g2mstart)
                {
                    proc.Kill();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not find or kill process");
            }

            try
            {
                Console.WriteLine("Closing g2mcomm process");
                Process[] g2mcomm = Process.GetProcessesByName("g2mcomm");

                foreach (var proc in g2mcomm)
                {
                    proc.Kill();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not find or kill process");
            }

            try
            {
                Console.WriteLine("Closing g2launcher process");
                Process[] g2mlauncher = Process.GetProcessesByName("g2mlauncher");

                foreach (var proc in g2mlauncher)
                {
                    proc.Kill();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not find or kill process");
            }
        }
    }
}
