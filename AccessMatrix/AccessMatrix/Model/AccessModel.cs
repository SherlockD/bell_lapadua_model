using System;
using System.Security.Cryptography;
using MatrixWork.AccessMatrix;

namespace AccessMatrix.Model
{
    public class AccessModel
    {
        private Matrix _matrix;
        private UserData _loginedUser;
        
        public AccessModel()
        {
            _matrix = new Matrix();
        }

        public void Login(string login, string password)
        {
            var currentUser = _matrix.GetUser(login);

            if (currentUser == null)
            {
                Console.WriteLine("Пользователя с таким именем не существует");
                Console.ReadKey();
                return;
            }

            if (Matrix.GetMd5Hash(MD5.Create(), password) == currentUser.Password)
            {
                _loginedUser = currentUser;
                Console.WriteLine($"Добро пожаловать {login}");
            }
            else
            {
                Console.WriteLine("Неверный пароль");
            }
            Console.ReadKey();
        }

        public void UnLogin() => _loginedUser = null;

        private UserData GetUserByLoginPassword(string login, string hashPassword)
        {
            var user = _matrix.GetUser(login);

            if (user !=null && user.Password == hashPassword)
            {
                return user;
            }

            return null;
        }
        
        public void EnterCommand(string command)
        {
            try
            {
                var split = command.Split(' ');
                switch (split[0])
                {
                    case "enter":
                        if (!_matrix.HasRight("o", _loginedUser.UserName, split[4]) && _loginedUser.SubjectType != 3 && _loginedUser.SubjectType != 4)
                        {
                            Console.WriteLine("Вы не обладаете достаточным правом доступа к объекту");
                            Console.ReadKey();
                            return;
                        }
                        
                        _matrix.AddRight(split[1], split[3], split[4]);
                        break;
                    case "delete":
                        if (!_matrix.HasRight("o", _loginedUser.UserName, split[4]) && _loginedUser.SubjectType != 3 && _loginedUser.SubjectType != 4)
                        {
                            Console.WriteLine("Вы не обладаете достаточным правом доступа к объекту");
                            Console.ReadKey();
                            return;
                        }
                        
                        _matrix.DeleteRight(split[1], split[3], split[4]);
                        break;
                    case "create":
                        switch (split[1])
                        {
                            case "object":
                                _matrix.AddNewObject(_loginedUser.UserName, split[2], split[5]);
                                break;
                            case "subject":
                                if (!_matrix.HasRight("c", _loginedUser.UserName, "rights") || (!_matrix.HasRight("c", _loginedUser.UserName, "rights") && _loginedUser.SubjectType < _matrix.ConvertType(split[4])))
                                {
                                    Console.WriteLine("Вы не имеете прав на создание субъекта данного типа\n Желаете ввести данные пользователя уровнем выше для создания?(y/n)");

                                    string menu = "";
                                    menu = Console.ReadLine();

                                    switch (menu)
                                    {
                                        case "y":
                                            Console.WriteLine("Введите логин пользователя ");
                                            string login = Console.ReadLine();
                                            Console.WriteLine("Введите пароль пользователя");
                                            string password = Console.ReadLine();
                                            string hashPassword = Matrix.GetMd5Hash(MD5.Create(), password);

                                            var user = GetUserByLoginPassword(login, hashPassword);

                                            if (user == null)
                                            {
                                                Console.WriteLine("Пользователя с такими параметрами не существует");
                                                Console.ReadKey();
                                                return;
                                            }
                                            
                                            if (user.SubjectType > _matrix.ConvertType(split[4]) &&
                                                _matrix.HasRight("c", user.UserName, "Rights"))
                                            {
                                                _matrix.AddNewSubject(split[2],_matrix.ConvertType(split[4]));
                                                return;
                                            }

                                            Console.WriteLine("Пользователь не иммеет достаточных прав");
                                            break;
                                        case "n":
                                            Console.WriteLine("Возвращение в главное меню");
                                            Console.ReadKey();
                                            break;
                                        default:
                                            Console.WriteLine("Некорректный ввод");
                                            Console.ReadKey();
                                            break;
                                    }
                                    Console.ReadKey();
                                    return;
                                }
                                
                                _matrix.AddNewSubject(split[2],_matrix.ConvertType(split[4]));
                                break;
                        }
                        break;
                    case "destroy":
                        if (_matrix.HasRight("d", _loginedUser.UserName, "rights"))
                        {
                            Console.WriteLine("Вы не имеете прав на удаление чего-либо");
                            Console.ReadKey();
                            return;
                        }

                        switch (split[1])
                        {
                            case "object":
                                _matrix.DestroyObject(split[2]);
                                break;
                            case "subject":
                                _matrix.DestroySubject(split[2]);
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Некорректный ввод команды");
                Console.ReadKey();
                return;
            }
        }
        
        public void PrintMatrix()
        {
            _matrix.PrintMatrix();
            Console.ReadKey();
        }

        public bool HasRights(string objectName, string right) =>
            _matrix.HasRight(right, _loginedUser.UserName, objectName);
        
        public bool IsLogined() => _loginedUser != null;
    }
}