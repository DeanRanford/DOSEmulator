namespace Core;

public sealed class CPU(IMemoryBus memory)
{
    // General Purpose Registers
    public ushort AX, BX, CX, DX;
    public ushort SI, DI, BP, SP;

    // Segment Registers
    public ushort CS, DS, ES, SS;

    // Instruction Pointer
    public ushort IP;

    // Status Register
    public ushort FLAGS;

    // Memory interface
    public IMemoryBus Memory { get; } = memory;

    public (bool success, string message) Step()
    {
        byte opcode;
        try
        {
            opcode = this.Memory.ReadByte(CS, IP++);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }

        switch (opcode)
        {
            case 0xBB: // MOV AX, imm16
                {
                    byte lo = this.Memory.ReadByte(CS, IP++);
                    byte hi = this.Memory.ReadByte(CS, IP++);
                    this.AX = (ushort)(lo | (hi << 8));
                    break;
                }

            // TODO: More OpCodes

            default:
                return (false, ($"Opcode {opcode:X2} not implemented"));

        }

        return (true, "ok");
    }

    public void LoadProgram(ushort cs, byte[] program)
    {
        const ushort loadOffset = 0x0100;
        for (var i = 0; i < program.Length; i++)
        {
            ushort offset = (ushort)(loadOffset + 1);
            this.Memory.WriteByte(cs, offset, program[i]);
        }
    }
}
