using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

public static class SavingUtils
{
    public static string Deflate(string[] inflatedData)
    {
        byte[] baseTilemapByteArray = StringArrayToByteArray(inflatedData);
        byte[] compressedByteArray = Compress(baseTilemapByteArray);
        return Convert.ToBase64String(compressedByteArray);
    }

    public static string[] Inflate(string deflatedData)
    {
        byte[] compressedData = Convert.FromBase64String(deflatedData);
        byte[] decompressedData = Decompress(compressedData);
        string dataString = Encoding.Default.GetString(decompressedData);
        return dataString.Split('/');
    }

    private static byte[] StringArrayToByteArray(string[] stringArray)
    {
        return string.Join("", stringArray).Select(x => (byte)x).ToArray();
    }

    private static byte[] Compress(byte[] data)
    {
        MemoryStream output = new MemoryStream();
        using (DeflateStream deflateStream = new DeflateStream(output, CompressionLevel.Optimal))
        {
            deflateStream.Write(data, 0, data.Length);
        }

        return output.ToArray();
    }

    private static byte[] Decompress(byte[] data)
    {
        MemoryStream input = new MemoryStream(data);
        MemoryStream output = new MemoryStream();
        using (DeflateStream deflateStream = new DeflateStream(input, CompressionMode.Decompress))
        {
            deflateStream.CopyTo(output);
        }

        return output.ToArray();
    }
}