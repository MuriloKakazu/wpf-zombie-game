using System;
using System.Collections.Generic;
using System.Linq;

namespace ZombieGame.Extensions
{
    public static class IEnumerableExtension
    {
        static Random R = new Random();

        public static T PickAny<T>(this IEnumerable<T> array)
        {
            return array.ElementAt(R.Next(0, array.Count()));
        }
    }
}
