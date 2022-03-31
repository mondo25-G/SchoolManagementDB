using SchoolManagementDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementDB.DataAccessLayer
{
    internal static class StoredProcHelper
    {
        #region Stored procedures definitions
        internal static string StoredProcReadAll<T>() 
        {
            string storedProc = $"usp_GetAll{typeof(T).Name}s";
            return storedProc;
        }
        //Stored procs to retrieve aggregated/grouped data
        internal static string StoredProcReadAll<T, U>()
        {
            string storedProc = $"usp_Get{typeof(T).Name}sPer{typeof(U).Name}";
            return storedProc;
        }
        internal static string StoredProcInsert<T>()
        {
            string storedProc = $"usp_Insert{typeof(T).Name}";
            return storedProc;
        }
       
        #endregion
    }
}
