using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common.XML
{
    /// <summary>
    /// Класс для сериализации/десериализации XML
    /// </summary>
    public static class XmlSerialize
    {
        /// <summary>
        /// Сериализует объект в XML файл на диске
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="fileName">Имя файла</param>
        public static void SaveToFile(object obj, string fileName)
        {
            using (TextWriter writer = new StreamWriter(fileName))
            {
                XmlSerializer x = new XmlSerializer(obj.GetType());
                x.Serialize(writer, obj);
            }
        }

        /// <summary>
        /// Сериализует объект в XML файл на диске (выполняется асинхронно)
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="fileName">Имя файла</param>
        public static void SaveToFileAsync(object obj, string fileName)
        {
            Task.Factory.StartNew(() => SaveToFile(obj, fileName));
        }

        /// <summary>
        /// Создает объект из XML-представления
        /// </summary>
        /// <typeparam name="TClass">Тип объекта</typeparam>
        /// <param name="xml">строка, содержащая XML в кодировке UTF-8</param>
        /// <returns>Объект указанного типа</returns>
        public static TClass GetFromString<TClass>(string xml) where TClass : class, new()
        {
            using (Stream reader = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof (TClass));
                return (TClass) serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Создает объект из XML-представления, загружая его из указанного файла
        /// </summary>
        /// <typeparam name="TClass">Тип объекта</typeparam>
        /// <param name="filePath">строка, содержащая путь к файлу</param>
        /// <returns>Объект указанного типа</returns>
        public static TClass GetFromFile<TClass>(string filePath) where TClass : class, new()
        {
            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                XmlSerializer serializer = new XmlSerializer(typeof (TClass));
                return (TClass) serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Сериализует объект в XML строку
        /// </summary>
        /// <param name="obj">Объект который необходимо сериализовать</param>
        /// <returns>Строка, содержащая сериализованный объект в виде XML</returns>
        public static string SerializeToString<TObject>(TObject obj)
        {
            var serializer = new XmlSerializer(obj.GetType());

            using (var stringWriter = new Utf8StringWriter())
            {
                serializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }

        #region [Helper Types]

        /// <summary>
        /// Переопределяет кодировку для сериализации (необходима кодировка UTF-8)
        /// </summary>
        private class Utf8StringWriter : StringWriter
        {
            /// <summary>
            /// Возвращает кодировку UTF-8
            /// </summary>
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }

        #endregion [/Helper Types]
    }
}
