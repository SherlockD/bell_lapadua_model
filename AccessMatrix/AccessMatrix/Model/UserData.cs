using System.Collections.Generic;

namespace AccessMatrix.Model
{
    public class UserData
    {
        public string UserName => _userName;
        public int SubjectType => _subjectType;
        public string Password => _password;
        public List<FileData> FileDatas => _fileDatas;
        
        private readonly string _userName;
        private readonly int _subjectType;
        private readonly string _password;
        
        private List<FileData> _fileDatas;

        public UserData(string userName, int subjectType, string password)
        {
            _userName = userName;
            _subjectType = subjectType;
            _password = password;
            _fileDatas = new List<FileData>();
        }

        public void AddFileData(FileData data)
        {
            _fileDatas.Add(data);
        }
    }
}