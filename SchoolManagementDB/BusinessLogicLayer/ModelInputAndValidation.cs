using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SchoolManagementDB.Models;
using System.Text.RegularExpressions;
using System.Globalization;
using SchoolManagementDB.DataAccessLayer;

namespace SchoolManagementDB.BusinessLogicLayer
{
    /// <summary>
    /// Class that performs a few simple data validations and inserts them  to model properties.
    /// </summary>
    public class ModelInputAndValidation
    {
        /// <summary>
        /// Uses reflection to read/validate input data from command line and set all corresponding school Entity properties
        /// </summary>
        /// <param name="schoolEntity">the School Entity instance</param>
        public static void InsertDataToModel(SchoolEntity schoolEntity)
        {
            var props = schoolEntity.GetType().GetProperties().Reverse();//That .Reverse() is annoying.
            
            //Deal with object properties that are foreign keys
            IEnumerable<PropertyInfo> propertiesForeignKeys = props.Where(x => !x.Name.Equals("Id") && x.Name.EndsWith("Id"));
            foreach (PropertyInfo property in propertiesForeignKeys)
            {
                SchoolManager<SchoolEntity>.InsertForeignKey(schoolEntity, property);
            }
            //Deal with all other properties
            IEnumerable<PropertyInfo> propertiesNotForeignKeys = props.Where(x=>!x.Name.EndsWith("Id"));
            foreach (PropertyInfo property in propertiesNotForeignKeys)
            {
                if (property.PropertyType.Equals(typeof(string)))
                {
                    //Seminar.Title and Course.Code are generated in database via triggers.
                    if (schoolEntity is Seminar && property.Name.Equals("Title"))
                    {
                        continue;
                    }
                    else if (schoolEntity is Course && property.Name.Equals("Code"))
                    {
                        continue;
                    }
                    else
                    {
                        ValidateString(schoolEntity, property);
                    }
                }

                if (property.PropertyType.Equals(typeof(DateTime)))
                {
                    ValidateDate(schoolEntity, property);
                }

                if (property.PropertyType.Equals(typeof(int)))
                {
                    ValidateInt(schoolEntity, property);
                }

                if (property.PropertyType.IsEnum)
                {
                    ValidateEnum(schoolEntity, property);
                }

            }
        }

        /// <summary>
        /// Validates an input string and sets a School Entity's property (typeof string) to its value.
        /// </summary>
        /// <param name="person">the school entity instance</param>
        /// <param name="property">the school entity property (typeof string)</param>
        public static void ValidateString(SchoolEntity schoolEntity, PropertyInfo property)
        {
            string model = schoolEntity.GetType().Name;
            string input;
            do
            {
                if (schoolEntity is SchoolMember && property.Name.Equals("SchoolEmail"))
                {
                    Console.WriteLine($"Give the {model}'s (unique) email prefix (lowercase letters and numbers only). Suffix generated automatically.");
                }
                else
                {
                    Console.Write($"Give the {model}'s {property.Name}: ");
                }

                input = Console.ReadLine();

                if (input.Contains("'"))//Problems in db if I allow that.
                {
                    Console.WriteLine($"{ model}'s {property.Name} cannot contain single quotes.");
                }
                else if (String.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine($"{model}'s {property.Name} cannot be empty");
                }
                else if (schoolEntity is SchoolMember && ((property.Name.Equals("FirstName") || property.Name.Equals("LastName")) && !Regex.IsMatch(input, @"^[a-zA-Z]+$")))
                {
                    Console.WriteLine($"{model}'s {property.Name} can only contain letters");
                }
                else if (schoolEntity is SchoolMember && property.Name.Equals("SchoolEmail") && (!Regex.IsMatch(input, @"^[a-z0-9]+$") || Regex.IsMatch(input, @"^[0-9]+$")))
                {
                    Console.WriteLine($"{model}'s {property.Name} is not a valid {property.Name}.");
                }
                else if ((schoolEntity is Subject || schoolEntity is TaughtModule) && property.Name.Equals("Code") && (!Regex.IsMatch(input, @"^[a-zA-Z0-9]+$") || Regex.IsMatch(input, @"^[0-9]+$")))
                {
                    Console.WriteLine($"{model}'s {property.Name} can be letters only or letters and numbers.");
                }
                //TODO: Find regex for subject,taught module to avoid input with only special characters or only letters etc.
                else
                {

                    if (schoolEntity is Student && property.Name.Equals("SchoolEmail"))
                    {
                        property.SetValue(schoolEntity, input + "@stud.bootcamp.org");
                    }
                    else if (schoolEntity is Lecturer && property.Name.Equals("SchoolEmail"))
                    {
                        property.SetValue(schoolEntity, input + "@fac.bootcamp.org");
                    }
                    else
                    {
                        if (schoolEntity is Subject || schoolEntity is TaughtModule)
                        {
                            property.SetValue(schoolEntity, input.ToUpper().Trim());
                        }
                        else
                        {
                            property.SetValue(schoolEntity, input.Trim());
                        }
                    }

                    break;
                }
            } while (true);
        }

        /// <summary>
        /// Validates an input string as datetime with format dd/MM/yyyy <br></br>
        /// and sets a School Entity's property (typeof DateTime) to its value.
        /// </summary>
        /// <param name="schoolEntity">the school entity instance</param>
        /// <param name="property">the school entity property (typeof Datetime)</param>
        public static void ValidateDate(SchoolEntity schoolEntity, PropertyInfo property)
        {
            string model = schoolEntity.GetType().Name;
            DateTime date;

            Console.Write($"Give the {model}'s {property.Name} with format (yyyy-MM-dd):");
            if (schoolEntity is SchoolMember && property.Name.Equals("DateOfBirth"))
            {
                while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                || date.CompareTo(DateTime.Now) > 0)
                {
                    Console.WriteLine("You did not give a date or the date has not come yet.");
                    Console.Write($"Give the {model}'s {property.Name}: ");

                }
                property.SetValue(schoolEntity, date);
            }
        }

        public static void ValidateInt(SchoolEntity schoolEntity, PropertyInfo property)
        {
            string model = schoolEntity.GetType().Name;
            Console.Write($"Give the {model}'s {property.Name}: ");
            int input;
            if (!int.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("You did not input a valid integer.");
                ValidateInt(schoolEntity, property);
            }
            else
            {
                if (schoolEntity is Course && property.Name.Equals("Credits") && (input <= 0))
                {
                    Console.WriteLine("Course Credits must be > 0 (int).");
                    ValidateInt(schoolEntity, property);
                }
            }
                property.SetValue(schoolEntity, input);
        }

        /// <summary>
        /// Validates/parses an integer and checks against all values of the corresponding enum property <br></br>
        /// of a school entity.
        /// </summary>
        /// <param name="schoolEntity"> the school entity instance </param>
        /// <param name="property"> the school entity property (typeof enum)</param>
        public static void ValidateEnum(SchoolEntity schoolEntity, PropertyInfo property)
        {
            string model = schoolEntity.GetType().Name;
            int enumItem;
            List<int> enumValues = new List<int>(schoolEntity.GetType().GetProperties().Length);

            Console.Write($"Select the {model}'s {property.Name}:\n");
            foreach (string enumName in Enum.GetNames(property.PropertyType))
            {
                var parsedEnum = Enum.Parse(property.PropertyType, enumName);
                Console.WriteLine("{0} = {1:D}", enumName, parsedEnum);
                enumValues.Add((int)parsedEnum);
            }

            while (!int.TryParse(Console.ReadLine(), out enumItem) || !enumValues.Contains(enumItem))
            {
                Console.WriteLine($"You did not select a valid {property.Name}");
            }

            property.SetValue(schoolEntity, enumItem);
        }
    }
}
