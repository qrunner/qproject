using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Conversion.Interfaces;
using Core.Conversion.Enums;

namespace Core.Conversion.Classes
{
    /// <summary>
    /// Строит цепочку обязанностей пулов конвертеров
    /// </summary>
    public class ChainPoolsBuilder : IChainPoolsBuilder
    {

        private IConversionEntitysFactory _conversionEntitysFactory;

        public ChainPoolsBuilder()
        {
            _conversionEntitysFactory = new ConversionEntitysFactory();
        }

        public static IEnumerable<EqualsConverterRulleType> DefaultRulleTypes
        {
            get
            {
                yield return EqualsConverterRulleType.Full;
                yield return EqualsConverterRulleType.DirectionExcluding;
                yield return EqualsConverterRulleType.ServiceNameExcluding;
                yield return EqualsConverterRulleType.ConversionTypeExcluding;
            }
        }

        public IConvertersPool Build(IEnumerable<EqualsConverterRulleType> rullesSequence)
        {
            IConvertersPool root = null;
            IConvertersPool pool = null;
            IConvertersPool previousPool = null;
            foreach(var rulle in rullesSequence)
            {
                pool = _conversionEntitysFactory.GetConvertersPool();
                pool.RullesChecker = _conversionEntitysFactory.GetRullesChecker(rulle);
                if (root == null)
                    root = pool;
                if (previousPool != null)
                    previousPool.ChildConvertersPool = pool;
                previousPool = pool;
            }
            return root;
        }

        public IConvertersPool Build()
        {
            return Build(DefaultRulleTypes);
        }
    }
}
