using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementDB.Models
{
    public class Enrollment:SchoolEntity
    {
        
        private int _seminarId;
        private int _studentId;
        public int StudentId
        {
            get { return _studentId; }
            set { _studentId = value; }
        }
        public int SeminarId
        {
            get { return _seminarId; }
            set { _seminarId = value; }
        }
    }
}
