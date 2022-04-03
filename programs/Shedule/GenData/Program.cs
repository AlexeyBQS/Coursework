using GenData.Database;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace GenData
{
    internal class Program
    {
        private delegate void Action();
        private delegate int GenRandomValue();

        private static bool _CancelProgram = false;
        private static readonly string DatabaseNameDefault = "SheduleDatabase";
        private static string _DatabaseName = string.Empty;

        private static bool CancelProgram
        {
            get { return _CancelProgram; }
            set
            {
                _CancelProgram = value;

                if (_CancelProgram == true)
                {
                    Environment.Exit(0);
                }
            }
        }
        private static string DatabaseName
        {
            get
            {
                if (_DatabaseName == string.Empty)
                {
                    _DatabaseName = ConfigurationManager.ConnectionStrings["DatabaseName"].ConnectionString;
                }

                return _DatabaseName;
            }
        }

        private static string[] Teachers = new string[]
        {
            "Баранова Маргарита Сергеевна",
            "Чернова Ева Денисовна",
            "Савельев Глеб Максимович",
            "Дмитриева Елизавета Владимировна",
            "Куликов Даниил Фёдорович",
            "Баранова Василиса Львовна",
            "Ильинский Артём Вячеславович",
            "Павлова Кира Александровна",
            "Антонова Малика Тимофеевна",
            "Сергеева Анастасия Арсентьевна",
            "Попов Арсений Сергеевич",
            "Степанова Ксения Никитична",
            "Соболева Алёна Данииловна",
            "Виноградов Алексей Тимофеевич",
            "Лазарева Юлия Артёмовна",
            "Серов Денис Фёдорович",
            "Морозова Екатерина Максимовна",
            "Фролова Елизавета Ивановна",
            "Белоусова Ксения Тимофеевна",
            "Быков Давид Глебович",
            "Леонтьева Анна Артёмовна",
            "Голубева Полина Егоровна",
            "Никитина Виктория Богдановна",
            "Панова Мария Александровна",
            "Вишневский Илья Максимович",
            "Панфилова Ясмина Владимировна",
            "Михайлова Виктория Вячеславовна",
            "Козлова Марина Львовна",
            "Морозов Иван Савельевич",
            "Астахов Лев Романович",
            "Игнатова Амира Ивановна",
            "Соколова Ясмина Львовна",
            "Никитина Оливия Ильинична",
            "Потапов Ростислав Германович",
            "Николаева София Ильинична",
            "Филиппов Алексей Григорьевич",
            "Беляев Илья Кириллович",
            "Филимонова Ульяна Максимовна",
            "Савельева Василиса Мироновна",
            "Виноградова Дарья Данииловна",
            "Платонов Григорий Фёдорович",
            "Бобров Вадим Кириллович",
            "Карпова Анастасия Львовна",
            "Орехов Мирон Михайлович",
            "Кудрявцева Афина Артуровна",
            "Ермилова Ева Александровна",
            "Сергеева Варвара Кирилловна",
            "Ульянов Артём Степанович",
            "Новикова Виктория Михайловна",
            "Романова Александра Михайловна"
        };

        private static void GenTeachers()
        {
            string lastPassport = "";
            Random rand = new Random();

            using (EducateContext context = new EducateContext())
            {
                Teachers.ToList().ForEach(teacher =>
                {
                    string passport = ((long)(new Random().NextDouble() * 10000000000)).ToString();
                    while (passport.Length != 10 || passport == lastPassport)
                        passport = ((long)(new Random().NextDouble() * 10000000000)).ToString();

                    string[] parts = teacher.Split(' ');

                    context.Teachers.Add(
                        new Teacher
                        {
                            Surname = parts[0],
                            Name = parts[1],
                            Midname = parts[2],
                            Birthday = new DateTime(rand.Next(1960, 1999), rand.Next(1, 13), rand.Next(1, 27)),
                            Passport = passport
                        });

                    lastPassport = passport;
                });

                context.SaveChanges();
            }
        }
        private static void GenCabinets()
        {
            using (EducateContext context = new EducateContext())
            {
                for (int i = 1; i <= 50; ++i)
                    context.Cabinets.Add(
                        new Cabinet
                        {
                            TeacherId = i,
                            Name = i.ToString()
                        });

                context.SaveChanges();
            }
        }
        private static ICollection<Class> GetClasses()
        {
            List<Class> classes = new List<Class>();

            int id = 1;
            int teacherId = 1;

            for (int i = 1; i <= 11; ++i)
                for (int j = 0; j < 3; ++j)
                    classes.Add(
                        new Class
                        {
                            ClassId = id++,
                            TeacherId = teacherId++,
                            Name = $"{i}{(new[] { 'а', 'б', 'в' })[j]}",
                            CountPupils = new Random().Next(20, 30)
                        });

            return classes;
        }
        private static void GenClasses()
        {
            using (EducateContext context = new EducateContext())
            {
                context.Classes.AddRange(GetClasses());
                context.SaveChanges();
            }
        }
        private static ICollection<Lesson> GetLessons()
        {
            string[] nameLessons = new string[] { "Русский язык", "Математика", "Литература" };
            List<Class> classes = GetClasses().ToList();
            List<Lesson> lessons = new List<Lesson>();


            List<int> lastCountHours = new List<int>();
            GenRandomValue grv = delegate ()
            {
                Random rand = new Random();
                int result = rand.Next(50, 150);

                while (lastCountHours.Contains(result))
                    result = rand.Next(50, 150);

                lastCountHours.Add(result);
                return result;
            };

            int id = 1;
            classes.ForEach(x => nameLessons.ToList().ForEach(y => lessons.Add(
                    new Lesson
                    {
                        LessonId = id++,
                        TeacherId = x.TeacherId,
                        ClassId = x.ClassId,
                        Name = y,
                        CountHours = grv.Invoke()
                    })));

            return lessons;
        }
        private static void GenLessons()
        {
            using (EducateContext context = new EducateContext())
            {
                context.Lessons.AddRange(GetLessons());
                context.SaveChanges();
            }
        }
        private enum DayOfWeek
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }
        private static ICollection<object[]> GetDate()
        {
            List<object[]> dateTimes = new List<object[]>();
            //DateTime nowDate = new DateTime(2022, 1, 2);

            object[] nowDate = new object[] { new DateTime(2022, 1, 1), DayOfWeek.Thursday };
            int dayOfWeek = (int)nowDate[1];

            while ((DateTime)nowDate[0] < new DateTime(2023, 5, 31))
            {
                nowDate[0] = ((DateTime)nowDate[0]).AddDays(1);

                dayOfWeek += 1;

                if (dayOfWeek < 7)
                    nowDate[1] = (DayOfWeek)dayOfWeek;
                else
                {
                    dayOfWeek = 0;
                    nowDate[1] = (DayOfWeek)dayOfWeek;
                }

                dateTimes.Add(new object[] { (DateTime)nowDate[0], (DayOfWeek)nowDate[1] });
            }

            return dateTimes;
        }
        private static ICollection<Shedule> GetShedules()
        {
            string[] nameLessons = new string[] { "Русский язык", "Математика", "Литература" };
            List<Class> classes = GetClasses().ToList();
            List<Lesson> lessons = GetLessons().ToList();
            List<object[]> dateTimes = GetDate().ToList();
            List<Shedule> shedules = new List<Shedule>();

            foreach (object[] day in dateTimes)
            {
                if ((DayOfWeek)day[1] != DayOfWeek.Saturday && (DayOfWeek)day[1] != DayOfWeek.Sunday)
                {
                    foreach (Class @class in classes)
                    {
                        List<Lesson> classLessons = lessons.Where(x => x.ClassId == @class.ClassId).ToList();
                        int numberLesson = 1;

                        for (int i = 0; i < 2; ++i)
                        {
                            foreach (Lesson lesson in classLessons)
                            {
                                shedules.Add(
                                    new Shedule
                                    {
                                        Day = (DateTime)day[0],
                                        NumberLesson = numberLesson++,
                                        ClassId = @class.ClassId,
                                        LessonId = lesson.LessonId,
                                        CabinetId = @class.ClassId
                                    });
                            }
                        }
                    }
                }
            }

            return shedules;
        }
        private static void GenShedules()
        {
            using (EducateContext context = new EducateContext())
            {
                context.Shedules.AddRange(GetShedules());
                context.SaveChanges();
            }
        }

        private static void ExecuteAndPush(string text, Action action)
        {
            Console.Write($"{text}... ");
            action.Invoke();
            Console.WriteLine("Complete!");
        }
        static void Start()
        {
            ExecuteAndPush("Creating and connection database", () => new EducateContext());

            ExecuteAndPush("Generate data in table \"Teachers\"", () => GenTeachers());

            ExecuteAndPush("Generate data in table \"Cabinets\"", () => GenCabinets());

            ExecuteAndPush("Generate data in table \"Classes\"", () => GenClasses());

            ExecuteAndPush("Generate data in table \"Lessons\"", () => GenLessons());

            ExecuteAndPush("Generate data in table \"Shedules\"", () => GenShedules());
        }

        // For work of executable file without files of project
        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = new AssemblyName(args.Name);

            string path = assemblyName.Name + ".dll";

            if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false)
            {
                path = string.Format(@"{0}\{1}", assemblyName.CultureInfo, path);
            }

            using (Stream stream = executingAssembly.GetManifestResourceStream(path))
            {
                if (stream == null) return null;

                byte[] assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);

                return Assembly.Load(assemblyRawBytes);
            }
        }

        private static void CheckConfigProgramFile()
        {
            string fullPath = $"{Directory.GetCurrentDirectory()}\\GenData.exe.Config";
            if (!File.Exists(fullPath))
            {
                string content =
                    $"<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                    + $"\n<configuration>"
                    + $"\n   <startup>"
                    + $"\n       <supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.8\"/>"
                    + $"\n   </startup>"
                    + $"\n   <connectionStrings>"
                    + $"\n       <add name=\"DatabaseName\" connectionString=\"{DatabaseNameDefault}\" providerName=\"\"/>"
                    + $"\n       <add name=\"EducateConnection\" connectionString=\"\" providerName=\"\"/>"
                    + $"\n   </connectionStrings>"
                    + $"\n</configuration>";

                File.WriteAllText(fullPath, content);
            }
        }

        private static bool ExistSqlLocalDb()
        {
            Process cmd = new Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("sqllocaldb info");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();

            return cmd.StandardOutput.ReadToEnd().Contains("MSSQLLocalDB");
        }

        private static void ChangePathConnectionStringDatabase()
        {
            if (!ExistSqlLocalDb())
            {
                MessageBox.Show(
                    "Отсутствует установленная программа Microsoft SQL Server!\nУстановите программу или обратитесь к системному администратору.",
                    "Ошибка!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                CancelProgram = true;
            }

            string DataSource = "(LocalDB)\\MSSQLLocalDB";
            string AttachDbFilename = $"{Directory.GetCurrentDirectory()}\\{DatabaseName}.mdf";
            bool IntegratedSecurity = true;
            string ApplicationIntent = "ReadWrite";
            bool MultiSubnetFailover = false;
            bool TrustedConnection = true;

            string connectionString =
                $"Data Source = {DataSource};"
                + $" AttachDbFilename = {AttachDbFilename};"
                + $" Integrated Security = {IntegratedSecurity};"
                + $" ApplicationIntent = {ApplicationIntent};"
                + $" MultiSubnetFailover = {MultiSubnetFailover};"
                + $" Trusted_Connection = {TrustedConnection}";

            string providerName = "System.Data.SqlClient";

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");

            connectionStringsSection.ConnectionStrings["EducateConnection"].ConnectionString = connectionString;
            connectionStringsSection.ConnectionStrings["EducateConnection"].ProviderName = providerName;
            config.Save();

            ConfigurationManager.RefreshSection("connectionStrings");
        }

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;

            CheckConfigProgramFile();
            ChangePathConnectionStringDatabase();

            Start();
        }
    }
}
