using Grpc.Core;
using Prime;
using System;
using System.IO;

namespace server
{
    class Program
    {
        const int Port = 50052;
        static void Main(string[] args)
        {
            Server server = null;
            try
            {
                server = new Server
                {
                    Services =
                    {
                        PrimeNumberService.BindService(new PrimeNumberServiceImpl())
                    },
                    Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
                };
                server.Start();
                Console.WriteLine($"The server is listening on the port: {Port}");
                Console.ReadLine();
            }
            catch (IOException ex)
            {
                Console.WriteLine("The server failed to start: " + ex.Message);
                throw;
            }
            finally
            {
                if (server != null)
                {
                    server.ShutdownAsync().Wait();
                }
            }
        }
    }
}
