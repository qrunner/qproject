namespace Common.Exceptions
{
    /// <summary>
    /// Расширение для работы с исключениями
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// Получает развернутое сообщение об ошибке, включающее все вложенные исключения
        /// </summary>
        /// <param name="ex">Исключение</param>
        /// <param name="delimeter">Разделитель вложенный исключений</param>
        /// <returns>Развернутое сообщение об ошибке</returns>
        public static string ExpandMessage(this System.Exception ex, string delimeter = " <= ")
        {
            string retval = ex.Message;

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                retval += delimeter + ex.Message;
            }
            return retval;
        }
    }
}