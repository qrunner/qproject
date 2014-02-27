using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Processing
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Source"></typeparam>
    /// <typeparam name="Dest"></typeparam>
    /// <typeparam name="ExecContext"></typeparam>
    public class GroupBuilder<Source, Dest, ExecContext> : IBuilder<Source, Dest, ExecContext>
        where Dest : class, new()
    {
        protected IEnumerable<ITransformer<Source, Dest, ExecContext>> transformers = null;

        public GroupBuilder(IEnumerable<ITransformer<Source, Dest, ExecContext>> transformers)
        {
            this.transformers = transformers;
        }

        public virtual Dest Transform(Source obj, ExecContext execContext)
        {
            Dest retval = new Dest();
            foreach (var tr in transformers)
                tr.Transform(obj, retval, execContext);
            return retval;
        }
        /// <summary>
        /// Складывает трансформеры из двух групп в одну. Дублирование исключается.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static GroupBuilder<Source, Dest, ExecContext> operator +(GroupBuilder<Source, Dest, ExecContext> a, GroupBuilder<Source, Dest, ExecContext> b)
        {
            IDictionary<Type, ITransformer<Source, Dest, ExecContext>> summaryTransformers = new Dictionary<Type,ITransformer<Source, Dest, ExecContext>>();
            
            foreach (var trA in a.transformers)
                if (!summaryTransformers.ContainsKey(trA.GetType()))
                    summaryTransformers.Add(trA.GetType(), trA);

            foreach (var trB in b.transformers)
                if (!summaryTransformers.ContainsKey(trB.GetType()))
                    summaryTransformers.Add(trB.GetType(), trB);
            
            return new GroupBuilder<Source, Dest, ExecContext>(summaryTransformers.Values);
        }
    }
}