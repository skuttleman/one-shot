﻿using System.Collections.Generic;

namespace Game.Utils {
    public static class Sets {
        public static bool ContainsAny<T>(ISet<T> set, params T[] coll) {
            foreach (T item in coll)
                if (set.Contains(item)) return true;
            return false;
        }

        public static ISet<T> Of<T>(params T[] items) {
            ISet<T> set = new HashSet<T>();
            foreach (T item in items) set.Add(item);
            return set;
        }
    }
}
