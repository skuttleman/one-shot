using System;
using System.Collections.Generic;

namespace Game.Utils
{
    public static class Sets
    {
        public static bool ContainsAny<T>(ISet<T> set, params T[] coll)
        {
            return Colls.ReduceUntil(
                coll,
                (b, i) =>
                    set.Contains(i)
                        ? Reduction<bool>.Reduced(true)
                        : Reduction<bool>.UnReduced(b),
                false);
        }
    }
}
