using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Conversion.Enums;
using Core.Conversion.Interfaces;

namespace Core.Conversion.Classes
{
    /// <summary>
    /// Поставщик группы конвертеров. Загружает порцию конвертеров в мультиконвертер
    /// </summary>
    public class LightConversionDiscription : IConversionDiscription 
    {

        public Type DestType { get; set; }

        public Type SourceType { get; set; }

        public ThirdPartyServiceType? ThirdPartyServiceType { get; set; }

        public bool? ToThirdPartyServiceDirection { get; set; }

        public ConversionType? ConversionType { get; set; }

        public bool PartialEquals(IConversionDiscription pattern)
        {

            if (TypeIs(DestType, pattern.DestType) && TypeIs(SourceType, pattern.SourceType) && PartialSpecificEquals(ThirdPartyServiceType, pattern.ThirdPartyServiceType)
                && PartialSpecificEquals(ToThirdPartyServiceDirection, pattern.ToThirdPartyServiceDirection)
                && PartialSpecificEquals(ConversionType, pattern.ConversionType))

                return true;
            return false;
        }

        public static bool PartialSpecificEquals(object source, object pattern)
        {
            if (pattern == null)
                return true;
            if (pattern.Equals(source))
                return true;
            return false;
        }

        public static bool TypeIs(Type typeOne, Type typeIs)
        {
            //Все объекты являются наследниками Object
            if (typeOne == typeIs)
                return true;
            if (typeIs == typeof(object))
                return true;
            if (typeOne.IsSubclassOf(typeIs))
                return true;
            if (typeIs.IsInterface)
            {
                var parentTypes = typeOne.GetInterfaces();
                if (parentTypes.Contains(typeIs))
                    return true;
            }
            return false;

        }




        public virtual IConverter Converter
        {
            get
            {
                throw new NotImplementedException("В классе LightConversionDiscription нет конвертера.");
            }
        }

        public override bool Equals(object obj)
        {
            var discription = obj as IConversionDiscription;

            if (discription == null)
                return false;

            return GetHashCode() == discription.GetHashCode();
        }

        public override int GetHashCode()
        {
            var hash = DestType.GetHashCode() ^ SourceType.GetHashCode();

            if (ThirdPartyServiceType != null)
                hash = hash ^ (ThirdPartyServiceType.GetHashCode() + 1);
            if (ToThirdPartyServiceDirection != null)
                hash = hash ^ (ToThirdPartyServiceDirection.GetHashCode() + 2);
            if (ConversionType != null)
                hash = hash ^ (ConversionType.GetHashCode() + 3);

            return hash;

        }
    }
}
