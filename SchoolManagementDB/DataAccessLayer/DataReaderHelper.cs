using System;
using System.Collections.Generic;
using System.Data;

namespace SchoolManagementDB.DataAccessLayer
{
    public static class DataReaderHelper
    {

        private static void DataReaderFieldNames(IDataReader reader)
        {
            string fieldNames = null;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                fieldNames = fieldNames + " " + string.Format("{0,-23}", reader.GetName(i).ToUpper());
            }
           Console.WriteLine(fieldNames + "\n");
        }
        private static void DataReaderFieldValues(IDataReader reader)
        {
            string fieldValues = null;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                //TODO: Look into date / datetime problems in C# vs MSSQL
                //There is a mix up with date / datetime, also CultureInfo determines the date format when displayed.
                if (reader[i].GetType() == typeof(DateTime))
                {
                    string dateString = reader[i].ToString();
                    int lastIndexOfDate = dateString.IndexOf(' ', 0);
                    string date = dateString.Substring(0, lastIndexOfDate);
                    fieldValues = fieldValues + " " + string.Format("{0,-23}", date);
                }
                else
                {
                    fieldValues = fieldValues + " " + string.Format("{0,-23}", reader[i]);
                }
            }
            Console.WriteLine(fieldValues);
        }


        public static void DataReaderPrint(IDataReader reader)
        {
            if (reader != null)
            {
                int counter = 0;
                while (reader.Read())
                {
                    counter++;
                    if (counter == 1)
                    {
                        DataReaderFieldNames(reader);
                    }
                    DataReaderFieldValues(reader);
                }
            }           
        }

        public static void DataReaderPrintAndStore(IDataReader reader, HashSet<int> storedIds)
        {
            if (reader != null)
            {
                int counter = 0;
                while (reader.Read())
                {
                    counter++;
                    if (counter == 1)
                    {
                        DataReaderFieldNames(reader);
                    }
                    DataReaderFieldValues(reader);
                    storedIds.Add(Convert.ToInt32(reader["Id"]));
                }
            }
        }
    }
}
