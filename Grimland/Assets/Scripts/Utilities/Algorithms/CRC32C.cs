using System.Text;
using Force.Crc32;

public static class CRC32C
{
    public static uint CalculateHash(string text)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(text);
        return Crc32CAlgorithm.Compute(bytes);
    }
}