using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace MagnasStudio.Util
{
    public static class MsFileExtensions
    {
        public static bool CheckAndReadFile(this string path,out string json)
        {
            var exit = path.FileExit();
            if (exit)
            {
                json= File.ReadAllText(path);
                return true;
            }
            json=string.Empty;
            return false;
        }

        public static void WriteFile(this string path, string data)
        {
            //TODO find better way to write file Temp fix as file writing is already created file is creating issue 
            path.DeleteFile();
            var file = File.Open(path, FileMode.OpenOrCreate);
            var bytes = Encoding.UTF8.GetBytes(data);
            file.Write(bytes, 0, bytes.Length);
            file.Close();
        }

        public static bool DeleteFile(this string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }

        public static bool FileExit(this string path)
        {
            return File.Exists(path);
        }
    }
}
