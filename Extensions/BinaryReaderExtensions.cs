using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GSIWinReTool.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static string ReadNullTerminatedString(this BinaryReader reader)
        {
            var bytes = new List<byte>(1024);

            for (var c = reader.ReadByte(); c != 0; c = reader.ReadByte())
            {
                bytes.Add(c);
            }

            if (bytes.Count > 0)
            {
                return Encoding.GetEncoding(932).GetString(bytes.ToArray());
            }

            return string.Empty;
        }

        public static string ReadCompressedString(this BinaryReader reader)
        {
            var bytes = new List<byte>(1024);

            for (var c = reader.ReadByte(); c != 0; c = reader.ReadByte())
            {
                if (c >= 0x81 && c <= 0x9F || (byte)(c + 0x20) <= 0xF)
                {
                    bytes.Add(c);
                    c = reader.ReadByte();
                    bytes.Add(c);
                }
                else
                {
                    // Unpack
                    var v0 = (ushort)(c - 0x7D62);
                    var v1 = (byte)(v0 >> 8);
                    var v2 = (byte)(v0 & 0xFF);
                    bytes.Add(v1);
                    bytes.Add(v2);
                }
            }

            if (bytes.Count > 0)
            {
                return Encoding.GetEncoding(932).GetString(bytes.ToArray());
            }

            return string.Empty;
        }

        public static int ReadInt32BigEndian(this BinaryReader reader)
        {
            return BinaryPrimitives.ReverseEndianness(reader.ReadInt32());
        }
    }
}
