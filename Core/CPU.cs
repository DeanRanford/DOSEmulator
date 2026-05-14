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
}

public interface IMemoryBus
{
    byte ReadByte(ushort seg, ushort offset);
    void WriteByte(ushort seg, ushort offset, byte value);
}

public sealed class FlatMemory : IMemoryBus
{
    private byte[] Memory { get; } = new byte[1024 * 1024];
    // Flatten Memory Location
    private static int Linear(ushort seg, ushort offset) => ((seg << 4) + offset) & 0xFFFFF;

    public byte ReadByte(ushort seg, ushort offset) => this.Memory[Linear(seg, offset)];
    public void WriteByte(ushort seg, ushort offset, byte value) => this.Memory[Linear(seg, offset)] = value;
}