namespace GSIWinReTool
{
    public enum GSIWinReInstruction
    {
        Unused = 0x00,
        Throw = 0x01,
        Op02 = 0x02,
        Op03 = 0x03,
        Op04 = 0x04,
        Op05 = 0x05,
        Op06 = 0x06,
        Op07 = 0x07,
        Op08 = 0x08,
        Op09 = 0x09,
        Msg = 0x0A, // string(c)
        Msg2 = 0x0B, // string
        Op0C = 0x0C,
        Op0D = 0x0D,
        Op0E = 0x0E,
        Op0F = 0x0F,
        Op10 = 0x10,
        Op11 = 0x11,
        Op12 = 0x12,
        Op13 = 0x13,
        CJump = 0x14, // addr
        Jump = 0x15, // addr
        Op16 = 0x16, // addr
        Op17 = 0x17,
        CallFunc = 0x18, // ?
        Op19 = 0x19, // dword
        Op1A = 0x1A, // dword
        Call = 0x1B, // addr
        Op1C = 0x1C, // byte
        PushDword = 0x32, // dword
        PushString = 0x33, // string
        Add = 0x34,
        Sub = 0x35,
        Mul = 0x36,
        Div = 0x37,
        Mod = 0x38,
        Rand = 0x39,
        Land = 0x3A,
        Lor = 0x3B,
        And = 0x3C,
        Or = 0x3D,
        Lt = 0x3E,
        Gt = 0x3F,
        Le = 0x40,
        Ge = 0x41,
        Eq = 0x42,
        Ne = 0x43,
        OpFA = 0xFA,
        OpFB = 0xFB,
        OpFC = 0xFC,
        OpFD = 0xFD,
        OpFE = 0xFE,
        OpFF = 0xFF,
    }
}
