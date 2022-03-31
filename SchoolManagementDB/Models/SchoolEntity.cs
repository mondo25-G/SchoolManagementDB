using SchoolManagementDB.BusinessLogicLayer;

namespace SchoolManagementDB.Models
{
    
    public class SchoolEntity
    {
        //Property Id will always be retrieved from database and never be inserted or manipulated in this tier.
        //Id only "lives" in DAL and is always retrieved never set. 
        public int Id 
        {
            get;
        }

        public SchoolEntity()
        {
            ModelInputAndValidation.InsertDataToModel(this);
        }
        
    }
}
