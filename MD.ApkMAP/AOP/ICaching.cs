using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MD.ApkMAP.AOP
{
    public interface ICaching
    {
        object Get(string cacheKey);

        bool Set(string cacheKey, object cacheValue,TimeSpan expiresSliding, TimeSpan expiressAbsoulte);
    }
}
