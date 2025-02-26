using System.Collections;

internal class DNSMessage
{
    internal byte[] Message => GetMessage();

    private byte[] GetMessage()
    {
        var bitArray = new BitArray(96);
        bitArray.SetAll(false);
        CopyBitArray(_packetIdentifierBits, bitArray, 0);
        bitArray.Set(17, true);
        var result = new byte[12];
        bitArray.CopyTo(result, 0);
        return result;
    }

    internal ushort PacketIdentifier { get; set; } = 1234;

    private BitArray _packetIdentifierBits => new BitArray(BitConverter.GetBytes(PacketIdentifier));

    private static void CopyBitArray(BitArray source, BitArray destination, int startIndex)
    {
        if (startIndex + source.Length > destination.Length)
        {
            throw new ArgumentException("Destination BitArray is not large enough");
        }

        for (int i = 0; i < source.Length; i++)
        {
            destination[startIndex + i] = source[i];
        }
    }
}