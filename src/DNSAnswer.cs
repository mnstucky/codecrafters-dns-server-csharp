using System.Text;

internal record DNSAnswer(string Name,
                          int Type,
                          int Class,
                          int TimeToLive,
                          IEnumerable<DNSIPAddress> Addresses)
{
    internal byte[] AnswerBytes => GetAnswerBytes();

    private byte[] GetAnswerBytes()
    {
        var result = new List<byte>();
        foreach (var label in Name.Split('.'))
        {
            result.AddRange(Encoding.ASCII.GetBytes(label));
        }
        result.Add(0x00);
        result.Add((byte)(Type >> 8));
        result.Add((byte)(Type & 0b11111111));
        result.Add((byte)(Class >> 8));
        result.Add((byte)(Class & 0b11111111));
        result.Add((byte)((TimeToLive >> 24) & 0b11111111));
        result.Add((byte)((TimeToLive >> 16) & 0b11111111));
        result.Add((byte)((TimeToLive >> 8) & 0b11111111));
        result.Add((byte)(TimeToLive & 0b11111111));
        var data = new List<byte>();
        foreach (var address in Addresses)
        {
            data.AddRange(address.AddressBytes);
        }
        result.Add((byte)(data.Count >> 8));
        result.Add((byte)(data.Count & 0b11111111));
        result.AddRange(data);
        return [.. result];
    }
}