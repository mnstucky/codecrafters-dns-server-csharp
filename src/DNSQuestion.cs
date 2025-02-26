using System.Text;

internal record DNSQuestion(string Name, int Type, int Class)
{
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