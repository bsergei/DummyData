using System;

namespace Example.DataGen
{
    internal static class Helpers
    {
        public static int ToInt32(this string s)
        {
            var charArray = s.ToCharArray();
            int sum = 0;
            unchecked
            {
                for (int i = 0; i < charArray.Length * sizeof(Char); i++)
                    sum += Buffer.GetByte(charArray, i) << (i % 24);
            }

            return sum;
        }
    }
}