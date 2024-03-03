using System;
using System.Collections.Generic;
using System.Linq;

namespace GSIWinReTool
{
    public class GSIWinReAssembly
    {
        public LinkedList<GSIWinReInstructionRecord> Instructions { get; }

        public GSIWinReAssembly()
        {
            Instructions = new LinkedList<GSIWinReInstructionRecord>();
        }

        public void Add(GSIWinReInstruction instruction, long addr, long length)
        {
            var inst = new GSIWinReInstructionRecord();

            inst.Instruction = instruction;
            inst.Addr = Convert.ToInt32(addr);
            inst.NewAddr = Convert.ToInt32(addr);
            inst.Length = Convert.ToInt32(length);

            Instructions.AddLast(inst);
        }

        public IEnumerator<GSIWinReInstructionRecord> GetEnumerator()
        {
            return Instructions.GetEnumerator();
        }

        public int Count
        {
            get => Instructions.Count;
        }

        public int TotalLength
        {
            get => Instructions.Sum(x => x.Length);
        }
    }
}
