using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Helper
{
    public static class BitopHelper
    {
        public static bool GetBit(int value, int bitIndex)
        {
            return 1 == ((value >> bitIndex) & 1);
        }
        public static bool GetBit(uint value, int bitIndex)
        {
            return 1 == ((value >> bitIndex) & 1);
        }
        public static sbyte GetSByte(int value, int byteIndex)
        {
            return (sbyte)((value >> (8 * byteIndex)) & 0xFF);
        }
        public unsafe static void SetByte(int* val, sbyte b, int pos)
        {
            *val &= ~((int)0xff << (8 * pos));
            *val |= ((int)b << (8 * pos));
        }
        public static void SetBit(ref int value, bool bitval, int bitpos)
        {
            if (!bitval) value &= ~(1 << bitpos); else value |= 1 << bitpos;
        }
        public static void SetBit(ref uint value, bool bitval, int bitpos)
        {
            if (!bitval) value &= ~(uint)((1 << bitpos)); else value |= (uint)(1 << bitpos);
        }

    }
}
