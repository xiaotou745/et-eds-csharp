using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCore
{
    public interface ITranslator<T, TResult>
    {
        TResult Translate(T from);
        T Translate(TResult from);

        IList<TResult> Translate(ICollection<T> froms);
    }
}
