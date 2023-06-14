using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NP2hwClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private async void toolStripButtonFind_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            using(Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
            {
                await server.ConnectAsync(new IPEndPoint(GetLocalIPAdress(), 1648));
                await server.SendAsync(Encoding.UTF8.GetBytes(toolStripTextBox1.Text), SocketFlags.None);
                byte[] buffer = new byte[1024];
                var received = await server.ReceiveAsync(buffer, SocketFlags.None);
                string response = Encoding.UTF8.GetString(buffer);
                foreach(string substring in response.Split('\n'))
                    listBox1.Items.Add(substring);
                server.Shutdown(SocketShutdown.Both);
            }
        }
        private IPAddress GetLocalIPAdress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            throw new Exception();
        }
    }
}