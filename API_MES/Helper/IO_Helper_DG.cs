using System;
using System.Collections.Generic;
using System.IO;

namespace API_MES.Helper
{
    public class IO_Helper_DG
    {
        public IO_Helper_DG()
        {
        }
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
