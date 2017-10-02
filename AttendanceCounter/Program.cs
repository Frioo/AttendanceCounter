using Synergia.NET;
using Synergia.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceCounter
{
    class Program
    {
        static SynergiaClient client = new SynergiaClient();
        static List<Subject> subjects;
        //static Dictionary<string, Subject> subjectsDict;
        static List<Attendance> attendances;
        static Dictionary<string, Attendance> attendancesDict;
        static List<AttendanceCategory> categories;
        static Dictionary<string, AttendanceCategory> categoriesDict;
        static List<Lesson> lessons;
        static Dictionary<string, Lesson> lessonsDict;

        static void Main(string[] args)
        {
            Console.Title = "Kalkulator frekwencji | Librus/Synergia";
            Login();
            ShowSubjects();
            Console.ReadKey(false);
        }

        static void Login()
        {
            Console.WriteLine("Zaloguj się");
            Console.Write("Login: ");
            var username = Console.ReadLine();
            Console.Write("Hasło: ");
            var password = Console.ReadLine();
            Console.WriteLine("Trwa logowanie...");
            client.Login(username, password);
            Console.Clear();
        }

        static void ShowSubjects()
        {
            if (subjects == null)
            {
                Console.WriteLine("Trwa pobieranie przedmiotów...");
                subjects = client.GetSubjects();
            }
            Console.Clear();

            for (int i = 0; i < subjects.Count; i++)
            {
                Console.WriteLine(i + 1 + @". " + subjects[i].Name);
            }

            int choice = int.Parse(Console.ReadLine()) - 1;
            Console.Clear();
            ShowAttendances(subjects[choice]);
        }

        static void ShowAttendances(Subject s)
        {
            Console.WriteLine("Trwa pobieranie danych...");

            if (attendances == null)
            {
                attendances = client.GetAttendances();
            }
            if (attendancesDict == null)
            {
                attendancesDict = client.GetAttendancesIDDictionary();
            }
            if (categories == null)
            {
                categories = client.GetAttendanceCategories();
            }
            if (categoriesDict == null)
            {
                categoriesDict = client.GetAttendanceCategoriesIDDictionary();
            }
            if (lessons == null)
            {
                lessons = client.GetLessons();
            }
            if (lessonsDict == null)
            {
                lessonsDict = client.GetLessonsIDDictionary();
            }
            Console.Clear();
            int total = 0;
            int absenceCount = 0;

            for (int i = 0; i < attendances.Count; i++)
            {
                Attendance a = attendances[i];
                if (lessonsDict[a.LessonID].SubjectID == s.ID.ToString())
                {
                    total++;
                    if (!categoriesDict[a.TypeID].IsPresenceType)
                    {
                        absenceCount++;
                    }
                }
            }
            double absences = double.Parse(absenceCount.ToString());
            double all = double.Parse(total.ToString());
            Console.WriteLine(s.Name);
            Console.WriteLine("Nieobecności: {0}", absenceCount);
            Console.WriteLine("Frekwencja: {0}", 100 - Math.Round(absences / all * 100, 1) + @"%" + Environment.NewLine);
            Console.WriteLine("Nacisnij dowolny klawisz aby wrócić...");
            Console.ReadKey(false);
            ShowSubjects();
        }
    }
}
