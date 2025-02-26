internal record DNSIPAddress(string Address)
{
    internal byte[] AddressBytes => GetAddressBytes();

    private byte[] GetAddressBytes()
    {
        var result = new byte[4];
        var addressSplit = Address.Split('.');
        result[0] = (byte)int.Parse(addressSplit[0]);
        result[1] = (byte)int.Parse(addressSplit[1]);
        result[2] = (byte)int.Parse(addressSplit[2]);
        result[3] = (byte)int.Parse(addressSplit[3]);
        return result;
    }
}