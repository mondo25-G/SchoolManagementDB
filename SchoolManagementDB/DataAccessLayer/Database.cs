using System.Data;

namespace SchoolManagementDB.DataAccessLayer
{
    /// <summary>
    /// The first part to creating a database factory design pattern is to implement a generic Database object.<br></br> 
    /// This is the object to reference when calling database routines. <br></br> 
    /// Since this object is generic, we don’t have to decide what type of physical database to actually use with it. <br></br>
    /// This gives the power to change database providers without changing any code. <br></br>
    /// </summary>
    ///  <Remarks>
    /// It’s important to note that we’re using .NET’s own abstract database factory interfaces. <br></br>
    /// The interfaces can be used in exactly the same way as the concrete database classes (SqlConnection, OdbcCommand, etc),<br></br>
    /// without forcing us to bind to one. This is the core piece to providing flexibility to the database layer.
    /// </Remarks>
    public abstract class Database
    {
        public string connectionString;

        #region Abstract Functions

        public abstract IDbConnection CreateConnection();
        public abstract IDbCommand CreateCommand();
        public abstract IDbConnection CreateOpenConnection();
        public abstract IDbCommand CreateCommand(string commandText, IDbConnection connection);
        public abstract IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection);
        public abstract IDataParameter CreateParameter(string parameterName, object parameterValue);

        #endregion
    }
}
