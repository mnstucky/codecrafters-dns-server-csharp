using System.Collections;

internal class DNSMessage
{
    internal byte[] MessageBytes => [.. GetHeaderBytes().Concat(GetQuestionBytes())];

    private byte[] GetQuestionBytes()
    {
        var result = new List<byte>();
        foreach (var question in Questions)
        {
            result.AddRange(question.QuestionBytes);
        }
        return [.. result];
    }

    private byte[] GetHeaderBytes()
    {
        var result = new byte[12];
        // Set half of ID
        result[0] = (byte)(PacketIdentifier >> 8);
        // Set other half; mask to keep only 8 bits
        result[1] = (byte)(PacketIdentifier & 0b11111111);
        result[2] = (byte)(
            // First bit
            (QueryResponseIndicator ? 1 : 0) << 7 |
            // Next four bits, masked in case OperationCode is too long
            (OperationCode & 0b1111) << 3 |
            // Sixth bit
            (AuthoritativeAnswer ? 1 : 0) << 2 |
            // Seventh bit
            (Truncation ? 1 : 0) >> 1 |
            // Eighth bit
            (RecursionDesired ? 1 : 0)
            );
        result[3] = (byte)(
            // First bit
            (RecursionAvailable ? 1 : 0) << 7 |
            // Three bits reserved
            // Last four bits, masked in case ResponseCode is too long
            (ResponseCode & 0b1111)
        );
        result[4] = (byte)(QuestionCount >> 8);
        result[5] = (byte)(QuestionCount & 0b11111111);
        result[6] = (byte)(AnswerRecordCount >> 8);
        result[7] = (byte)(AnswerRecordCount & 0b11111111);
        result[8] = (byte)(AuthorityRecordCount >> 8);
        result[9] = (byte)(AuthorityRecordCount & 0b11111111);
        result[10] = (byte)(AdditionalRecordCount >> 8);
        result[11] = (byte)(AdditionalRecordCount & 0b11111111);
        return result;
    }

    internal ushort PacketIdentifier { get; set; } = 1234;

    internal bool QueryResponseIndicator { get; set; } = true;

    internal int OperationCode { get; set; }

    internal bool AuthoritativeAnswer { get; set; }

    internal bool Truncation { get; set; }

    internal bool RecursionDesired { get; set; }

    internal bool RecursionAvailable { get; set; }

    internal int ResponseCode { get; set; }

    internal int QuestionCount => Questions.Count;

    internal int AnswerRecordCount { get; set; }

    internal int AuthorityRecordCount { get; set; }

    internal int AdditionalRecordCount { get; set; }

    internal List<DNSQuestion> Questions { get; set; } = [];
}