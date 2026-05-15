namespace Core;

public interface IMemoryBus
{
    byte ReadByte(ushort seg, ushort offset);
    void WriteByte(ushort seg, ushort offset, byte value);
}
