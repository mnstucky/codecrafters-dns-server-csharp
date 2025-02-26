using System.Text;

internal record DNSQuestion(string Name, int Type, int Class)
{
    internal static List<DNSQuestion> GetDNSQuestionsFromPacket(byte[] packet)
    {
        var result = new List<DNSQuestion>();
        int i = 12;
        while (i < packet.Length)
        {
            var nameLength = packet[i];
            var name = Encoding.ASCII.GetString(packet, i, nameLength);
            Console.WriteLine(name);
            var type = packet[i + nameLength + 1] << 8 | packet[i + nameLength + 2];
            var classEncoded = packet[i + nameLength + 3] << 8 | packet[i + nameLength + 4];
            result.Add(new DNSQuestion(name, type, classEncoded));
            i += nameLength + 6;
        }
        return result;
    }

    internal byte[] QuestionBytes => GetQuestionBytes();

    private byte[] GetQuestionBytes()
    {
        var result = new List<byte>();
        foreach (var label in Name.Split('.'))
        {
            result.Add((byte)label.Length);
            result.AddRange(Encoding.ASCII.GetBytes(label));
        }
        result.Add(0x00);
        result.Add((byte)(Type >> 8));
        result.Add((byte)(Type & 0b11111111));
        result.Add((byte)(Class >> 8));
        result.Add((byte)(Class & 0b11111111));
        return [.. result];
    }
};