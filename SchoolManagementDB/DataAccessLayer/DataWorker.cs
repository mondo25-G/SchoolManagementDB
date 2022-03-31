using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SchoolManagementDB.DataAccessLayer
{
    /// <summary>
    /// To actually use the database factory, we’ll create a data worker class to take care of handling how the database factory and generic database are used.<br></br>
    /// The data worker class holds a static instance of the abstract Database class. <br></br>
    /// In its constructor, we instantiate the concrete database (defined in App.config file) by using our database factory design pattern.<br></br>
    /// </summary>
    /// <remarks>
    /// Notice how, in the below code, there are no references to concrete database providers.<br></br>
    /// Everything is kept generic by using the abstract Database class and leaving the details of the concrete type<br></br>
    /// inside the App.config file for the factory to access.
    /// </remarks>
    public class DataWorker
    {
        private static Database _database = null;

        static DataWorker()
        {
            try
            {
                _database = DatabaseFactory.CreateDatabase();
            }
            catch (Exception excep)
            {
                throw excep;
            }
        }

        public static Database database
        {
            get { return _database; }
        }
    }
}
