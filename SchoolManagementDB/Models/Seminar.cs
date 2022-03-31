using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SchoolManagementDB.Models.Enums;

namespace SchoolManagementDB.Models
{
    public class Seminar:SchoolEntity
    {
        private int _courseId;
        private int _lecturerId;
        private string _title;
        private Term _term;       
        public int LecturerId
        {
            get { return _lecturerId; }
            set { _lecturerId = value; }
        }
        public int CourseId
        {
            get { return _courseId; }
            set { _courseId = value; }
        }
        public Term Term
        {
            get { return _term; }
            set { _term = value; }
        }
        private string Title
        {
            get { return _title; }
        }


    }
}
