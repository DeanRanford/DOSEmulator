namespace Core;

public sealed class FlatMemory : IMemoryBus
{
    private byte[] Memory { get; } = new byte[1024 * 1024];
    // Flatten Memory Location
    private static int Linear(ushort seg, ushort offset) => ((seg << 4) + offset) & 0xFFFFF;

    public byte ReadByte(ushort seg, ushort offset) => this.Memory[Linear(seg, offset)];
    public void WriteByte(ushort seg, ushort offset, byte value) => this.Memory[Linear(seg, offset)] = value;
}