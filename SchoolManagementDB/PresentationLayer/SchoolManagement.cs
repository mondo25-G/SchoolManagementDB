using SchoolManagementDB.DataAccessLayer;
using SchoolManagementDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementDB.PresentationLayer
{
    class SchoolManagement
    {
        public SchoolManagement()
        {
            Console.WriteLine("Welcome to the Coding Bootcamp Admin Panel.\n");
            Menu();
        }
        private void Menu()
        {
            Console.SetWindowSize(160,30);
            string choice;
            do
            {

                Console.WriteLine("To read data from Coding Bootcamp DB press 1.");
                Console.WriteLine("To insert data to Coding Bootcamp DB press 2.");
                Console.WriteLine("To exit press 3."); ;
                choice = Console.ReadLine();

            } while (choice != "1" && choice != "2" && choice != "3");

            if (choice == "1")
            {
                RetrieveMenu();
            }
            if (choice == "2")
            {
                InsertMenu();
            }
            if (choice =="3")
            {
                Environment.Exit(0);
            }

        }

        private void RetrieveMenu()
        {
            string choice;
            do
            {
                Console.WriteLine("To get all students press 1");
                Console.WriteLine("To get all lecturers press 2");
                Console.WriteLine("To get all taught modules press 3");
                Console.WriteLine("To get all subjects press 4");
                Console.WriteLine("To get all courses press 5");
                Console.WriteLine("To get all seminars press 6");
                Console.WriteLine("To get all enrollments pres 7");
                Console.WriteLine("To retrieve coding bootcamp statistics press 8");
                Console.WriteLine("To get to main menu press 9");
                choice = Console.ReadLine();
            } while (choice != "1" && choice != "2" && choice != "3" && choice != "4" && choice != "5"
            && choice != "6" && choice != "7" && choice != "8" && choice != "8" && choice!="9");

            if (choice == "9")
            {
                Menu();
            }
            else
            {
                RetrieveActions(choice);
            }
        }

        private void RetrieveActions(string choice)
        {
            switch (choice)
            {
                case "1":
                    SchoolManager<Student>.ReadAllRecords();
                    break;
                case "2":
                    SchoolManager<Lecturer>.ReadAllRecords();
                    break;
                case "3":
                    SchoolManager<TaughtModule>.ReadAllRecords();
                    break;
                case "4":
                    SchoolManager<Subject>.ReadAllRecords();
                    break;
                case "5":
                    SchoolManager<Course>.ReadAllRecords();
                    break;
                case "6":
                    SchoolManager<Seminar>.ReadAllRecords();
                    break;
                case "7":
                    SchoolManager<Enrollment>.ReadAllRecords();
                    break;
                case "8":
                    StatisticsMenu();
                    break;
                case "9":
                    Menu();
                    break;
            }
            string nextChoice;
            do
            {
                Console.WriteLine("Press 1  to look up more data or 2 to go back to main menu");
                nextChoice = Console.ReadLine();
            } while (nextChoice != "1" && nextChoice != "2");

            if (nextChoice == "1")
            {
                RetrieveMenu();
            }

            if (nextChoice == "2")
            {
                Menu();
            }
        }


        public void StatisticsMenu()
        {
            string choice;
            do
            {
                Console.WriteLine("To get total number of students per seminar press 1");
                Console.WriteLine("To get total number of seminars per student press 2");
                Console.WriteLine("To get to main menu press 3");
                choice = Console.ReadLine();
            } while (choice != "1" && choice != "2" && choice != "3" );

            if (choice == "3")
            {
                Menu();
            }
            else
            {
                StatisticsActions(choice);
            }
        }

        public void StatisticsActions(string choice)
        {
            switch (choice)
            {
                case "1":
                    SchoolManager<Student>.ReadAllRecords<Seminar>();
                    break;
                case "2":
                    SchoolManager<Seminar>.ReadAllRecords<Student>();
                    break;
            }
        }

        public void InsertMenu()
        {
            string choice;
            do
            {
                Console.WriteLine("To insert student press 1");
                Console.WriteLine("To insert lecturer press 2");
                Console.WriteLine("To insert taught module press 3");
                Console.WriteLine("To insert subject press 4");
                Console.WriteLine("To insert course press 5");
                Console.WriteLine("To insert seminar press 6");
                Console.WriteLine("To insert student to seminar press 7");
                Console.WriteLine("To get to main menu press 8");
                choice = Console.ReadLine();
            } while (choice != "1" && choice != "2" && choice != "3" && choice != "4" && choice != "5"
            && choice != "6" && choice != "7" && choice!="8");

            if (choice == "8")
            {
                Menu();
            }
            else
            {
                InsertActions(choice);
            }
        }

        private void InsertActions(string choice)
        {
            switch (choice)
            {
                case "1":
                    SchoolManager<Student>.CreateRecord();
                    break;
                case "2":
                    SchoolManager<Lecturer>.CreateRecord();
                    break;
                case "3":
                    SchoolManager<TaughtModule>.CreateRecord();
                    break;
                case "4":
                    SchoolManager<Subject>.CreateRecord();
                    break;
                case "5":
                    SchoolManager<Course>.CreateRecord();
                    break;
                case "6":
                    SchoolManager<Seminar>.CreateRecord();
                    break;
                case "7":
                    SchoolManager<Enrollment>.CreateRecord();
                    break;
                case "8":
                    Menu();
                    break;

            }
            string nextChoice;
            do
            {
                Console.WriteLine("Press 1  to insert more data or 2 to go back to main menu");
                nextChoice = Console.ReadLine();
            } while (nextChoice != "1" && nextChoice != "2");

            if (nextChoice == "1")
            {
                InsertMenu();
            }
            if (nextChoice == "2")
            {
                Menu();
            }
        }

        
    }
}
