using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponycode_Editor
{
    public class BitOperations
    {
        /// <summary>
        /// decodes Character for int value i
        /// </summary>
        /// <param name="i"></param>
        /// <returns>decoded Character</returns>
        static public char GetCharacter(int i)
        {
            char result = ' ';
            // A: 65 - Z: 90
            if (i >= 26 && i < 52)
            {
                result = (char)(i + 65 - 26);
            }
            // a: 97 - z: 122
            if (i >= 0 && i < 26)
            {
                result = (char)(i + 97);
            }
            return result;
        }

        static public int GetIndex(char c)
        {
            int result = 62;
            int i = (int)c;
            // A: 65 - Z: 90
            if (i >= 65 && i < 91)
            {
                result = i - 65 + 26;
            }
            // a: 97 - z: 122
            if (i >= 97 && i < 123)
            {
                result = i - 97;
            }
            return result;
        }

        /// <summary>
        /// returns n bits from start of inputBits
        /// </summary>
        /// <param name="inputBits"></param>
        /// <param name="start"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        static public BitArray GetBits(BitArray inputBits, int start, int n)
        {
            if (start + n > inputBits.Length)
            {
                n = inputBits.Length - start;
            }
            
            BitArray outputBits = new BitArray(n);

            for (int i = 0; i < n; i++)
            {
                outputBits[i] = inputBits[i + start];
            }

            return outputBits;
        }

        /// <summary>
        /// overwrites destination with update starting at pos
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="update"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        static public BitArray UpdateBitArray(BitArray destination, BitArray update, int pos, int len)
        {
            // initialize new bitarry
            int length = destination.Length;
            if (pos + len > destination.Length)
            {
                length = pos + len;
            }
            BitArray newArray = new BitArray(length);

            // copy original
            for (int i = 0; i < destination.Length; i++)
            {
                newArray[i] = destination[i];
            }

            // update and append
            for (int i = 0; i < len; i++)
            {
                newArray[i + pos] = update[i];
            }
            return newArray;
        }
    }
}
