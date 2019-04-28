using System;
using System.Net.Mime;
using System.Runtime.InteropServices;
using AccessMatrix.Model;

namespace AccessMatrix
{
    internal class Program
    {
        private static void PrintMenu(bool isLogined)
        {
            Console.WriteLine("Меню:\n 0)Войти/Перезайти в систему\n 1)Напечатать матрицу \n9)Выход");
            if (isLogined)
            {
                Console.WriteLine("\n2)Ввести команду");
                Console.WriteLine("3)Проверить право");
                Console.WriteLine("8)Разлогиниться");
            }
        }
        
        public static void Main()
        {
            AccessModel accessMatrix = new AccessModel();

            int menu = -2;
            while (menu != -1)
            {
                Console.Clear();
                PrintMenu(accessMatrix.IsLogined());

                if (!Int32.TryParse(Console.ReadLine(), out menu))
                {
                    Console.WriteLine("Некорректный ввод");
                    Console.ReadKey();
                    continue;
                }
                switch (menu)
                {
                    case 0:
                        Console.WriteLine("Введите логин");
                        string login = Console.ReadLine();
                        Console.WriteLine("Введите пароль");
                        string password = Console.ReadLine();
                        accessMatrix.Login(login, password);
                        break;
                    case 1:
                        accessMatrix.PrintMatrix();
                        break;
                    case 2:
                        if (!accessMatrix.IsLogined())
                        {
                            Console.WriteLine("Сначала нужно войти в систему");
                            Console.ReadKey();
                            continue;
                        }
                        string command = Console.ReadLine();
                        accessMatrix.EnterCommand(command);
                        break;
                    case 3:
                        Console.WriteLine("Введите имя объекта");
                        string objectName = Console.ReadLine();
                        Console.WriteLine("Введите право");
                        string right = Console.ReadLine();
                        if (accessMatrix.HasRights(objectName, right))
                        {
                            Console.WriteLine("Пользователь имеет данное право");
                        }
                        else
                        {
                            Console.WriteLine("Пользователь не имеет данное право");
                        }

                        Console.ReadKey();
                        break;
                    case 8:
                        accessMatrix.UnLogin();
                        break;
                    case 9:
                        return;
                    default:
                        Console.WriteLine("Данного пункта меню не существует введите другой");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}