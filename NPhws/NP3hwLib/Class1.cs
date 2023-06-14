using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;

namespace NP3hwLib
{
    public static class Library
    {
        //длина след. блока заранее неизвестна -> читаем первые 4 Бт из потока - длина, затем блок полученной длины;
        public async static Task<byte[]> BufferedReadBlock(Stream stream)
        {
            byte[] prefixedBuffer = await BufferedRead(stream, sizeof(int));//длина
            int length = BitConverter.ToInt32(prefixedBuffer);
            byte[] response = await BufferedRead(stream, length);
            return response;
        }
        //если известна длина(кол-во байтов) зачитывается столько байтов с потока
        public async static Task<byte[]> BufferedRead(Stream stream, int length)
        {
            byte[] buffer = new byte[length];
            for (int progress = 0; progress < length;)
            {
                int chunk = await stream.ReadAsync(buffer, progress, length - progress);
                progress += chunk;
            }
            return buffer;
        }
        public static IPAddress GetLocalIPAdress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            throw new Exception();
        }
    }
    public enum KnownFolder
    {
        Contacts,
        Downloads,
        Favorites,
        Links,
        SavedGames,
        SavedSearches
    }
    public static class KnownFolders
    {
        private static readonly Dictionary<KnownFolder, Guid> _guids = new()
        {
            [KnownFolder.Contacts] = new("56784854-C6CB-462B-8169-88E350ACB882"),
            [KnownFolder.Downloads] = new("374DE290-123F-4565-9164-39C4925E467B"),
            [KnownFolder.Favorites] = new("1777F761-68AD-4D8A-87BD-30B759FA33DD"),
            [KnownFolder.Links] = new("BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968"),
            [KnownFolder.SavedGames] = new("4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4"),
            [KnownFolder.SavedSearches] = new("7D1D3A04-DEBB-4115-95CF-2F29DA2920DA")
        };

        public static string GetPath(KnownFolder knownFolder)
        {
            return SHGetKnownFolderPath(_guids[knownFolder], 0);
        }

        [DllImport("shell32",
            CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        private static extern string SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags,
            nint hToken = 0);
    }
}