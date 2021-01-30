using System;
using System.IO;

namespace API_MES.Helper
{
    public class RootPath_MVC
    {
        
        public static bool CreateDirectoryIfNotExist(string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            return true;
        }
    }
}
