using System.Reflection;
using System;
using System.Configuration;

namespace SchoolManagementDB.DataAccessLayer
{

    /// <summary>
    /// The database factory is the class we will use to instantiate the concrete provider for our generic Database class.
    ///  Remember, we defined an abstract Database class which we can use without deciding on a concrete database provider.
    /// Instead, we specify the concrete provider in the app.config and let the database factory design pattern instantiate it.
    /// Note, the database factory is able to instantiate any type of concrete database by using C# .NET reflection.
    /// </summary>
    /// <remarks>
    /// It’s important to note the use of .NET Reflection in the database factory. <br></br>
    /// While this adds the extensibility and power for our database layer, it also adds a bit of overhead to processing.<br></br> 
    /// This overhead can be minimized, in the DataWorker class, <br></br>
    /// by utilizing a static variable so that the number of times objects are instantiated by the factory is minimized.
    /// </remarks>
    public sealed class DatabaseFactory
    {
        public static DatabaseFactorySectionHandler sectionHandler = (DatabaseFactorySectionHandler)ConfigurationManager.GetSection("DatabaseFactoryConfiguration");

        private DatabaseFactory() { }

        public static Database CreateDatabase()
        {
            
            // Verify a DatabaseFactoryConfiguration line exists in the app.config.
            if (sectionHandler.Name.Length == 0)
            {
                throw new Exception("Database name not defined in DatabaseFactoryConfiguration section of app.config.");
            }

            try
            {
                
                // Find the class
                Type database = Type.GetType(sectionHandler.Name);

                // Get it's constructor
                ConstructorInfo constructor = database.GetConstructor(new Type[] { });

                // Invoke it's constructor, which returns an instance.
                Database createdObject = (Database)constructor.Invoke(null);

                // Initialize the connection string property for the database.
                createdObject.connectionString = sectionHandler.ConnectionString;

                // Pass back the instance as a Database
                return createdObject;
            }
            catch (Exception excep)
            {
                throw new Exception("Error instantiating database " + sectionHandler.Name + ". " + excep.Message);
            }
        }
    }
}
