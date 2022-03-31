using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SchoolManagementDB.Models.Enums;

namespace SchoolManagementDB.Models
{
    public class Student: SchoolMember
    {
        
        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }

    }
}
