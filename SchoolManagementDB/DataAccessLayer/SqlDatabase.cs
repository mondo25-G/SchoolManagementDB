
using System.Data;
using System.Data.SqlClient;

namespace SchoolManagementDB.DataAccessLayer
{
    /// <summary>
    /// Here we create a concrete implementation of the abstract Database object. <br></br>
    /// We will create a concrete Database class for the type of database you plan on using (or may use in the future). <br></br>
    /// If we end up needing to change the database provider, we can change the web.config <br></br>
    /// to use a different concrete implementation.
    /// </summary>
    public class SqlDatabase : Database
    {
        public override IDbConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
        public override IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }
        public override IDbConnection CreateOpenConnection()
        {
  
            SqlConnection connection = (SqlConnection)CreateConnection();
            connection.Open();         
            return connection;
        }
        public override IDbCommand CreateCommand(string commandText, IDbConnection connection)
        {
            SqlCommand command = (SqlCommand)CreateCommand();
            command.CommandText = commandText;
            command.Connection = (SqlConnection)connection;
            command.CommandType = CommandType.Text;
            return command;
        }
        public override IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection)
        {
            SqlCommand command = (SqlCommand)CreateCommand();
            command.CommandText = procName;
            command.Connection = (SqlConnection)connection;
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }
        public override IDataParameter CreateParameter(string parameterName, object parameterValue)
        {
            return new SqlParameter(parameterName, parameterValue);
        }

        
    }
}
