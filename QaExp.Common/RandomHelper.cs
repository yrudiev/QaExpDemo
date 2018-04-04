using System;
using System.Linq;

namespace QaExp.Common
{
    public static class RandomHelper
    {
        private static readonly Random _random;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        static RandomHelper()
        {
            _random = new Random();
        }

        public static int GetRandomInt()
        {
            return _random.Next(0, int.MaxValue);
        }

        public static string GetRandomAlphanumericString(int length)
        {
            return new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
