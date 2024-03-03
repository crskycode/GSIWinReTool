using GSIWinReTool.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GSIWinReTool
{
    public partial class ScriptFile
    {
        private byte[] _codeBuffer;
        private List<int> _labels;
        private List<int> _exlabels;
        private readonly GSIWinReAssembly _assembly;

        public ScriptFile()
        {
            _codeBuffer = [];
            _labels = [];
            _exlabels = [];
            _assembly = new GSIWinReAssembly();
        }

        public void Load(string path)
        {
            var stream = File.OpenRead(path);
            var reader = new BinaryReader(stream);

            // Read header

            var labelCount = reader.ReadInt32();
            var exlabelCount = reader.ReadInt32();

            // Read labels

            if (labelCount > 0)
            {
                _labels = new List<int>(labelCount);

                for (var i = 0; i < labelCount; i++)
                {
                    _labels.Add(reader.ReadInt32());
                }
            }

            if (exlabelCount > 0)
            {
                _exlabels = new List<int>(exlabelCount);

                for (var i = 0; i < exlabelCount; i++)
                {
                    _exlabels.Add(reader.ReadInt32());
                }
            }

            // Read code data

            var codeLength = Convert.ToInt32(stream.Length - stream.Position);
            _codeBuffer = reader.ReadBytes(codeLength);

            Disassemble();

            // Finish

            stream.Dispose();
        }

        public void Save(string path)
        {
            var stream = File.Create(path);
            var writer = new BinaryWriter(stream);

            writer.Write(_labels.Count);
            writer.Write(_exlabels.Count);

            // Write labels

            for (var i = 0; i < _labels.Count; i++)
            {
                writer.Write(_labels[i]);
            }

            for (var i = 0; i < _exlabels.Count; i++)
            {
                writer.Write(_exlabels[i]);
            }

            // Write code data

            writer.Write(_codeBuffer);

            // Finish

            writer.Flush();
            stream.Dispose();
        }

        private void Disassemble()
        {
            var stream = new MemoryStream(_codeBuffer);
            var reader = new BinaryReader(stream);

            var stack = new List<int>(8);

            while (stream.Position < stream.Length)
            {
                var addr = stream.Position;
                var code = (GSIWinReInstruction)reader.ReadByte();

                switch (code)
                {
                    case GSIWinReInstruction.Unused:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Throw:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op02:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op03:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op04:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op05:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op06:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op07:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op08:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op09:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Msg:
                    {
                        reader.ReadCompressedString();
                        break;
                    }
                    case GSIWinReInstruction.Msg2:
                    {
                        reader.ReadNullTerminatedString();
                        break;
                    }
                    case GSIWinReInstruction.Op0C:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op0D:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op0E:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op0F:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op10:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op11:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op12:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Op13:
                    {
                        break;
                    }
                    case GSIWinReInstruction.CJump:
                    {
                        reader.ReadInt32BigEndian();
                        break;
                    }
                    case GSIWinReInstruction.Jump:
                    {
                        reader.ReadInt32BigEndian();
                        break;
                    }
                    case GSIWinReInstruction.Op16:
                    {
                        reader.ReadInt32BigEndian();
                        break;
                    }
                    case GSIWinReInstruction.Op17:
                    {
                        break;
                    }
                    case GSIWinReInstruction.CallFunc:
                    {
                        var funcId = stack[0];

                        switch (funcId)
                        {
                            case 0x01:
                            case 0x02:
                            case 0x03:
                            case 0x0A:
                            case 0x0B:
                            case 0x0C:
                            case 0x0D:
                            case 0x0E:
                            case 0x0F:
                            case 0x10:
                            case 0x11:
                            case 0x12:
                            case 0x13:
                            case 0x14:
                            case 0x15:
                            case 0x16:
                            case 0x17:
                            case 0x18:
                            case 0x19:
                            case 0x1A:
                            case 0x1B:
                            case 0x1C:
                            case 0x1D:
                                break;
                            default:
                                throw new Exception("Unknow function id.");
                        }

                        break;
                    }
                    case GSIWinReInstruction.Op19:
                    {
                        reader.ReadInt32BigEndian();
                        break;
                    }
                    case GSIWinReInstruction.Op1A:
                    {
                        reader.ReadInt32BigEndian();
                        break;
                    }
                    case GSIWinReInstruction.Call:
                    {
                        reader.ReadInt32BigEndian();
                        break;
                    }
                    case GSIWinReInstruction.Op1C:
                    {
                        reader.ReadByte();
                        break;
                    }
                    case GSIWinReInstruction.PushDword:
                    {
                        var v = reader.ReadInt32BigEndian();

                        stack.Insert(0, v);

                        if (stack.Count > 3)
                            stack.RemoveAt(stack.Count - 1);

                        break;
                    }
                    case GSIWinReInstruction.PushString:
                    {
                        reader.ReadNullTerminatedString();
                        break;
                    }
                    case GSIWinReInstruction.Add:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Sub:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Mul:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Div:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Mod:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Rand:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Land:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Lor:
                    {
                        break;
                    }
                    case GSIWinReInstruction.And:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Or:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Lt:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Gt:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Le:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Ge:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Eq:
                    {
                        break;
                    }
                    case GSIWinReInstruction.Ne:
                    {
                        break;
                    }
                    case GSIWinReInstruction.OpFA:
                    {
                        break;
                    }
                    case GSIWinReInstruction.OpFB:
                    {
                        break;
                    }
                    case GSIWinReInstruction.OpFC:
                    {
                        break;
                    }
                    case GSIWinReInstruction.OpFD:
                    {
                        break;
                    }
                    case GSIWinReInstruction.OpFE:
                    {
                        break;
                    }
                    case GSIWinReInstruction.OpFF:
                    {
                        break;
                    }
                    default:
                    {
                        throw new InvalidDataException("Unknow opcode.");
                    }
                }

                var length = Convert.ToInt32(stream.Position - addr);
                _assembly.Add(code, addr, length);
            }
        }

        public void ExportText(string path)
        {
            var writer = File.CreateText(path);

            var stream = new MemoryStream(_codeBuffer);
            var reader = new BinaryReader(stream);

            foreach (var rec in _assembly)
            {
                string s = string.Empty;

                if (rec.Instruction == GSIWinReInstruction.Msg)
                {
                    stream.Position = rec.Addr + 1;
                    s = reader.ReadCompressedString();
                }
                else if (rec.Instruction == GSIWinReInstruction.Msg2 ||
                    rec.Instruction == GSIWinReInstruction.PushString)
                {
                    stream.Position = rec.Addr + 1;
                    s = reader.ReadNullTerminatedString();
                }

                if (!string.IsNullOrWhiteSpace(s) && s[0] > 0x80)
                {
                    writer.WriteLine("◇{0:X8}◇{1}", rec.Addr, s);
                    writer.WriteLine("◆{0:X8}◆{1}", rec.Addr, s);
                    writer.WriteLine();
                }
            }

            writer.Flush();
            writer.Dispose();
        }

        [GeneratedRegex(@"◆(\w+)◆(.+$)")]
        private static partial Regex TextLineRegex();

        private static Dictionary<long, string> LoadTranslation(string path)
        {
            using var reader = File.OpenText(path);

            var dict = new Dictionary<long, string>();
            var num = 0;

            while (!reader.EndOfStream)
            {
                var n = num;
                var line = reader.ReadLine();
                num++;

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line[0] != '◆')
                {
                    continue;
                }

                var match = TextLineRegex().Match(line);

                if (match.Groups.Count != 3)
                {
                    throw new Exception($"Bad format at line: {n}");
                }

                var addr = long.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
                var text = match.Groups[2].Value.Unescape();

                dict.Add(addr, text);
            }

            reader.Close();

            return dict;
        }

        public void ImportText(string path)
        {
            Console.WriteLine("Loading translation...");

            var translation = LoadTranslation(path);

            Console.WriteLine("Preparing to rebuild...");

            var instMap = _assembly.Instructions.ToDictionary(x => x.Addr);

            var codeStream = new MemoryStream(_codeBuffer);
            var codeReader = new BinaryReader(codeStream);

            var outStream = new MemoryStream(_codeBuffer.Length * 2);
            var outWriter = new BinaryWriter(outStream);

            Console.WriteLine("Building code...");

            foreach (var rec in _assembly.Instructions)
            {
                rec.NewAddr = Convert.ToInt32(outStream.Position);

                if (rec.Instruction == GSIWinReInstruction.Msg)
                {
                    if (translation.TryGetValue(rec.Addr, out string? text))
                    {
                        outWriter.Write((byte)rec.Instruction);
                        outWriter.WriteCompressedString(text);
                    }
                    else
                    {
                        outWriter.Write(_codeBuffer, rec.Addr, rec.Length);
                    }
                }
                else if (rec.Instruction == GSIWinReInstruction.Msg2 ||
                    rec.Instruction == GSIWinReInstruction.PushString)
                {
                    if (translation.TryGetValue(rec.Addr, out string? text))
                    {
                        outWriter.Write((byte)rec.Instruction);
                        outWriter.WriteNullTerminatedString(text);
                    }
                    else
                    {
                        outWriter.Write(_codeBuffer, rec.Addr, rec.Length);
                    }
                }
                else
                {
                    outWriter.Write(_codeBuffer, rec.Addr, rec.Length);
                }
            }

            Console.WriteLine("Fixing code...");

            foreach (var rec in _assembly.Instructions)
            {
                if (rec.Instruction == GSIWinReInstruction.CJump ||
                    rec.Instruction == GSIWinReInstruction.Jump ||
                    rec.Instruction == GSIWinReInstruction.Op16 ||
                    rec.Instruction == GSIWinReInstruction.Call)
                {
                    codeStream.Position = rec.Addr + 1;
                    outStream.Position = rec.NewAddr + 1;

                    var dest = codeReader.ReadInt32BigEndian();
                    var newDest = instMap[dest].NewAddr;

                    // Console.WriteLine("Fix offset at {0:X8} : {1:X8} -> {2:X8}", outCodeStream.Position, dest, newDest);

                    outWriter.WriteInt32BigEndian(newDest);
                }
            }

            Console.WriteLine("Updating labels...");

            for (var i = 0; i < _labels.Count; i++)
            {
                _labels[i] = instMap[_labels[i]].NewAddr;
            }

            for (var i = 0; i < _exlabels.Count; i++)
            {
                _exlabels[i] = instMap[_exlabels[i]].NewAddr;
            }

            Console.WriteLine("Flush code...");

            _codeBuffer = outStream.ToArray();

            Console.WriteLine("Rebuild finished.");
        }
    }
}
