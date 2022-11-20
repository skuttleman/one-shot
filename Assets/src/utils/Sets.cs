using System.Collections.Generic;

namespace Game.Utils {
    public static class Sets {
        public static bool ContainsAny<T>(ISet<T> set, params T[] coll) {
            foreach (T item in coll)
                if (set.Contains(item)) return true;
            return false;
        }
    }
}
