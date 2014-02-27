using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Conversion.Interfaces
{
    public interface IConvertersProvider
    {
        void Provide();

        IConvertersPool ConvertersPool { get; set; }
    }
}
