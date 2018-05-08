using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Console;

namespace System.IO
{
    class StreamEnumerator : IAsyncEnumerator<byte>, IAsyncDisposable
    {
        private const int BufferSize = 8192;
        private readonly Stream _stream;
        private readonly byte[] _buffer;
        private int _bytesRead;
        private int _bufferIndex;

        public StreamEnumerator(Stream stream)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            _buffer = new byte[BufferSize];
            _bytesRead = -1;
        }

        public async Task<bool> WaitForNextAsync()
        {
            await Task.Delay(1000);

            _bytesRead = await _stream.ReadAsync(_buffer, 0, BufferSize);
            _bufferIndex = 0;

            if (_bytesRead == 0)
            {
                WriteLine();
                WriteLine("Enumeration complete!");
                return false;
            }

            WriteLine($"Read {_bytesRead:0,0} bytes");

            return true;
        }

        public byte TryGetNext(out bool success)
        {
            if (_bufferIndex == _bytesRead)
            {
                success = false;
                return 0;
            }

            var result = _buffer[_bufferIndex];
            _bufferIndex++;

            success = true;
            return result;
        }

        public Task DisposeAsync()
        {
            _stream.Dispose();
            WriteLine("Stream disposed!");
            return Task.CompletedTask;
        }
    }

    class EnumerableStream : IAsyncEnumerable<byte>
    {
        private readonly Stream _stream;

        public EnumerableStream(Stream stream)
            => _stream = stream ?? throw new ArgumentNullException(nameof(stream));

        public IAsyncEnumerator<byte> GetAsyncEnumerator()
            => new StreamEnumerator(_stream);
    }

    static class StreamExtensions
    {
        public static IAsyncEnumerable<byte> AsEnumerable(this Stream stream)
            => new EnumerableStream(stream);
    }
}
