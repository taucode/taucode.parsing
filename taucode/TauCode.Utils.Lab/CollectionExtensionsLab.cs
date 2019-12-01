﻿using System;
using System.Collections.Generic;

namespace TauCode.Utils.Lab
{
    public static class CollectionExtensionsLab
    {
        public static int FindFirstIndexOfLab<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (predicate(item))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int FindFirstIndexOfLab<T>(this IReadOnlyList<T> list, T value)
        {
            return list.FindFirstIndexOfLab(x => Equals(x, value));
        }

        public static int FindLastIndexOfLab<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            for (var i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];
                if (predicate(item))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int FindLastIndexOfLab<T>(this IReadOnlyList<T> list, T value)
        {
            return list.FindLastIndexOfLab(x => Equals(x, value));
        }
    }
}