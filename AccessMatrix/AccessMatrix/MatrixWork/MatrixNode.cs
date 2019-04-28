using System;
using AccessMatrix.Model;

namespace MatrixWork.AccessMatrix
{
    public class MatrixNode
    {
        public UserData UserData => _userData;
        public string FileName => _fileName;
        public string Rights => _rights;


        private UserData _userData;
        private string _fileName;
        private string _rights;

        public MatrixNode(UserData userData, string fileName, string rights)
        {
            _userData = userData;
            _fileName = fileName;
            _rights = rights;
        }
        
        public void AddRight(string right)
        {
            if (!_rights.Contains(right))
            {
                _rights += right;
            }
            else
            {
                Console.WriteLine("Данный тип прав уже имеется");
            }
        }

        public void DeleteRight(string right)
        {
            if (_rights.Contains(right))
            {
                _rights.Replace(right, "");
            }
            else
            {
                Console.WriteLine("Подобного типа прав у O/S нет");
            }
        }
    }
}