using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StressClient.Core.Extensions
{
    public static class RandomExtensions
    {
        private static Random randomizer = new Random();

        public static bool HitTest(this double value)
        {
            return randomizer.NextDouble() <= value;
        }

        /// <summary>
        /// Get a random element from a list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>B
        public static T Random<T>(this IEnumerable<T> list)
        {
            return list.ElementAt(randomizer.Next(list.Count()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> RandomList<T>(this IEnumerable<T> list, int count)
        {
            if (list.Count() <= count)
                return new List<T>(list);

            List<T> res = new List<T>();
            List<T> clone = new List<T>(list);

            while (res.Count != count && clone.Count > 0)
            {
                var item = clone.Random();
                res.Add(item);
                clone.Remove(item);
            }

            return res;
        }
        public static IEnumerable<T> RandomList<T>(this IEnumerable<T> list, double percent)
        {
            if (percent < 0 || percent > 1)
                throw new InvalidOperationException("percent should be between 0 and 1.");

            var nb = Convert.ToInt32(Math.Ceiling(percent * list.Count()));

            return list.RandomList(nb);
        }
    }
}
