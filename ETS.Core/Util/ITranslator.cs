using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Util
{
    public interface ITranslator<T, TResult>
    {
        TResult Translate(T from);
        T Translate(TResult from);

        IList<TResult> Translate(ICollection<T> froms);
    }
}
