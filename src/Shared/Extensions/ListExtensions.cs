using System;
using System.Collections.Generic;
using System.Linq;

namespace POC.Storage.Extensions
{
    /// <summary>
    /// Extension methods for Lists
    /// </summary>
    internal static class ListExtensions
    {
        /// <summary>
        /// Builds a tree from a sequential collection.
        /// </summary>
        public static IList<T> AsHierarchyList<T, TKey>(this IList<T> items, Func<T, bool> isRootFunc, Func<T, TKey> idFunc, Func<T, TKey> parentIdFunc, Action<T, T> addToChildren)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var newItems = new List<T>();
            if (items.Count > 0)
            {
                var dictionary = items.ToDictionary(idFunc);
                foreach (var item in dictionary.Values)
                {
                    var parentId = parentIdFunc(item);
                    if (parentId != null)
                    {
                        if (dictionary.TryGetValue(parentId, out var parent))
                        {
                            addToChildren(parent, item);
                        }
                    }

                    if (isRootFunc(item))
                    {
                        newItems.Add(item);
                    }
                }
            }
            return newItems;
        }

        /// <summary>
        /// Builds a tree from a sequential collection.
        /// </summary>
        public static T AsHierarchy<T, TKey>(this IList<T> items, Func<T, TKey> idFunc, Func<T, TKey> parentIdFunc, Action<T, T> addToChildren)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (items.Count > 0)
            {
                var root = items[0];
                if (items.Count > 1)
                {
                    var dictionary = items.ToDictionary(idFunc);
                    foreach (var item in dictionary.Values)
                    {
                        var parentId = parentIdFunc(item);
                        if (parentId != null)
                        {
                            if (dictionary.TryGetValue(parentId, out var parent))
                            {
                                addToChildren(parent, item);
                            }
                        }
                    }
                }
                return root;
            }
            return default!;
        }
    }
}
