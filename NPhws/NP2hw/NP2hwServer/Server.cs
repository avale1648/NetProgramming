using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
namespace NP2hwServer
{
    internal class Server
    {
        public static async void Start()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            listener.Bind(new IPEndPoint(IPAddress.Any, 1648));
            listener.Listen(100);
            Console.WriteLine("Сервер запущен, Ожидание подключений");
            while (true)
            {
                using (Socket client = await listener.AcceptAsync())
                {

                    Console.WriteLine($"Соединение с {client.RemoteEndPoint} установлено");
                    byte[] buffer = new byte[1024];
                    //исключение на 28
                    var received = await client.ReceiveAsync(buffer, SocketFlags.None);
                    string response = Encoding.UTF8.GetString(buffer, 0, received);
                    Console.WriteLine($"Запрос от {client.RemoteEndPoint}: {response}");
                    string message = string.Empty;
                    try
                    {
                        message = DbMaster.Select(int.Parse(response));
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                    }
                    await client.SendAsync(Encoding.UTF8.GetBytes(message), 0);
                    Console.WriteLine($"Обратное сообщение для {client.RemoteEndPoint}:\n{message}");
                    client.Shutdown(SocketShutdown.Both);
                }
            }
            listener.Shutdown(SocketShutdown.Both);
            listener.Close();
        }
    }
}
