using System.Text;

internal record DNSQuestion(string Name, int Type, int Class)
{
    internal static List<DNSQuestion> GetDNSQuestionsFromPacket(byte[] packet)
    {
        var result = new List<DNSQuestion>();
        int i = 12;
        while (i < packet.Length)
        {
            var name = "";
            while (packet[i] != 0x00)
            {
                Console.WriteLine(Encoding.ASCII.GetString(packet, i, 1));
                if (!string.IsNullOrEmpty(name))
                {
                    name += ".";
                }
                var nameLength = packet[i];
                var nameSection = Encoding.ASCII.GetString(packet, i, nameLength + 1);
                name += nameSection;
                i += nameLength + 1;
                Console.WriteLine(name);
            }
            var type = packet[i + 1] << 8 | packet[i + 2];
            Console.WriteLine(type);
            var classEncoded = packet[i + 3] << 8 | packet[i + 4];
            Console.WriteLine(classEncoded);
            result.Add(new DNSQuestion(name, type, classEncoded));
            i += 5;
        }
        while (packet[i] != 0x00)
        {
            var nameLength = packet[i];
            var name = Encoding.ASCII.GetString(packet, i, nameLength);
            Console.WriteLine(name);
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