using System;
using System.IO;

namespace Common.IO
{
    /// <summary>
    /// Класс для работы с путями файлов и каталогов
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// Возвращает полный путь по относительному. Текущий каталог соответствует расположению приложения. Исходная строка должна начинаться с символа ~\
        /// </summary>
        /// <param name="relativePath">Относительный путь</param>
        /// <returns>Полный путь</returns>
        public static string FullPath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentNullException("relativePath");

            string fullPath = relativePath;
            if (relativePath.StartsWith(@"~\"))
                fullPath = relativePath.Replace(@"~\", AppDomain.CurrentDomain.BaseDirectory);
            
            return fullPath;
        }
        /// <summary>
        /// Возвращает полный путь по относительному. Текущий каталог соответствует расположению приложения. Исходная строка должна начинаться с символа ~\. 
        /// Если указанный каталог не существует, то он будет создан. 
        /// </summary>
        /// <param name="relativePath">Относительный путь</param>
        /// <returns>Полный путь</returns>
        public static string GetFullPathForceDirectory(string relativePath)
        {
            string retval = FullPath(relativePath);
            string dir = Path.GetDirectoryName(retval);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return retval;
        }
    }
}