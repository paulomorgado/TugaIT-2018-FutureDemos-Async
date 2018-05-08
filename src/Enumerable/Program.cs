using System.IO;
using System.Threading.Tasks;
using static System.Console;

class Program
{
    static (Stream stream, long checksum) CreateStream()
    {
        var checksum = 0L;

        var bytes = new byte[20000];
        for (int i = 0; i < bytes.Length; i++)
        {
            var value = (byte)(i % byte.MaxValue);
            bytes[i] = value;

            unchecked { checksum += value; }
        }

        var stream = new MemoryStream(bytes);

        return (stream, checksum);
    }

    static async Task Main(string[] args)
    {
        var (stream, checksum) = CreateStream();

        var c = 0L;

        foreach await (var b in stream.AsEnumerable())
        {
            unchecked { c += b; }
        }

        if (c == checksum)
        {
            WriteLine("Checksums match!");
        }
    }
}
