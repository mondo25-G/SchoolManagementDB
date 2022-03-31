using System;
using System.Configuration;

namespace SchoolManagementDB.DataAccessLayer
{
    /// <summary>
    /// This section handler derives from the ConfigurationSection base class. <br></br>
    /// When this class is instantiated, it will automatically read the line from the app.config file <br></br>
    /// and populate with the values. Since we include a ConnectionString property, <br></br>
    /// it will also fetch the correct connection string, so that we never have to access <br></br>
    /// the ConfigurationManager.ConnectionStrings property ourselves
    /// </summary>

    public sealed class DatabaseFactorySectionHandler : ConfigurationSection
    {
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string)base["Name"]; }
        }

        [ConfigurationProperty("ConnectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)base["ConnectionStringName"]; }
        }

        public string ConnectionString
        {
            get
            {
                try
                {
                    return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
                }
                catch (Exception excep)
                {
                    throw new Exception("Connection string " + ConnectionStringName + " was not found in App.config. " + excep.Message);
                }
            }
        }
    }
}
