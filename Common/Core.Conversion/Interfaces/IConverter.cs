using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Conversion.Interfaces
{
    public interface IConverter<From, To> : ITypedConverter, IConverter
    {
        To Convert(From source);

    }

    public interface IConverter
    {
        object Convert(object source);
    }

    public interface IConverterDefault<From, To>
    {
        To Convert(From source, To defVal);
        
    }

    public interface ITypedConverter
    {
        Type FromType { get; }
        Type ToType { get; }
    }
}