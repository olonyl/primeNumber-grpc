using Grpc.Core;
using Prime;
using System;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        const string Target = "127.0.0.1:50052";
        static async Task Main(string[] args)
        {
            Channel channel = new Channel(Target, ChannelCredentials.Insecure);

            await channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("The Client connected successfully");
                }
            });

            await CallPrimeNumberService(channel);

            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }

        private static async Task CallPrimeNumberService(Channel channel)
        {
            var client = new PrimeNumberService.PrimeNumberServiceClient(channel);

            var request = new PrimeNumberDecompositionRequest { Number = 120 };
            var response = client.Decompose(request);

            while (await response.ResponseStream.MoveNext())
            {
                Console.WriteLine(response.ResponseStream.Current.PrimeFactor);
                await Task.Delay(500);
            }
        }
    }
}
