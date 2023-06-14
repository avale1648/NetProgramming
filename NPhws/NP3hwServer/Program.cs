using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NP3hwLib;

namespace NP3hwServer
{
    public class Program
    {
        private ICollection<TcpClient> clients = new List<TcpClient>();
        public static void Main(string[] args) => new Program().Run().Wait();
        private async Task Run()
        {
            TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Any, 1648));
            listener.Start();
            Console.WriteLine("Ожидание входящих соеднинений");
            await WaitIncomingConnectionsAsync(listener);
            Console.ReadKey();
        }
        private async Task WaitIncomingConnectionsAsync(TcpListener listener)
        {
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                lock (clients)
                    clients.Add(client);
                ServeClient(client);
            }
        }
        private async void ServeClient(TcpClient client)
        {
            Console.WriteLine(clients.Last().Client.RemoteEndPoint +" подключился");
            while (true)
            {
                byte[] buffer = await Library.BufferedReadBlock(client.GetStream());
                string filenameResponse = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                IEnumerable<TcpClient> copy;
                lock (clients)
                    copy = clients.ToList();
                await Task.WhenAll(copy.Select(other => other.GetStream().WriteAsync(BitConverter.GetBytes(buffer.Length), 0, buffer.Length)));//длина имени файла
                await Task.WhenAll(copy.Select(other => other.GetStream().WriteAsync(buffer, 0, buffer.Length)));//имя файла
                buffer = await Library.BufferedRead(client.GetStream(), sizeof(long));
                long filesize = BitConverter.ToInt64(buffer, 0);
                await Task.WhenAll(copy.Select(other => other.GetStream().WriteAsync(buffer, 0, buffer.Length)));//
                long sent = 0;
                while (sent < filesize)
                {
                    int block = (int)Math.Min(1024, filesize - sent);
                    await client.GetStream().ReadAsync(buffer, 0, block);//файл
                    await Task.WhenAll(copy.Select(other => other.GetStream().WriteAsync(buffer, 0, buffer.Length)));
                    sent += block;
                }
            }
        }
    }
}