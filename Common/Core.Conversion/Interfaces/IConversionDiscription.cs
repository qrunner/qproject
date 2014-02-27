using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Conversion.Enums;

namespace Core.Conversion.Interfaces
{
    public interface IConversionDiscription<From, To> : IConversionDiscription
    {
        new IConverter<From, To> Converter { get; }

        //Func<IConverter<From, To>> CreateConverterFunc { get; set; }

    }

    public interface IConversionDiscription
    {
        Type DestType { get; set;}

        Type SourceType { get; set; }

        ThirdPartyServiceType? ThirdPartyServiceType { get; set; }

        IConverter Converter { get; }

        bool? ToThirdPartyServiceDirection { get; set; }

        ConversionType? ConversionType { get; set; }

        bool PartialEquals(IConversionDiscription pattern);
    }
}
