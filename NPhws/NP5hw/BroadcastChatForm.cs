using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NP5hw;
public partial class BroadcastChatForm : Form
{
    public string MyName {get; set; } = string.Empty;
    private const int Port = 1648;
    private const string BroadcastIP = "255.255.255.255";
    private readonly UdpClient client = new();
    private readonly CancellationTokenSource source = new();
    public BroadcastChatForm()
    {
        InitializeComponent();
        this.FormClosed += Form1_FormClosed;
        client.Client.Bind(new IPEndPoint(IPAddress.Any, Port));
        Listen();
    }
    private void Form1_FormClosed(object sender, FormClosedEventArgs e) => source.Cancel();
    private async void Listen()
    {
        CancellationToken token = source.Token;
        while (true)
        {
            UdpReceiveResult? result = await ReceiveAsyncNullable(token);
            if (result is null)
                break;
            listBoxMessages.Items.Add(Encoding.Unicode.GetString(result.Value.Buffer));
            listBoxMessages.SelectedIndex = listBoxMessages.Items.Count - 1;
        }
    }
    private async Task<UdpReceiveResult?> ReceiveAsyncNullable(CancellationToken token)
    {
        TaskCompletionSource<bool> source = new();
        using (token.Register(() => source.TrySetResult(true), false))
        {
            Task<UdpReceiveResult> receiving = client.ReceiveAsync();
            if (await Task.WhenAny(receiving, source.Task) != receiving)
                return null;
            else
                return receiving.Result;
        }
    }
    private void buttonSend_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(textBoxMessage.Text))
            return;
        byte[] buffer = Encoding.Unicode.GetBytes(MyName + ": " +textBoxMessage.Text);
        client.Send(buffer, buffer.Length, BroadcastIP, Port);
        textBoxMessage.Clear();
    }
}
