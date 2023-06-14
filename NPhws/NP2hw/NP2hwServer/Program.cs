using NP2hwServer;
Menu();
void Menu()
{
    Console.WriteLine("Сервер: консольные команды\n" +
                      "/dbSettings - перейти к настройке базы данных\n" +
                      "/start - запустить сервер\n" +
                      "/exit - выход");
    while (true)
    {
        string command = Console.ReadLine();
        if (command == "/dbSettings")
            DbMaster.Menu();
        else if (command == "/start")
            Server.Start();
        else if (command == "/exit")
            break;
        else
            Console.WriteLine("Ошибка: комманда была введена некорректно");
    }
}
