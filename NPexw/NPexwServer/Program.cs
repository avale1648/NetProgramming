using Microsoft.Win32.SafeHandles;
using NPexwLib;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Schema;

namespace NPexwServer
{
    public class Program
    {
        private TcpClient[] clients = new TcpClient[Constants.PlayersNumber];
        private int row = 4;
        private static int[,] values = new int[Constants.Length, Constants.Length];
        public Program()
        {
            for(int i =0; i<Constants.Length; i++)
            {
                for(int j = 0; j < Constants.Length; j++)
                {
                    values[i, j] = 0;
                }
            }
        }
        public static void Main(string[] args) => new Program().Run().Wait();
        private async Task Run()
        {
            TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Parse("192.168.1.2"), Ports.Server));
            listener.Start();
            Console.WriteLine("Server is running, waiting incoming connections");
            WaitIncomingConnectionsAsync(listener);
            Console.WriteLine("Press any button for exit");
            Console.ReadKey();
        }
        private async void WaitIncomingConnectionsAsync(TcpListener listener)
        {
            while(true)
            {
                TcpClient newClient = await listener.AcceptTcpClientAsync();
                lock (clients)
                {
                    if (clients[0] == null)
                        clients[0] = newClient;
                    else
                        clients[1] = newClient;
                }
                    ServeClient(newClient);   
            }
        }
        private async void ServeClient(TcpClient client)
        {
            while(true)
            {
                byte[] buffer = new byte[1024];
                int count = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                string json = Encoding.UTF8.GetString(buffer);
                Console.WriteLine("Data delivered:\n" + json);
                Data restoredData = JsonSerializer.Deserialize<Data>(json);
                restoredData.Sign = restoredData.Sign == Sign.Cross ? Sign.Nought : Sign.Cross;//меняем знак;
                UpdateValues(restoredData);
                restoredData.IsWon = IsWon(restoredData);
                json = JsonSerializer.Serialize(restoredData);
                buffer = Encoding.UTF8.GetBytes(json);
                List<Task> tasks = new List<Task>();
                foreach (TcpClient item in clients)
                    tasks.Add(item.GetStream().WriteAsync(buffer, 0, count));
                await Task.WhenAll(tasks);
            }
        }
        private void UpdateValues(Data? data) => values[data.X, data.Y] = data.Value;
        private bool IsXInBound(Data? data)//проверка для того, чтобы X не выходил за пределы массива:
        {
            return data.X >= row && data.X <= Constants.Length - row;
        }
        private bool IsYInBound(Data data)//проверка для того, чтобы X находился за пределы массива:
        {
            return data.X >= row && data.Y <= Constants.Length - row;
        }
        private bool ReturnWinner(Data data, string winner)
        {
            data.Status = winner;
            return true;
        }
        private bool IsWon(Data? data)
        {
            bool isFiveInRow = false;
            string winner = data.Value == 1 ? "Crosses won" : "Noughts won";

            for (int i = -row; i <= 0; i++)
            {
                isFiveInRow = false;
                for (int j = 0; j <= row; j++)
                {
                    if (IsXInBound(data) && values[data.X, data.Y] == values[data.X + i + j, data.Y])
                        isFiveInRow = true;
                }
            }
            if (isFiveInRow)
                return ReturnWinner(data, winner);
            for (int i = -row; i <= 0; i++)
            {
                for (int j = 0; j <= row; j++)
                {
                    isFiveInRow = false;
                    if (IsYInBound(data) && values[data.X, data.Y] == values[data.X, data.Y + i + j])
                        isFiveInRow = true;
                }
            }
            if (isFiveInRow)
                return ReturnWinner(data, winner);
            for (int i = -row; i <= 0; i++)
            {
                for (int j = 0; j <= row; j++)
                {
                    isFiveInRow = false;
                    if (IsXInBound(data) && IsYInBound(data) && values[data.X, data.Y] == values[data.X + i + j, data.Y + i + j])
                        isFiveInRow = true;
                }
            }
            if (isFiveInRow)
                return ReturnWinner(data, winner);
            for (int i = -row; i <= 0; i++)
            {
                for (int j = 0; j <= row; j++)
                {
                    isFiveInRow = false;
                    if (IsXInBound(data) && IsYInBound(data) && values[data.X, data.Y] == values[data.X + i + j, data.Y - i - j])
                        isFiveInRow = true;
                }
            }
            if (isFiveInRow)
                return ReturnWinner(data, winner);
            int count = 0;
            for (int i = 0; i < Constants.Length; i++)
            {
                for (int j = 0; j < Constants.Length; j++)
                {
                    if (values[i, j] != 0)
                        count = count + 1;
                }
            }
            if (count == Constants.Length * Constants.Length)
                return ReturnWinner(data, "Draft");
            return false;
        }
    }
}
