using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementDB.Models
{
    public class Subject: SchoolEntity
    {
        private string _code;
        private string _title;
        
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

    }
}
