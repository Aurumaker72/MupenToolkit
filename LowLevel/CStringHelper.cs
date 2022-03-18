using System.Linq;
using System.Text;

namespace MupenToolkitPRE.LowLevel
{
    public static class CStringHelper
    {
        public static string[] SplitAt(this string source, params int[] index)
        {
            index = index.Distinct().OrderBy(x => x).ToArray();
            string[] output = new string[index.Length + 1];
            int pos = 0;

            for (int i = 0; i < index.Length; pos = index[i++])
                output[i] = source.Substring(pos, index[i] - pos);

            output[index.Length] = source.Substring(pos);
            return output;
        }
        private static string TrimAfterNUL(this string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '\0')
                {
                    str = str.SplitAt(i)[0];
                    break;
                }
            }
            return str;
        }
        public static string ASCIIToStringTrimmed(this byte[] b)
        {
            return Encoding.ASCII.GetString(b).Trim().TrimStart().TrimEnd().TrimAfterNUL();
        }
        public static string UTF8ToStringTrimmed(this byte[] b)
        {
            return Encoding.ASCII.GetString(b).Trim().TrimStart().TrimEnd().TrimAfterNUL();
        }
    }
}
