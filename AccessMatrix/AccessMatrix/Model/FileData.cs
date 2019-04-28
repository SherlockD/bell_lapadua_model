using System;

namespace AccessMatrix.Model
{
    public class FileData
    {
        public string FileName => _fileName;
        public string Rights => _rights;
        
        private string _fileName;
        private string _rights = "-";

        public FileData(string fileName,string rights)
        {
            _fileName = fileName;
            AddRight(rights);
        }
        
        public void AddRight(string right)
        {
            if (_rights == "-") _rights = "";
            
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
                _rights = _rights.Replace(right, "");

                if (_rights == "") _rights = "-";
            }
            else
            {
                Console.WriteLine("Подобного типа прав у O/S нет");
            }
        }
    }
}