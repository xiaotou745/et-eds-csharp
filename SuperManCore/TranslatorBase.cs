using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCore
{
    public abstract class TranslatorBase<TDomain, TModel> : ITranslator<TDomain, TModel>
    {
        public IList<TModel> Translate(ICollection<TDomain> froms)
        {
            var tos = new List<TModel>();
            foreach (TDomain from in froms)
            {
                var to = this.Translate(from);
                tos.Add(to);
            }
            return tos;
        }

        public abstract TModel Translate(TDomain from);


        public abstract TDomain Translate(TModel from);
    }
}
