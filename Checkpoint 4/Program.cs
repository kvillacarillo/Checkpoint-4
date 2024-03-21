using System;
namespace Checkpoint4
{
    class Program
    {
        private static List<Student> records = new List<Student>();
        private static List<Subject> subjects = new List<Subject>();
        private delegate bool arrayOperations();

        public Program()
        {
            subjects = new List<Subject>()
            {
                           { new Subject { Id = 1, Name = "Mathematics" } },
                           { new Subject { Id = 2, Name = "Science" } },
                           { new Subject { Id = 3, Name = "English" } },
            };
        }
        static void Main(string[] args)
        {
            displayMenu();
        }

        private static void displayMenu()
        {
            bool gracefullyCompleted = false;
            var options = new Dictionary<int, arrayOperations>
            {
                {1, enrollStudent},
                {2, enterStudentGrade},
                {3, displayStudentGrade},
                {4, displayTopStudent},
                //{5, () => Environment.Exit(0)}
            };


           
            do
            {
                Console.WriteLine("Welcome to the Student Grades System!\r\n[1]Enroll Students\r\n[2]Enter Student Grades\r\n[3]Show Student Grades\r\n[4]Show Top Student\r\n[5]Exit");
                InputHandler<int>("Option", out int option);

                if (options.ContainsKey(option))
                {
                    try {
                        options[option]();
                        gracefullyCompleted = false;
                    } catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        gracefullyCompleted = false;

                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option selected!");
                    displayMenu();
                }

                if(InputHandler<bool>("Exit?", out bool toContinue))
                {
                    //Terminate
                    gracefullyCompleted = true;
                }
            } while (!gracefullyCompleted);


        }

        private static List<Student> _getAllStudents()
        {
            Console.WriteLine("Fetching all students . . .");
            return records;
        }
        private static T InputHandler<T>(String inputName, out T value) where T : IConvertible
        {
            string input;
            try
            {
                Console.WriteLine("Enter the " + inputName + $" [TYPE] <{typeof(T).Name}>");
                input = Console.ReadLine() ?? "";

                if (string.IsNullOrEmpty(input))
                {
                    throw new Exception("Blank values are not allowed!");
                }

                if (typeof(T) != typeof(T))
                {
                    throw new Exception("Datatype Error!");
                }


                if (typeof(T) == typeof(string) && input.Any(char.IsDigit))
                {
                    throw new Exception("String cannot contain numbers!");
                }

                value = (T)Convert.ChangeType(input, typeof(T));

                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return InputHandler(inputName, out value);
            }
        }

        private static bool enrollStudent()
        {
            try
            {
                Console.WriteLine("Add student . . .");
                InputHandler<string>("Student Name", out string name);

                var student = new Student
                {
                    Id = records?.Count + 1 ?? 1,
                    Name = name
                };
                records.Add(student);

        

                string formattedTable = $"{"Student Id",-10} {"Student Name",-20} {"Mathematics",-12} {"Science",-10} {"English",-10} {"Average",-10:F2}";

                Console.WriteLine("----------|--------------------|------------|----------|----------|----------");
                Console.WriteLine(formattedTable);
                Console.WriteLine("----------|--------------------|------------|----------|----------|----------");
                foreach (Student record in records)
                {
                    double math = record?.Grades?[0].SubGrade ?? 0.0;
                    double science = record?.Grades?[1].SubGrade ?? 0.0;
                    double english = record?.Grades?[2].SubGrade ?? 0.0;

                    double average = (math + science + english) / 3.0;
                    string body = string.Format("{0,-10} {1,-20} {2,-12} {3,-10} {4,-10} {5, -10:F2}", record.Id, record.Name, math, science, english, average);
                    Console.WriteLine(body);
                }

                Console.WriteLine("Student enrolled successfully!");

                if (InputHandler<bool>($"Enter again?", out bool enterAgain))
                {
                    enrollStudent();
                }

                return true;
            }
            catch (Exception e)
            {
               throw new Exception(e.Message);
            }


        }

        private static bool enterStudentGrade ()
        {
            Console.WriteLine("Entering student grade . . .");
            try
            {
                var students = _getAllStudents();
                if (students.Count == 0)
                {
                    throw new Exception("No student enrolled yet!");
                }

                InputHandler<int>("Student ID", out int studentId);
                Student student = students.Find(s => s.Id == studentId);

                if (student == null)
                {
                    throw new Exception("Student not found!");
                }
                InputHandler<double>("Student Grade in Mathematics", out double math);
                InputHandler<double>("Student Grade in English", out double english);
                InputHandler<double>("Student Grade in Science", out double science);

                List<Grade> newGrades = new List<Grade>()
                {
                    new Grade { SubjectId = "Mathematics", SubGrade = math },
                    new Grade { SubjectId = "Science", SubGrade = science },
                    new Grade { SubjectId = "English", SubGrade = english },
                };

                student.Grades = newGrades;
               
                double average = (math + english + science) / 3.0;

                string formattedTable = $"{"Student Id",-10} {"Student Name",-20} {"Mathematics",-12} {"Science",-10} {"English",-10} {"Average",-10:F2}";
                string body = string.Format("{0,-10} {1,-20} {2,-12} {3,-10} {4,-10} {5, -10:F2}", student.Id, student.Name, student.Grades[0].SubGrade, student.Grades[1].SubGrade, student.Grades[2].SubGrade, average);
               
                Console.WriteLine("----------|--------------------|------------|----------|----------|----------");
                Console.WriteLine(formattedTable);
                Console.WriteLine("----------|--------------------|------------|----------|----------|----------");
                Console.WriteLine(body);

                if (InputHandler<bool>($"Enter Grade Again?", out bool enterAgain))
                {
                    enterStudentGrade();
                }

                return true;

            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        private static bool displayTopStudent()
        {
            try
            {
                var students = _getAllStudents();
                if (students.Count == 0)
                {
                    throw new Exception("No student enrolled yet!");
                }
                Console.WriteLine("Getting Top Student . . .");

                double highestAverage = 0.0;
                Student topStudent = new Student();

                string formattedTable = $"{"Student Id",-10} {"Student Name",-20} {"Mathematics",-12} {"Science",-10} {"English",-10} {"Average",-10:F2}";

                Console.WriteLine("----------|--------------------|------------|----------|----------|----------");
                Console.WriteLine(formattedTable);
                Console.WriteLine("----------|--------------------|------------|----------|----------|----------");

                foreach (var student in students)
                {
                    double math = student?.Grades?[0].SubGrade ?? 0.0;
                    double science = student?.Grades?[1].SubGrade ?? 0.0;
                    double english = student?.Grades?[2].SubGrade ?? 0.0;

                    var average = (math + science + english) / 3.0;

                    if (average > highestAverage)
                    {
                        highestAverage = average;
                        topStudent = student;
                 
                    }
                }
                string body = string.Format("{0,-10} {1,-20} {2,-12} {3,-10} {4,-10} {5, -10:F2}", topStudent?.Id, topStudent?.Name, topStudent?.Grades?[0].SubGrade, topStudent?.Grades?[1].SubGrade, topStudent?.Grades?[1].SubGrade, highestAverage);
                Console.WriteLine(body);

                if (InputHandler<bool>($"View Again?", out bool enterAgain))
                {
                    displayTopStudent();
                }

                return true;
            }
             catch (Exception e) { 
                throw new Exception(e.Message);
            }

        }
        private static bool displayStudentGrade()
        {
            try
            {
                var students = _getAllStudents();
                if (students.Count == 0)
                {
                    throw new Exception("No student enrolled yet!");
                }
                Console.WriteLine("Fetching student . . .");
                string formattedTable = $"{"Student Id",-10} {"Student Name",-20} {"Mathematics",-12} {"Science",-10} {"English",-10} {"Average",-10:F2}";

                Console.WriteLine("----------|--------------------|------------|----------|----------|----------");
                Console.WriteLine(formattedTable);
                Console.WriteLine("----------|--------------------|------------|----------|----------|----------");

                foreach (Student student in students)
                {
                    double math = student?.Grades?[0].SubGrade ?? 0.0;
                    double science = student?.Grades?[1].SubGrade ?? 0.0;
                    double english = student?.Grades?[2].SubGrade ?? 0.0;

                    double average = (math + science + english) / 3.0;
                    string body = string.Format("{0,-10} {1,-20} {2,-12} {3,-10} {4,-10} {5, -10:F2}", student.Id, student.Name, math, science, english, average);
                    Console.WriteLine(body);
                }
                if (InputHandler<bool>($"View Again?", out bool enterAgain))
                {
                    displayTopStudent();
                }
                return true;
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Grade>? Grades { get; set; }
    }

    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public double Grade { get; set; } = 0.0;
    }

    public class Grade
    {
        public string SubjectId { get; set; } = "";
        public double SubGrade { get; set; } = 0.0;
    }
}