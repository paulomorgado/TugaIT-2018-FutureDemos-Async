using System;
using System.Threading.Tasks;
using static System.Console;

class LongRunningDisposable : IAsyncDisposable
{
    public async Task DisposeAsync()
    {
        WriteLine();
        WriteLine($"{nameof(DisposeAsync)}: Oh dear! This might take awhile!");
        await Task.Delay(2000);
        WriteLine($"{nameof(DisposeAsync)}: Whew! Done!");
        WriteLine();
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        using await (new LongRunningDisposable())
        {
            WriteLine($"{nameof(Main)}: Using a resource with a long-running {nameof(IAsyncDisposable.DisposeAsync)} method.");
        }

        WriteLine($"{nameof(Main)}: Finished with resource.");
        WriteLine();
    }
}
