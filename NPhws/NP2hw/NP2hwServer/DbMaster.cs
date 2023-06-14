using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP2hwServer
{
    internal class DbMaster
    {
        public DbMaster() { }
        public static void Menu()
        {
            Console.WriteLine("\tМастер базы данных: консольные команды\n" +
                              "\t/add [почтовый_индекс] [улица] - добавление эл-та в таблицу Posts;\n" +
                              "\t/delete [идентификатор] - удаление эл-та из таблицы Posts;\n" +
                              "\t/select [почтовый_индекс] - вывод эл-тов по почтовому индексу,\n" +
                              "\t(если значение почтового индекса равно нулю, то вывод всех эл-тов);\n" +
                              "\t/exit - выход из меню базы данных.");
            while (true)
            {
                string command = Console.ReadLine();
                if (command.Split()[0] == "/add" && IsDigits(command.Split()[1]))
                    Add(int.Parse(command.Split()[1]), command.Split()[2]);
                else if (command.Split()[0] == "/delete" && IsDigits(command.Split()[1]))
                    Delete(int.Parse(command.Split()[1]));
                else if (command.Split()[0] == "/select" && IsDigits(command.Split()[1]))
                    Console.WriteLine(Select(int.Parse(command.Split()[1])));
                else if (command == "/exit")
                    break;
                else
                    Console.WriteLine("\tОшибка: комманда была введена некорректно.");
            }
        }
        private async static void Add(int zipCode, string street)
        {
            using (PostContext db = new PostContext())
            {
                db.Posts.Add(new Post { ZipCode = zipCode, Street = street });
                db.SaveChanges();
                Console.WriteLine($"\tВ таблицу Posts был добавлен элемент: {db.Posts.Where(p => p.Street == street).ToString()}");
            }
        }
        private static void Delete(int id)
        {
            using (PostContext db = new PostContext())
            {
                db.Posts.Remove(db.Posts.Find(id));
                db.SaveChanges();
            }
        }
        public static string Select(int zipCode)
        {
            string result = string.Empty;
            using (PostContext db = new PostContext())
            {
                if(zipCode == 0)
                    foreach(Post post in db.Posts)
                        result += post.ToString() + "\n";
                else
                    foreach(Post post in db.Posts.Where(p => p.ZipCode == zipCode))
                        result += post.Street + "\n";
            }
            return result;
        }
        private static bool IsDigits(string @string)//проверка аргумента, является ли он числом
        {
            bool result = true;
            foreach(char @char in @string)
                if(!char.IsDigit(@char))
                    result = false;
            return result;
        }
    }
}
