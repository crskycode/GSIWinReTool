using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace GSIWinReTool.Extensions
{
    public static class BinaryWriterExtensions
    {
        public static void WriteNullTerminatedString(this BinaryWriter writer, string value, Encoding encoding)
        {
            var bytes = encoding.GetBytes(value);
            writer.Write(bytes);
            writer.Write((byte)0);
        }

        public static void WriteCompressedString(this BinaryWriter writer, string value)
        {
            var bytes = Encoding.GetEncoding(932).GetBytes(value);

            for (var i = 0; i < bytes.Length; i += 2)
            {
                if (bytes[i] < 0x81)
                {
                    throw new Exception("This method does not support non Shift_JIS characters.");
                }

                var b1 = bytes[i];
                var b2 = bytes[i + 1];

                var v0 = (ushort)((b2 | (b1 << 8)) + 0x7D62);

                if (v0 < 0x54)
                {
                    writer.Write((byte)v0);
                }
                else
                {
                    writer.Write(b1);
                    writer.Write(b2);
                }
            }

            writer.Write((byte)0);
        }

        public static void WriteInt32BigEndian(this BinaryWriter writer, int value)
        {
            writer.Write(BinaryPrimitives.ReverseEndianness(value));
        }
    }
}
