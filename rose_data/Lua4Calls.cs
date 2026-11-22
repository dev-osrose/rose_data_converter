using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace rose_data
{
    /// <summary>
    /// The calls a compiled Lua 4.0 chunk (the conversation files' scripts)
    /// makes with a literal first argument: <c>GETGLOBAL f; PUSHSTRING s</c>,
    /// recorded with the global function the code is in.
    /// </summary>
    public static class Lua4Calls
    {
        public struct Call
        {
            public string Function; // the enclosing global function, "" for the main chunk
            public string Callee;
            public string Argument; // null when the first argument is not a literal
        }

        // Lua 4.0 opcodes (lopcodes.h) that matter here.
        private const int OpPushString = 7;
        private const int OpGetGlobal = 12;
        private const int OpSetGlobal = 19;
        private const int OpClosure = 48;

        private class Proto
        {
            public List<string> Strings = new List<string>();
            public List<Proto> Protos = new List<Proto>();
            public uint[] Code = Array.Empty<uint>();
        }

        /// <summary>Every call in <paramref name="chunk"/> to one of <paramref name="callees"/>.</summary>
        public static List<Call> Find(byte[] chunk, ISet<string> callees)
        {
            var reader = new BinaryReader(new MemoryStream(chunk));
            if (reader.ReadByte() != 0x1B || reader.ReadByte() != (byte)'L' || reader.ReadByte() != (byte)'u' ||
                reader.ReadByte() != (byte)'a' || reader.ReadByte() != 0x40)
                throw new InvalidDataException("not a Lua 4.0 chunk");
            // Endianness, the sizes of int, size_t, Instruction, its fields and
            // a Number, then a test Number.
            reader.ReadBytes(8);
            reader.ReadDouble();
            var calls = new List<Call>();
            Scan(ReadProto(reader), "", callees, calls);
            return calls;
        }

        private static Proto ReadProto(BinaryReader r)
        {
            var p = new Proto();
            ReadString(r); // source
            r.ReadInt32(); // line defined
            r.ReadInt32(); // parameters
            r.ReadByte(); // vararg
            r.ReadInt32(); // max stack
            var locals = r.ReadInt32();
            for (var i = 0; i < locals; i++)
            {
                ReadString(r);
                r.ReadInt32();
                r.ReadInt32();
            }
            r.ReadBytes(4 * r.ReadInt32()); // line info
            var strings = r.ReadInt32();
            for (var i = 0; i < strings; i++) p.Strings.Add(ReadString(r));
            r.ReadBytes(8 * r.ReadInt32()); // numbers
            var protos = r.ReadInt32();
            for (var i = 0; i < protos; i++) p.Protos.Add(ReadProto(r));
            var code = r.ReadInt32();
            p.Code = new uint[code];
            for (var i = 0; i < code; i++) p.Code[i] = r.ReadUInt32();
            return p;
        }

        private static string ReadString(BinaryReader r)
        {
            var size = r.ReadUInt32();
            if (size == 0) return null;
            var bytes = r.ReadBytes((int)size);
            return Encoding.Latin1.GetString(bytes, 0, bytes.Length - 1);
        }

        private static void Scan(Proto p, string function, ISet<string> callees, List<Call> calls)
        {
            Proto closure = null;
            for (var i = 0; i < p.Code.Length; i++)
            {
                var op = (int)(p.Code[i] & 0x3F);
                var u = (int)(p.Code[i] >> 6);
                if (op == OpGetGlobal && callees.Contains(p.Strings[u]))
                {
                    string argument = null;
                    if (i + 1 < p.Code.Length && (p.Code[i + 1] & 0x3F) == OpPushString)
                        argument = p.Strings[(int)(p.Code[i + 1] >> 6)];
                    calls.Add(new Call { Function = function, Callee = p.Strings[u], Argument = argument });
                }
                else if (op == OpClosure)
                {
                    // One not stored in a global is scanned as part of this function.
                    if (closure != null) Scan(closure, function, callees, calls);
                    closure = p.Protos[(int)(p.Code[i] >> 15)];
                }
                else if (op == OpSetGlobal && closure != null)
                {
                    Scan(closure, p.Strings[u], callees, calls);
                    closure = null;
                }
            }
            if (closure != null) Scan(closure, function, callees, calls);
        }
    }
}
