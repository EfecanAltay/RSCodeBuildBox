using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RSCodeBuildBox.Helper
{
    public class IOHelper
    {
        public static void RemoveDirectory(string path)
        {
            string[] projectDirectories = Directory.GetFileSystemEntries(path, "", SearchOption.AllDirectories);
            foreach (var projectDirectory in projectDirectories)
            {
                File.SetAttributes(projectDirectory, FileAttributes.Normal);
            }
            Directory.Delete(path, true);
        }

        public static bool CleanOrInitDirectory(string path)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                try
                {
                    RemoveDirectory(path);
                    Directory.CreateDirectory(path);
                }
                catch (DirectoryNotFoundException ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }
            return true;
        }
    }
}
