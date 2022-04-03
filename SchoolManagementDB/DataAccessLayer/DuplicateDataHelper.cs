using SchoolManagementDB.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


namespace SchoolManagementDB.DataAccessLayer
{
    /// <summary>
    /// Class that contains methods to check uniqueness of SchoolEntity instance against corresponding records in the database. 
    /// </summary>
    public static class DuplicateDataHelper
    {
        #region Check For duplicates in db definitions
        /// <summary>
        /// Returns a SchoolEntity object's List of PropertyInfo that map to the corresponding DB table fields
        /// that are either marked with a unique constraint, or participate in the formation of a unique constraint.
        /// </summary>
        /// <param name="schoolEntity">the instance of the SchoolEntity</param>
        /// <returns>the PropertyInfo object</returns>
        public static List<Tuple<PropertyInfo,string>> UniquenessCriteria(SchoolEntity schoolEntity)
        {
            
            List<Tuple<PropertyInfo, string>> uniqueProperties = new List<Tuple<PropertyInfo, string>>();
            if (schoolEntity is SchoolMember)
            {
                //Each person has a UNIQUE School Email. 
                uniqueProperties.Add(new Tuple<PropertyInfo,string>(schoolEntity.GetType().GetProperty("SchoolEmail"),"")); 
            }
            if (schoolEntity is Subject || schoolEntity is TaughtModule)
            {
                //Each subject/taughtModule has a UNIQUE code AND a UNIQUE Title
                uniqueProperties.Add(new Tuple<PropertyInfo, string>(schoolEntity.GetType().GetProperty("Code"),"OR"));
                uniqueProperties.Add(new Tuple<PropertyInfo, string>(schoolEntity.GetType().GetProperty("Title"),"OR")); ;
            }
            if (schoolEntity is Course)
            {
                //Each course has a UNIQUE COMBINATION of TaughtModuleId and SubjectId
                uniqueProperties.Add(new Tuple<PropertyInfo, string>(schoolEntity.GetType().GetProperty("TaughtModuleId"),"AND"));
                uniqueProperties.Add(new Tuple<PropertyInfo, string>(schoolEntity.GetType().GetProperty("SubjectId"), "AND"));
            }
            if (schoolEntity is Seminar)
            {
                //Each Seminar has a UNIQUE COMBINATION of CourseId and Term
                uniqueProperties.Add(new Tuple<PropertyInfo, string>(schoolEntity.GetType().GetProperty("CourseId"), "AND"));
                uniqueProperties.Add(new Tuple<PropertyInfo, string>(schoolEntity.GetType().GetProperty("Term"), "AND"));
            }
            if (schoolEntity is Enrollment)
            {
                //Each Seminar has a UNIQUE COMBINATION of SeminarId and StudentId
                uniqueProperties.Add(new Tuple<PropertyInfo, string>(schoolEntity.GetType().GetProperty("SeminarId"), "AND"));
                uniqueProperties.Add(new Tuple<PropertyInfo, string>(schoolEntity.GetType().GetProperty("StudentId"), "AND"));
            }
            return uniqueProperties;
        }
        /// <summary>
        /// The query to perform against the database to check whether the record to be inserted already exists.
        /// </summary>
        /// <param name="entity">instance of entity to be inserted</param>
        /// <param name="uniqueProperties">the entity properties that ensure uniqueness (i.e. parameters of query where clause)</param>
        /// <returns> the query as a string</returns>
        public static string CheckForDuplicateQuery(SchoolEntity entity, List<Tuple<PropertyInfo, string>> uniqueProperties)
        {
            StringBuilder commandSb = new StringBuilder();
            commandSb.Append($"SELECT Count(id) FROM {entity.GetType().Name} WHERE ");
            for (int i = 0; i < uniqueProperties.Count; i++)
            {
                commandSb.Append($"{uniqueProperties[i].Item1.Name}=");
                //Format strings in where clause
                if (uniqueProperties[i].Item1.PropertyType.Equals(typeof(string)))
                {
                    commandSb.Append($"'{uniqueProperties[i].Item1.GetValue(entity)}'");
                }//Format dates in where clause
                else if (uniqueProperties[i].Item1.PropertyType.Equals(typeof(DateTime)))
                {
                    var dateValue = (DateTime)uniqueProperties[i].Item1.GetValue(entity);
                    var formattedDate = dateValue.ToString("yyyy-MM-dd");
                    commandSb.Append($"'{formattedDate}'");
                }
                else if (uniqueProperties[i].Item1.PropertyType.IsEnum)//Format Enums in where clause
                {
                    //This cast may be a longterm problem. It assumes that all enum underlying types are int32. 
                    //(which is a recommended practice, but still..)
                    var enumVal = (int)uniqueProperties[i].Item1.GetValue(entity);
                    commandSb.Append($"{enumVal}");
                }
                else//all other cases
                {
                    commandSb.Append($"{uniqueProperties[i].Item1.GetValue(entity)}");
                }
                //At the end of each "WHERE [TablefieldValue]=[PropertyValue]" append " OR " or " AND " or ""
                if (i<uniqueProperties.Count-1)
                {
                    commandSb.Append($" {uniqueProperties[i].Item2} ");
                }
            }
            //Console.WriteLine(commandSb);
            return commandSb.ToString();
        }

       
        #endregion 
    }
}
