using System.CodeDom.Compiler;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using NPexwLib;
namespace NPexw
{
    public partial class CrossesNoughts : Form
    {
        private TcpClient server;
        public Sign MySign { get; set; }
        private Data data = new Data();
        private Button[,] field = new Button[Constants.Length, Constants.Length];
        public CrossesNoughts()
        {
            InitializeComponent();
            GenerateField();
        }
        private async void CrossesNoughts_Load(object sender, EventArgs e)
        {
            server = new TcpClient();
            await server.ConnectAsync("192.168.1.2", Ports.Server);
            ListenServer();
        }
        private async void ListenServer()
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int count = await server.GetStream().ReadAsync(buffer, 0, buffer.Length);
                string json = Encoding.UTF8.GetString(buffer);
                data = JsonSerializer.Deserialize<Data>(json);
                UpdateField();
            }
        }
        private void GenerateField()//генерация поля
        {
            for (int i = 0; i < Constants.Length; i++)
            {
                for (int j = 0; j < Constants.Length; j++)
                {
                    field[i, j] = new Button();
                    field[i, j].Size = new Size(Constants.CellSide, Constants.CellSide);
                    field[i, j].Location = new Point(i * Constants.CellSide, j * Constants.CellSide);
                    field[i, j].Text = string.Empty;
                    field[i, j].Click += CellClick;
                    this.Controls.Add(field[i, j]);
                }
            }
        }
        private void UpdateField()//обновление поля
        {
            Text = data.Status;
            if (MySign == data.Sign)
            {
                for (int i = 0; i < Constants.Length; i++)
                {
                    for (int j = 0; j < Constants.Length; j++)
                    {
                        field[i, j].Enabled = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Constants.Length; i++)
                {
                    for (int j = 0; j < Constants.Length; j++)
                    {
                        field[i, j].Enabled = false;
                    }
                }
            }
            field[data.X, data.Y].Text = data.Value == 1 ? Constants.Cross : Constants.Nought;
            field[data.X, data.Y].Enabled = data.Value != 0 ? false : true;
            if (data.IsWon)
            {
                for (int i = 0; i < Constants.Length; i++)
                {
                    for (int j = 0; j < Constants.Length; j++)
                    {
                        field[i, j].Enabled = false;
                    }
                }
                MessageBox.Show(data.Status);
            }
        }
        private async void CellClick(object sender, EventArgs e)//обработчик на клик ячейки
        {
            Button lastCell = sender as Button;
            for (int i = 0; i < Constants.Length; i++)
            {
                for (int j = 0; j < Constants.Length; j++)
                {
                    if (lastCell.Location == field[i, j].Location)
                    {
                        data.X = i;
                        data.Y = j;
                    }
                }
            }
            if (MySign == Sign.Cross)
            {
                data.Value = 1;
                data.Status = "Nougths\' Turn";
            }
            else
            {
                data.Value = 2;
                data.Status = "Crosses\' Turn";
            }
            string json = JsonSerializer.Serialize(data);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            await server.GetStream().WriteAsync(buffer, 0, buffer.Length);
            UpdateField();
        }
    }
}