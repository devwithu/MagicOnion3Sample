using Grpc.Core;
using MagicOnion.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Sample.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var ports = new ServerPort("0.0.0.0", 12345, ServerCredentials.Insecure);

            await MagicOnionHost.CreateDefaultBuilder()
                .UseMagicOnion(ports)
                .RunConsoleAsync();
        }
    }
}
