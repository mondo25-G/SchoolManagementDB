using SchoolManagementDB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;



namespace SchoolManagementDB.DataAccessLayer
{

    /// <summary>
    /// SchoolManager is a class that deals with data insertion/retrieval from a database.
    /// There is some logic inserted here that is dependent on the specific database schema used in the project, 
    /// and on specific naming conventions in model properties so that the mapping between them is consistent.
    /// (For this small project it was a deliberate choice not to let an ORM do the work but use only ADO.NET)
    /// </summary>
    public sealed class SchoolManager<T>: DataWorker where T : SchoolEntity,new()
    {

        #region CRUD implementations
        public static void CreateRecord()
        {
            //This is the only place where we instantiate a SchoolEntity.
            T schoolEntity = new T();
            List<Tuple<PropertyInfo, string>> uniqCriteria = new List<Tuple<PropertyInfo, string>>(schoolEntity.GetType().GetProperties().Length);
            //Here we check for duplicates in db.
            if (IsDuplicate(schoolEntity, out uniqCriteria))
            {
                StringBuilder message = new StringBuilder();
                message.Append($"A {schoolEntity.GetType().Name} already exists with the same ");
                for (int i = 0; i < uniqCriteria.Count; i++)
                {
                    message.Append($"{uniqCriteria[i].Item1.Name} ");
                    if (i < uniqCriteria.Count - 1)
                    {
                        message.Append($"{uniqCriteria[i].Item2} ");
                    }
                }
                Console.WriteLine(message);
            }
            else
            {
                using (IDbConnection connection = database.CreateOpenConnection())
                {
                    using (IDbCommand insertCommand = database.CreateStoredProcCommand(StoredProcHelper.StoredProcInsert<T>(), connection))
                    {
                        foreach (PropertyInfo prop in schoolEntity.GetType().GetProperties())
                        {
                            if (!prop.Name.Equals("Id"))
                            {
                                IDataParameter parameter = database.CreateParameter($"@{prop.Name}", prop.GetValue(schoolEntity));
                                insertCommand.Parameters.Add(parameter);
                            }
                        }
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }
        /// <summary>
        /// Reads all records of a table.
        /// </summary>
        public static void ReadAllRecords()
        {
            using (IDbConnection connection = database.CreateOpenConnection())
            {
                using (IDbCommand command = database.CreateStoredProcCommand(StoredProcHelper.StoredProcReadAll<T>(), connection))
                {
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataReaderHelper.DataReaderPrint(reader);
                    }
                }
            }
        }
        #endregion
        #region Further Schema specific Methods
        /// <summary>
        /// Reads some aggregation of all records of a table (corresponds to T) over the all the records of another table (corresponds to U).
        /// Example: students per course, courses taken by each student e.t.c
        /// </summary>
        /// <typeparam name="U">the school entity type to aggregate over</typeparam>
        public static void ReadAllRecords<U>() where U : SchoolEntity
        {
            using (IDbConnection connection = database.CreateOpenConnection())
            {
                using (IDbCommand command = database.CreateStoredProcCommand(StoredProcHelper.StoredProcReadAll<T, U>(), connection))
                {
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataReaderHelper.DataReaderPrint(reader);
                    }
                }
            }
        }
        /// <summary>
        /// Helper to insert table fields that are foreign keys of another table
        /// </summary>
        /// <param name="connection">the db connection</param>
        /// <returns></returns>
        private static HashSet<int> ReadAllIds(IDbConnection connection,string tableName)
        {
            HashSet<int> entityIds = new HashSet<int>();
            using (IDbCommand command = database.CreateCommand($"SELECT * from {tableName} order by Id", connection))
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    DataReaderHelper.DataReaderPrintAndStore(reader, entityIds);
                }
            }
            return entityIds;
        }
        /// <summary>
        /// Checks if the record to be inserted already exists in db. This is accomplished by a db-schema specific record uniqueness criteria<br></br>
        /// and the application of the appropriate select query in the db.
        /// </summary>
        /// <param name="entity">instance of entity (i.e. the record) to insert</param>
        /// <param name="uniquenessCriteria">uniqueness criteria to apply in select query</param>
        /// <returns>true if record already exists, false otherwise</returns>
        private static bool IsDuplicate(T entity, out List<Tuple<PropertyInfo, string>> uniquenessCriteria)
        {
            bool isDuplicate;
            int queryResult = 0;
            uniquenessCriteria = DuplicateDataHelper.UniquenessCriteria(entity);

            string commandSb = DuplicateDataHelper.CheckForDuplicateQuery(entity, uniquenessCriteria);

            using (IDbConnection connection = database.CreateOpenConnection())
            {
                using (IDbCommand command = database.CreateCommand(commandSb, connection))
                {
                    queryResult = (int)command.ExecuteScalar();
                }
            }
            return isDuplicate = (queryResult == 0) ? false : true;
        }
        /// <summary>
        /// Given an entity, inserts a foreign key value to the relevant db table field.
        /// </summary>
        /// <param name="entity">The instance of the entity</param>
        public static void InsertForeignKey(T entity,PropertyInfo prop)
        {
            //This is also the place where we defer insertion details of entity's property values 
            //that correspond to foreign keys in the respective db table.
            Console.WriteLine($"Select {prop.Name} to insert to {entity.GetType().Name}");
            //extract table name
            string entityForeignKeyName = prop.Name.Substring(0, prop.Name.Length - 2);
            HashSet<int> entityIds;
            int id;
            using (IDbConnection connection = database.CreateOpenConnection())
            {
                 entityIds = ReadAllIds(connection,entityForeignKeyName);
            }
            while (!int.TryParse(Console.ReadLine(), out id) || !entityIds.Contains(id))
            {
                 Console.WriteLine("You either gave an invalid Id (int) or the id is not in the records.");
            }
            prop.SetValue(entity, id);
        }

        #endregion


    }
}