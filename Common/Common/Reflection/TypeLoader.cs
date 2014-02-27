using System;

namespace Common.Reflection
{
    public static class TypeLoader
    {
        /// <summary>
        /// Пытается получить тип по имени. При отсутствии типа в загруженных сборках, загружает сборку, и ищет тип там.
        /// </summary>
        /// <param name="typeName">Полное имя типа, с указанием сборки</param>
        /// <param name="type">Тип</param>
        /// <returns>true если тип найден, false - если тип не был найден.</returns>
        public static bool TryGetType(string typeName, out Type type)
        {
            type = null;
            try
            {
                type = Type.GetType(typeName, true);
            }
            catch (TypeLoadException)
            {
                return false;
            }
            return true;
        }

        public static T CreateInstance<T>(string typeName)
        {
            T retval;
            Type providerClass;

            if (TryGetType(typeName, out providerClass))
                retval = (T) Activator.CreateInstance(providerClass);
            else
                throw new Exception("Невозможно загрузить тип " + typeName);

            return retval;
        }
    }
}