using System.IO;
using System.Threading.Tasks;

namespace Common.IO
{
    /// <summary>
    /// Предоставляет методы для работы с текстовыми файлами
    /// </summary>
    public static class TextFile
    {
        /// <summary>
        /// Сохранение строки в файл (выполняется асинхронно). Если файл не существует - он создается. Если существует - его содержимое перезаписывается.
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="fileName">Имя файла</param>
        public static void WriteAllTextAsync(string str, string fileName)
        {
            Task.Factory.StartNew(() => File.WriteAllText(str, fileName));
        }
    }
}