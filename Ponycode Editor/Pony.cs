using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponycode_Editor
{
    public class Pony
    {
        // Ponycode formatting
        //(2-bit charactertype) character type(race)
        //(3 8-bit colors) 24 bits hair color 0
        //(3 8-bit colors) 24 bits hair color 1
        //(3 8-bit colors) 24 bits body color
        //(3 8-bit colors) 24 bits eye color
        //(3 8-bit colors) 24 bits hoof color
        //(8-bit short) 8 bits mane type
        //(8-bit short) 8 bits tail type
        //(8-bit short) 8 bits eye type
        //(8-bit short) 5 bits hoof type
        //(3 10-bit ints) 30 bits cutie mark type
        //(2-bit gender) 2 bits gender
        //(32-bit single) 32 bits body type
        //(32-bit float) 32 bits horn size
        //(6-bit char[]) name

        const int PonyCode_SizeBits = 252;  // Size of PonyCode minus name
        private byte[] pc;
        public byte[] PonyCode
        {
            get { return this.pc; }
            set
            {
                this.pc = value;
                parsePonyCode(value);
            }
        }

        private int PonyData_Size = 43; // Size of the PonyData minus the name
        private byte[] pd;
        public byte[] PonyData
        {
            get { return this.pd; }
            set
            {
                this.pd = value;
                parsePonyData(value);
            }
        }

        public byte Race;
        public byte Gender;
        public byte[] BodyColor = new byte[3];
        public byte[] EyeColor = new byte[3];
        public byte[] ManeColor1 = new byte[3];
        public byte[] ManeColor2 = new byte[3];
        public byte[] HoofColor = new byte[3];
        public int[] CutieMarks = new int[3];
        public byte ManeType;
        public byte TailType;
        public byte EyeType;
        public byte HoofType;
        public Single BodySize;
        public Single HornSize;
        public String Name;

        /// <summary>
        /// fills the fields with data from value
        /// </summary>
        /// <param name="value"></param>
        private void parsePonyCode(byte[] value)
        {
            BitArray ponycodeBits = new BitArray(value);
            int index = 0;
            int fieldwidth;
            byte[] readByte = new byte[1];

            //(2-bit charactertype) character type(race)
            fieldwidth = 2;
            BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(readByte, 0);
            this.Race = readByte[0];
            index += fieldwidth;

            //(3 8-bit colors) 24 bits hair color 0
            fieldwidth = 8;
            for (int i = 0; i < 3; i++)
            {
                BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(ManeColor1, i);
                index += fieldwidth;
            }

            //(3 8-bit colors) 24 bits hair color 1
            fieldwidth = 8;
            for (int i = 0; i < 3; i++)
            {
                BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(ManeColor2, i);
                index += fieldwidth;
            }

            //(3 8-bit colors) 24 bits body color
            fieldwidth = 8;
            for (int i = 0; i < 3; i++)
            {
                BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(BodyColor, i);
                index += fieldwidth;
            }

            //(3 8-bit colors) 24 bits eye color
            fieldwidth = 8;
            for (int i = 0; i < 3; i++)
            {
                BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(EyeColor, i);
                index += fieldwidth;
            }

            //(3 8-bit colors) 24 bits hoof color
            fieldwidth = 8;
            for (int i = 0; i < 3; i++)
            {
                BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(HoofColor, i);
                index += fieldwidth;
            }

            //(8-bit short) 8 bits mane type
            fieldwidth = 8;
            BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(readByte, 0);
            this.ManeType = readByte[0];
            index += fieldwidth;

            //(8-bit short) 8 bits tail type
            fieldwidth = 8;
            BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(readByte, 0);
            this.TailType = readByte[0];

            index += fieldwidth;

            //(8-bit short) 8 bits eye type
            fieldwidth = 8;
            BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(readByte, 0);
            this.EyeType = readByte[0];

            index += fieldwidth;

            //(8-bit short) 5 bits hoof type
            fieldwidth = 5;
            BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(readByte, 0);
            this.HoofType = readByte[0];
            index += fieldwidth;

            //(3 10-bit ints) 30 bits cutie mark type
            fieldwidth = 10;
            for (int i = 0; i < 3; i++)
            {
                BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(CutieMarks, i);
                index += fieldwidth;
            }


            // get 2 bits starting at 181 as gender
            fieldwidth = 2;
            BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(readByte, 0);
            this.Gender = readByte[0];

            index += fieldwidth;

            // get 32 bits starting at 183 as body size
            fieldwidth = 32;
            byte[] byteBodySize = new byte[4];
            BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(byteBodySize, 0);
            this.BodySize = BitConverter.ToSingle(byteBodySize, 0);
            index += fieldwidth;

            // get 32 bits starting at 215 as horn size
            fieldwidth = 32;
            byte[] byteHornSize = new byte[4];
            BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(byteHornSize, 0);
            this.HornSize = BitConverter.ToSingle(byteHornSize, 0);
            index += fieldwidth;

            // get the name starting at 247
            // first 5 bits are string length
            fieldwidth = 5;
            int[] num = new int[1];
            BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(num, 0);
            int namelength = num[0] + 1;
            index += 5;

            // parse the 6bit characters
            fieldwidth = 6;
            if (index + (fieldwidth * namelength) > ponycodeBits.Length)
            {
                throw new Exception("Decoding Error: Name length missmatch");
            }

            int[] charIndexes = new int[(ponycodeBits.Length- index)/fieldwidth];
            this.Name = "";

            //int ind = 0;
            //while (index + fieldwidth <= ponycodeBits.Length)
            //{
            //    BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(charIndexes, ind);
            //    this.Name += BitOperations.GetCharacter(charIndexes[ind]);
            //    index += fieldwidth;
            //    ind++;
            //}
            for (int i = 0; i < namelength; i++)
            {
                BitOperations.GetBits(ponycodeBits, index, fieldwidth).CopyTo(charIndexes, i);
                this.Name += BitOperations.GetCharacter(charIndexes[i]);
                index += fieldwidth;
            }
        }

        /// <summary>
        /// updates ponycode changes from pony
        /// </summary>
        /// <returns>base64 encoded string</returns>
        public string calculatePonyCode() 
        {
            String result = "";
            BitArray ponycodeBits = new BitArray(PonyCode_SizeBits);
            BitArray updateBits;
            int index = 0;
            int fieldwidth;

            // 2 bits starting at 0 as race
            fieldwidth = 2;
            updateBits = new BitArray(new byte[] { this.Race });
            ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, updateBits, 0, 2);
            index += fieldwidth;

            //(3 8-bit colors) 24 bits hair color 0
            fieldwidth = 8;
            for (int i = 0; i < 3; i++)
            {
                updateBits = new BitArray(new byte[] { this.ManeColor1[i] });
                ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, updateBits, index, fieldwidth);
                index += fieldwidth;
            }

            //(3 8-bit colors) 24 bits hair color 1
            fieldwidth = 8;
            for (int i = 0; i < 3; i++)
            {
                updateBits = new BitArray(new byte[] { this.ManeColor2[i] });
                ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, updateBits, index, fieldwidth);
                index += fieldwidth;
            }

            //(3 8-bit colors) 24 bits body color
            fieldwidth = 8;
            for (int i = 0; i < 3; i++)
            {
                updateBits = new BitArray(new byte[] { this.BodyColor[i] });
                ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, updateBits, index, fieldwidth);
                index += fieldwidth;
            }

            //(3 8-bit colors) 24 bits eye color
            fieldwidth = 8;
            for (int i = 0; i < 3; i++)
            {
                updateBits = new BitArray(new byte[] { this.EyeColor[i] });
                ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, updateBits, index, fieldwidth);
                index += fieldwidth;
            }

            //(3 8-bit colors) 24 bits hoof color
            fieldwidth = 8;
            for (int i = 0; i < 3; i++)
            {
                updateBits = new BitArray(new byte[] { this.HoofColor[i] });
                ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, updateBits, index, fieldwidth);
                index += fieldwidth;
            }

            //(8-bit short) 8 bits mane type
            fieldwidth = 8;
            updateBits = new BitArray(new byte[] { this.ManeType });
            ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, updateBits, index, fieldwidth);
            index += fieldwidth;

            //(8-bit short) 8 bits tail type
            fieldwidth = 8;
            updateBits = new BitArray(new byte[] { this.TailType });
            ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, updateBits, index, fieldwidth);
            index += fieldwidth;

            //(8-bit short) 8 bits eye type
            fieldwidth = 8;
            updateBits = new BitArray(new byte[] { this.EyeType });
            ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, updateBits, index, fieldwidth);
            index += fieldwidth;

            //(8-bit short) 5 bits hoof type
            fieldwidth = 5;
            updateBits = new BitArray(new byte[] { this.HoofType });
            ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, updateBits, index, fieldwidth);
            index += fieldwidth;

            //(3 10-bit ints) at 151 as 30 bit cutie mark
            fieldwidth = 10;
            for (int i = 0; i < 3; i++)
            {
                BitArray cutieMarkBits = new BitArray(BitConverter.GetBytes(this.CutieMarks[i]));
                ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, cutieMarkBits, index, fieldwidth);
                index += fieldwidth;
            }

            // 2 bits starting at 181 as gender
            fieldwidth = 2;
            BitArray gender = new BitArray(new byte[] { this.Gender });
            ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, gender, index, fieldwidth);
            index += fieldwidth;

            // 4 byte float starting at 183 as body size
            fieldwidth = 32;
            BitArray bodySizeBits = new BitArray(BitConverter.GetBytes(this.BodySize));
            ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, bodySizeBits, index, fieldwidth);
            index += fieldwidth;

            // 4 byte float starting at 215 as horn size
            fieldwidth = 32;
            BitArray hornSizeBits = new BitArray(BitConverter.GetBytes(this.HornSize));
            ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, hornSizeBits, index, fieldwidth);
            index += fieldwidth;

            // 5 bit int starting at 247 as name.length
            fieldwidth = 5;
            BitArray nameLenBits = new BitArray(BitConverter.GetBytes(Name.Length-1));
            ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, nameLenBits, index, fieldwidth);            
            index += fieldwidth;

            // name.length * 6 bit coded name
            fieldwidth = 6;
            for (int i = 0; i < Name.Length; i++)
            {
                BitArray nameBits = new BitArray(BitConverter.GetBytes(BitOperations.GetIndex(Name[i])));
                ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, nameBits, index, fieldwidth);
                index += fieldwidth;
            }

            // add a 6bit terminator
            ponycodeBits = BitOperations.UpdateBitArray(ponycodeBits, new BitArray(fieldwidth, false), index, fieldwidth);
            index += fieldwidth;

            if (index < ponycodeBits.Length)
            {
                ponycodeBits = BitOperations.GetBits(ponycodeBits, 0, index);
            }


            byte[] newPonycode = new byte[(ponycodeBits.Length - 1) / 8 + 1];
            ponycodeBits.CopyTo(newPonycode, 0);
            result = Convert.ToBase64String(newPonycode);

            return result;
        }

        private void parsePonyData(byte[] value)
        {
            int index;
            int fieldwidth;

            // parse name length
            index = 0;
            int strlen;
            fieldwidth = 0;
            Byte num3; int num = 0, num2 = 0;
            do
            {
                num3 = value[index + fieldwidth];
                fieldwidth++;
                num |= (num3 & 0x7f) << num2;
                num2 += 7;
            } while ((num3 & 0x80) != 0);
            strlen = num;
            index += fieldwidth;

            if (PonyData_Size + strlen + fieldwidth != value.Length)
            {
                throw new Exception("Decoding Error: Ponydata length missmatch");
            }

            // parse name
            byte[] nameBytes = new byte[strlen];
            this.Name = Encoding.UTF8.GetString(value, index, strlen);
            index += strlen;

            // parse race
            fieldwidth = 1;
            this.Race = value[index];
            index += fieldwidth;

            // parse gender
            fieldwidth = 1;
            this.Gender = value[index];
            index += fieldwidth;

            // parse CutieMark
            fieldwidth = 4;
            for (int i = 0; i < 3; i++)
            {
                this.CutieMarks[i] = BitConverter.ToInt32(value, index);
                index += fieldwidth;
            }

            // parse hair color 0
            fieldwidth = 1;
            for (int i = 0; i < 3; i++)
            {
                this.ManeColor1[i] = value[index];
                index += fieldwidth;
            }

            // parse hair color 1
            fieldwidth = 1;
            for (int i = 0; i < 3; i++)
            {
                this.ManeColor2[i] = value[index];
                index += fieldwidth;
            }

            // parse body color
            fieldwidth = 1;
            for (int i = 0; i < 3; i++)
            {
                this.BodyColor[i] = value[index];
                index += fieldwidth;
            }

            // parse eye color
            fieldwidth = 1;
            for (int i = 0; i < 3; i++)
            {
                this.EyeColor[i] = value[index];
                index += fieldwidth;
            }

            // parse hoof color
            fieldwidth = 1;
            for (int i = 0; i < 3; i++)
            {
                this.HoofColor[i] = value[index];
                index += fieldwidth;
            }

            // parse mane
            fieldwidth = 2;
            this.ManeType = value[index]; // we ignore the following byte
            index += fieldwidth;

            // parse tail
            fieldwidth = 2;
            this.TailType = value[index]; // we ignore the following byte
            index += fieldwidth;

            // parse eye
            fieldwidth = 2;
            this.EyeType = value[index]; // we ignore the following byte
            index += fieldwidth;

            // parse hoof
            fieldwidth = 2;
            this.HoofType = value[index]; // we ignore the following byte
            index += fieldwidth;

            // parse 16bit float as Hornsize
            fieldwidth = 2;
            index = value.Length - fieldwidth;

            // take the 16 bit and fill it up to 32 bit
            byte[] hornSizeBytes2 = new byte[2];
            Array.Copy(value, index, hornSizeBytes2, 0, fieldwidth);
            BitArray hornSizeBits32 = new BitArray(new byte[] { 0x00, 0x00, 0x00, 0x3c }); // binary representation is reverse to IEEE 754 converter http://www.h-schmidt.net/FloatConverter/IEEE754.html
            BitArray hornSizeBits16 = new BitArray(hornSizeBytes2);
            hornSizeBits32 = BitOperations.UpdateBitArray(hornSizeBits32, hornSizeBits16, 10, 16);
            
            // convert the 32bit to float
            byte[] hornSizeBytes4 = new byte[4];
            hornSizeBits32.CopyTo(hornSizeBytes4, 0);
            this.HornSize = BitConverter.ToSingle(hornSizeBytes4, 0);
            

            // parse float Bodysize
            fieldwidth = 4;
            index -= fieldwidth;
            this.BodySize = BitConverter.ToSingle(value, index);
        }

        public string calculatePonyData()
        {
            string result = "";
            byte[] newPonyData = new byte[1 + this.Name.Length + PonyData_Size];
            int index = 0;
            int fieldwidth;

            //name length
            fieldwidth = 1;
            newPonyData[index] = Convert.ToByte(this.Name.Length);
            index += fieldwidth;

            //name
            fieldwidth = this.Name.Length;
            System.Buffer.BlockCopy(Encoding.UTF8.GetBytes(this.Name), 0, newPonyData, index, fieldwidth);
            index += fieldwidth;

            // race
            fieldwidth = 1;
            newPonyData[index] = this.Race;
            index += fieldwidth;

            // gender
            fieldwidth = 1;
            newPonyData[index] = this.Gender;
            index += fieldwidth;

            // CutieMark
            fieldwidth = 4;
            for (int i = 0; i < 3; i++)
            {
                System.Buffer.BlockCopy(BitConverter.GetBytes(this.CutieMarks[i]), 0, newPonyData, index, fieldwidth);
                index += fieldwidth;
            }

            // Mane Color 1
            fieldwidth = 1;
            for (int i = 0; i < 3; i++)
            {
                newPonyData[index] = this.ManeColor1[i];
                index += fieldwidth;
            }

            // Mane Color 2
            fieldwidth = 1;
            for (int i = 0; i < 3; i++)
            {
                newPonyData[index] = this.ManeColor2[i];
                index += fieldwidth;
            }

            // Body Color
            fieldwidth = 1;
            for (int i = 0; i < 3; i++)
            {
                newPonyData[index] = this.BodyColor[i];
                index += fieldwidth;
            }

            // Eye Color
            fieldwidth = 1;
            for (int i = 0; i < 3; i++)
            {
                newPonyData[index] = this.EyeColor[i];
                index += fieldwidth;
            }

            // Hoof Color
            fieldwidth = 1;
            for (int i = 0; i < 3; i++)
            {
                newPonyData[index] = this.HoofColor[i];
                index += fieldwidth;
            }

            // Mane
            fieldwidth = 2;
            newPonyData[index] = this.ManeType;
            index += fieldwidth;

            // Tail
            fieldwidth = 2;
            newPonyData[index] = this.TailType;
            index += fieldwidth;

            // Eye
            fieldwidth = 2;
            newPonyData[index] = this.EyeType;
            index += fieldwidth;

            // Hoof
            fieldwidth = 2;
            newPonyData[index] = this.HoofType;
            index += fieldwidth;

            // Body Size
            fieldwidth = 4;
            System.Buffer.BlockCopy(BitConverter.GetBytes(this.BodySize), 0, newPonyData, index, fieldwidth);
            index += fieldwidth;

            // Horn Size
            fieldwidth = 2;
            byte[] hornSizeBytes4 = BitConverter.GetBytes(this.HornSize);
            BitArray hornSizeBits32 = new BitArray(hornSizeBytes4);
            BitArray hornSizeBits16 = BitOperations.GetBits(hornSizeBits32, 10, 16);
            byte[] hornSizeBytes2 = new byte[2];
            hornSizeBits16.CopyTo(hornSizeBytes2, 0);

            System.Buffer.BlockCopy(hornSizeBytes2, 0, newPonyData, index, fieldwidth);
            index += fieldwidth;

            if (index != newPonyData.Length)
                throw new Exception("Error calculating PonyData: index: " + index + ", expected: " + newPonyData.Length);

            result = Convert.ToBase64String(newPonyData);
            return result;
        }
    }
}
