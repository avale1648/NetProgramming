using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using NP3hwLib;
//1. Файлообменник.Напишите клиент-серверную программу, позволяющую клиенту разослать файл всем остальным подключенным клиентам.
//Клиент должен быть оконным приложением, позволяющим выбрать файл.Затем пользователь нажимает кнопку «Отправить».
//Клиент пересылает серверу имя файла, размер файла, а затем его содержимое.
//Сервер ретранслирует эту информацию всем подключенным клиентам(кроме самого отправителя).
//На стороне каждого принимающего клиента формируется новый файл с указанным именем, куда записывается его содержимое.
//Для тестирования разные клиенты могут класть его в разные папки.
//Поскольку доставка файла может занимать длительно время, следует добавить полоску прогресса при отправке и получении файла.
namespace NP3hwClient
{
    public partial class Form1 : Form
    {
        private TcpClient server;
        private string filename = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            server = new TcpClient();
            await server.ConnectAsync(Library.GetLocalIPAdress(), 1648);
            ListenToServer();
        }
        private async void ListenToServer()
        {
        //    while (true)
        //    {
        //        byte[] buffer = new byte[1024];
        //        int count = await server.GetStream().ReadAsync(buffer, 0, buffer.Length);//длина имени файла
        //        string filenameResponse = Encoding.UTF8.GetString(buffer, 0, count);//имя файла
        //        buffer = await Library.BufferedRead(server.GetStream(), sizeof(long));
        //        long filesize = BitConverter.ToInt64(buffer, 0);
        //        using (Stream s = File.Create(Path.Combine(KnownFolders.GetPath(KnownFolder.Downloads), filenameResponse)))
        //        {
        //            long read = 0;
        //            while (read < filesize)
        //            {
        //                int block = (int)Math.Min(1024, buffer.Length - read);
        //                await server.GetStream().ReadAsync(buffer, 0, block);
        //                await s.WriteAsync(buffer, 0, block);
        //                read += block;
        //            }
        //        }
        //    }
        }
        private void textBoxSource_DoubleClick(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = /*Path.GetFileName(openFileDialog1.FileName);*/openFileDialog1.FileName;
                textBoxSource.Text = openFileDialog1.FileName;
            }
        }
        private async void buttonSend_Click(object sender, EventArgs e)
        {
            if (filename != string.Empty)
            {
                Stream stream = server.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes(Path.GetFileName(filename));//имя файла
                await stream.WriteAsync(BitConverter.GetBytes(buffer.Length));//длина имени файла;
                await stream.WriteAsync(buffer);//имя файла;
                //await stream.WriteAsync(BitConverter.GetBytes(new FileInfo(filename).Length));//длина файла
                buffer = File.ReadAllBytesAsync(filename).Result;
                await stream.WriteAsync(BitConverter.GetBytes(buffer.Length));//длина файла
                long sent = 0;
                while( sent < buffer.Length)
                {
                    int block = (int)Math.Min(1024, buffer.Length - sent);
                    await stream.WriteAsync(buffer, 0, block);//файл
                    sent += block;
                }
               
                filename = string.Empty;
                textBoxSource.Text = filename;
            }
        }
    }
}