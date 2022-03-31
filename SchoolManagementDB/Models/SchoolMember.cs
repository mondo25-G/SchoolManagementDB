using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SchoolManagementDB.Models.Enums;

namespace SchoolManagementDB.Models
{
    public abstract class SchoolMember:SchoolEntity
    {
        private string _firstName;
        private string _lastName;
        private string _schoolEmail;      
        public string SchoolEmail //This is unique for every school member i.e. gx@fac.bootcamp.org for faculty, gx@stu.bootcamp.org
        {                         //are assumed to be two different people. We would need to consider a different schema if
                                  //for example we used the social security number as unique property then we would need a SchoolMember table 
                                  //in the db(Not having such a table would mean a student could be a lecturer and vice versa in this layer) 
                                  //An extra table is overkill for such a small project, so the class is marked abstract with the convention for the
                                  //email to help Reflection for model input.
            get { return _schoolEmail; }
            set { _schoolEmail = value; }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }



    }
}
