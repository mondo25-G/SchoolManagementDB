
using System;



namespace SchoolManagementDB.Models
{
    public class Course: SchoolEntity
    {
        private int _credits;
        private int _subjectId;
        private int _taughtModuleId;
        private string _code;
        private string Code
        {
            get { return _code; }
        }
        public int Credits
        {
            get { return _credits; }
            set { _credits = value; }
        }
        public int SubjectId
        {
            get { return _subjectId; }
            set { _subjectId = value; }
        }
        public int TaughtModuleId
        {
            get { return _taughtModuleId; }
            set { _taughtModuleId = value; }
        }
        
        

        
        
        
       

    }
}
