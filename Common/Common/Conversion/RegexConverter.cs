using System.Collections.Generic;
using Common.Strings;

namespace Common.Conversion
{
    /// <summary>
    /// Конвертер на основе Regex
    /// </summary>
    /// <typeparam name="TResult">Тип искомого (выходного) объекта конвертации</typeparam>
    public class RegexConverter<TResult> : IConverter<string, TResult>
    {
        private readonly RegexMapper<TResult> _mapper;

        public RegexConverter(IDictionary<string, TResult> content)
            : this((IEnumerable<KeyValuePair<string, TResult>>) content)
        {
        }

        public RegexConverter(IEnumerable<KeyValuePair<string, TResult>> content)
        {
            _mapper = new RegexMapper<TResult>(content);
        }

        public virtual TResult Convert(string source)
        {
            TResult retval;
            if (_mapper.Find(source.ToLower(), out retval))
                return retval;

            throw new KeyNotFoundException(string.Format("Для указанного значения <{0}> не найдено соответствие в справочнике.", source));
        }
    }
}