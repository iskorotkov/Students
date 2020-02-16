using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Students.CommandLine
{
    static class Program
    {
        private const string Folder = @"C:\Projects\Students";
        private const string Filename = "students.xml";
        
        private static void Main()
        {
            var b1 = new Bachelor
            {
                FirstName = "Ivan",
                SecondName = "Korotkov",
                Faculty = "Math"
            };

            var m1 = new Master
            {
                FirstName = "Ivan",
                SecondName = "Korotkov",
                Faculty = "Math",
                Degree = new Degree
                {
                    GraduationDate = DateTime.Today,
                    Domain = "Programming"
                }
            };

            var studs = new List<Student> {b1, m1};
            Serialize(studs);
            var studs2 = Deserialize();
        }

        private static void Serialize(List<Student> student)
        {
            var serializer = new XmlSerializer(typeof(List<Student>), new[] {typeof(Student), typeof(Bachelor), typeof(Master)});
            using var stream = new FileStream(Path.Combine(Folder, Filename), FileMode.Create);
            serializer.Serialize(stream, student);
        }

        private static List<Student> Deserialize()
        {
            var serializer = new XmlSerializer(typeof(List<Student>), new[] {typeof(Student), typeof(Bachelor), typeof(Master)});
            using var stream = new FileStream(Path.Combine(Folder, Filename), FileMode.Open);
            return (List<Student>)serializer.Deserialize(stream);
        }
    }
}
