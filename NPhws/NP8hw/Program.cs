using System.ComponentModel;
using System.Net;
FileInfo file = new FileInfo("threesome.png");
Console.WriteLine($"Speed of uploading file is {GetUploadSpeed()} B/s.");
Console.WriteLine($"Speed of downloading file is {await GetDownloadSpeed()} B/s");
double GetUploadSpeed()
{
    double speed = 0.0;
    FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://ftp.dlptest.com/" + file.Name));
    request.Method = WebRequestMethods.Ftp.UploadFile;
    request.Credentials = new NetworkCredential("dlpuser", "rNrKYTX9g7z3RgJRmxWuGHbeu");
    request.KeepAlive = false;
    request.UseBinary = true;
    request.ContentLength = file.Length;
    byte[] buffer = new byte[1024];
    int length = 0;
    FileStream filestream = file.OpenRead();
    int counter = 0;
    DateTime startTime = DateTime.Now;
    try
    {
        Stream stream = request.GetRequestStream();
        length = filestream.Read(buffer, 0, buffer.Length);
        while (length != 0)
        {
            stream.Write(buffer, 0, length);
            counter += length;
            speed = Math.Round(Convert.ToDouble(counter) / DateTime.Now.Subtract(startTime).TotalSeconds)/1000; // байт в секунду
            length = filestream.Read(buffer, 0, buffer.Length);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    return speed;
}
//Не удалось решить проблему с загрузкой на компьютер
async Task<double> GetDownloadSpeed()
{
    double speed = 0.0;
    FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://ftp.dlptest.com/" + file.Name));
    request.Method = WebRequestMethods.Ftp.DownloadFile;
    request.Credentials = new NetworkCredential("dlpuser", "rNrKYTX9g7z3RgJRmxWuGHbeu");
    request.KeepAlive = false;
    request.UseBinary = true;
    DateTime start = DateTime.Now;
    FtpWebResponse response = (FtpWebResponse) await request.GetResponseAsync();
    Stream stream = response.GetResponseStream();
    int readCount = 0;
    byte[] buffer = new byte[1024];
    while(readCount < stream.Length)
    {
         readCount += await stream.ReadAsync(buffer, 0, buffer.Length);
            speed = Math.Round(readCount / (DateTime.Now.Subtract(start)).TotalSeconds)/1000;
    }
    long filesize = stream.Length;
    
    return speed;
}

