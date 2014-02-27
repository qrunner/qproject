using System.Collections.Generic;

namespace Common.Strings.Transliteration
{
    /// <summary>
    /// Провайдер транслитерации с кириллицы на латиницу по ГОСТ Р 52535.1-2006
    /// </summary>
    public class CyrillicToLatinProvider : DictionaryTransliteProvider
    {
        readonly Dictionary<char, string> _translitDictionary = new Dictionary<char, string>
            {
                {'а', "a"}, 
                {'б', "b"},
                {'в', "v"},
                {'г', "g"}, 
                {'д', "d"}, 
                {'е', "e"},
                {'ё', "e"}, 
                {'ж', "zh"}, 
                {'з', "z"}, 
                {'и', "i"}, 
                {'й', "i"}, 
                {'к', "k"},
                {'л', "l"},
                {'м', "m"},
                {'н', "n"}, 
                {'о', "o"}, 
                {'п', "p"},
                {'р', "r"},
                {'с', "s"}, 
                {'т', "t"},
                {'у', "u"},
                {'ф', "f"},
                {'х', "kh"},
                {'ц', "tc"}, 
                {'ч', "ch"}, 
                {'ш', "sh"}, 
                {'щ', "shch"},
                {'ъ', ""}, 
                {'ы', "y"}, 
                {'ь', ""},
                {'э', "e"},
                {'ю', "iu"}, 
                {'я', "ia"},
                
                {'А', "A"}, 
                {'Б', "B"},
                {'В', "V"}, 
                {'Г', "G"}, 
                {'Д', "D"}, 
                {'Е', "E"},
                {'Ё', "E"}, 
                {'Ж', "ZH"},
                {'З', "Z"}, 
                {'И', "I"},
                {'Й', "I"}, 
                {'К', "K"},
                {'Л', "L"}, 
                {'М', "M"},
                {'Н', "N"}, 
                {'О', "O"}, 
                {'П', "P"}, 
                {'Р', "R"},
                {'С', "S"},
                {'Т', "T"}, 
                {'У', "U"}, 
                {'Ф', "F"}, 
                {'Х', "KH"},
                {'Ц', "TC"}, 
                {'Ч', "CH"},
                {'Ш', "SH"},
                {'Щ', "SHCH"},
                {'Ъ', ""}, 
                {'Ы', "Y"}, 
                {'Ь', ""},
                {'Э', "E"},
                {'Ю', "IU"},
                {'Я', "IA"}
            };

        protected override IDictionary<char, string> TranslitDictionary
        {
            get { return _translitDictionary; }
        }  
    }
}
