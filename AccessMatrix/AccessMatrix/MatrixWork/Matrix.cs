using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using AccessMatrix.Model;

namespace MatrixWork.AccessMatrix
{
    public class Matrix
    {
        public List<UserData> MatrixObject => _matrixObject;

        private List<UserData> _matrixObject = new List<UserData>();

        private List<string> _objects;

        public Matrix()
        {
            TakeUsers("Users.txt");
            ReadMatrix("StartMatrix.txt");
        }

        public void AddRight(string right ,string subject, string obj)
        {   
            foreach (var file in GetUser(subject).FileDatas)
            {
                if(file.FileName!= obj) continue;
                file.AddRight(right);
                return;
            }
            Console.WriteLine("Данного файла или пользователя не существует");
        }
        
        public void DeleteRight(string right ,string subject, string obj)
        {
            foreach (var file in GetUser(subject).FileDatas)
            {
                if(file.FileName!= obj) continue;
                file.DeleteRight(right);
                return;
            }
            Console.WriteLine("Данного файла или пользователя не существует");
        }
        
        public void AddNewSubject(string userName, int type)
        {
            if (GetUser(userName) != null)
            {
                Console.WriteLine("Пользователь с таким именем уже существует");
                return;
            }
            Console.WriteLine("Введите пароль");
            var password = Console.ReadLine();
            var newUser = new UserData(userName,type, GetMd5Hash(MD5.Create(), password));
            foreach (var obj in _objects)
            {
               newUser.AddFileData(new FileData(obj,"-"));
            }
            _matrixObject.Add(newUser);
        }
        
        public void AddNewObject(string subjectName ,string objectName, string type)
        {
            if (String.IsNullOrEmpty(objectName))
            {
                Console.WriteLine("Введите не пустое имя");
                return;
            }

            _objects.Add(objectName);
            
            foreach (var obj in _matrixObject)
            {
                obj.AddFileData(new FileData(objectName,"-"));
                Console.WriteLine($"Создан объект {objectName} c типом {type}");
            }
            
            AddRight("o",subjectName,objectName);
        }

        public void DestroySubject(string subjectName)
        {
            if (GetUser(subjectName) == null)
            {
                Console.WriteLine("Такого пользователя не существет");
            }

            _matrixObject.Remove(GetUser(subjectName));
        }

        public void DestroyObject(string obj)
        {
            if (String.IsNullOrEmpty(obj))
            {
                Console.WriteLine("Имя не должно быть пустым");
            }

            foreach (var node in _matrixObject)
            {
                foreach (var file in node.FileDatas)
                {
                    if (file.FileName == obj)
                    {
                        node.FileDatas.Remove(file);
                        break;
                    }
                }
            }
        }
        
        public void PrintMatrix()
        {
            foreach (var matrixNode in _matrixObject)
            {
                Console.Write(matrixNode.UserName);
                foreach (var data in matrixNode.FileDatas)
                {
                    Console.Write($" {data.FileName} {data.Rights}");
                }
                Console.Write("\n");
            }
        }

        public UserData GetUser(string name)
        {
            foreach (var node in _matrixObject)
            {
                if (node.UserName == name)
                {
                    return node;
                }
            }

            return null;
        }
        private void ReadMatrix(string fileName)
        {   
            using (StreamReader reader = new StreamReader(fileName))
            {
                var line = reader.ReadLine();
                
                _objects = new List<string>(ReadObjects(line));
                
                while ((line = reader.ReadLine())!=null)
                {                       
                    AddRights(line);
                }
            }
        }

        private string[] ReadObjects(string str)
        {
            var split = str.Split(' ');

            return split;
        }

        private void TakeUsers(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(' ');
                    _matrixObject.Add(new UserData(split[0],ConvertType(split[1]), split[2]));                    
                }
            }
        } 
        
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public int ConvertType(string userType)
        {
            switch (userType)
            {
                case "super_admin":
                    return 4;
                case "admin":
                    return 3;
                case "user":
                    return 2;
            }

            return 1;
        }
        
        private void AddRights(string str)
        {
            var split = str.Split(' ');

            var rulesList = new List<string>();
            
            var userName = split[0];

            var user = GetUser(userName);

            if (user == null)
            {
                Console.WriteLine("Пользователя с таким именем не существует");
                return;
            }
            
            for (int i = 0; i < split.Length-1; i++)
            {
                user.FileDatas.Add(new FileData(_objects[i],split[i+1]));
            }
        }

        public bool HasRight(string right, string subjectName, string objectName)
        {
            foreach (var obj in MatrixObject)
            {
                if (obj.UserName == subjectName)
                {
                    foreach (var file in obj.FileDatas)
                    {
                        if (file.FileName == objectName && file.Rights.Contains(right))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}