using System;
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
    public class GroupTransformer<Source, Dest, ExecContext> : ITransformer<Source, Dest, ExecContext>
    {
        protected Dictionary<Type, ITransformer<Source, Dest, ExecContext>> transformers = new Dictionary<Type, ITransformer<Source, Dest, ExecContext>>();

        public GroupTransformer(IEnumerable<ITransformer<Source, Dest, ExecContext>> transformers)
        {
            foreach (var tr in transformers)
                this.transformers.Add(tr.GetType(), tr);
        }

        public virtual void Transform(Source obj, Dest destObj, ExecContext execContext)
        {
            foreach (var tr in transformers)
                tr.Value.Transform(obj, destObj, execContext);
        }

        public void Add(ITransformer<Source, Dest, ExecContext> transformer)
        {
            if (!transformers.ContainsKey(transformer.GetType()))
                transformers.Add(transformer.GetType(), transformer);
        }

        /// <summary>
        /// Складывает трансформеры из двух групп в одну. Дублирование исключается.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static GroupTransformer<Source, Dest, ExecContext> operator +(GroupTransformer<Source, Dest, ExecContext> a, GroupTransformer<Source, Dest, ExecContext> b)
        {
            IDictionary<Type, ITransformer<Source, Dest, ExecContext>> summaryTransformers = new Dictionary<Type, ITransformer<Source, Dest, ExecContext>>();

            if (a == null) return b;

            if (b == null) return a;

            foreach (var trA in b.transformers.Values)
                a.Add(trA);

            return a;
        }
    }
}