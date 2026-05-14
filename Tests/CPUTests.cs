using Core;

namespace Tests;

public static class ProgramRepository
{
    public static readonly byte[] BasicProgram =
    [
        0xB8, 0x41, 0x00, // MOV AX, 0x0041
        0xCD, 0x21,       // INT 21h
        0xF4              // HLT
    ];
}

public sealed class TestMachine
{
    public CPU CPU { get; }
    public FlatMemory Memory { get; }
    public TestMachine(byte[] program)
    {
        this.Memory = new FlatMemory();
        this.CPU = new CPU(this.Memory);

        ushort cs = 0x1000;
        this.CPU.LoadProgram(cs, program);

        this.CPU.CS = cs;
        this.CPU.IP = 0x0100;
        this.CPU.DS = cs;
        this.CPU.ES = cs;
        this.CPU.SS = (ushort)(cs + 0x100);
        this.CPU.SP = (0xFFFE);
    }

    public void Run(int maxCycles)
    {
        for (var i = 0; i < maxCycles; i++)
        {
            if (!this.CPU.Step().success) break;
        }
    }
}
public class CPUTests
{
    [Fact]
    public void CPUTestBasicOpCode()
    {
        var m = new TestMachine(ProgramRepository.BasicProgram);

        var halted = false;
        while (!halted)
        {
            byte opcode = m.Memory.ReadByte(m.CPU.CS, m.CPU.IP);
            m.CPU.Step();
            halted = opcode == 0xF4; // HLT
        }
    }
}
