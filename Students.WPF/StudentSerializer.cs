using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Students.WPF
{
    public class StudentSerializer
    {
        private const string Path = @"C:\Projects\Students";
        private const string Filename = "students.xml";
        
        public void Serialize(Student student)
        {
            var serializer = new XmlSerializer(typeof(Student));
            using var stream = new FileStream(System.IO.Path.Combine(Path, Filename), FileMode.Create);
            serializer.Serialize(stream, student);
        }

        public void Serialize(List<Student> students)
        {
            var serializer = new XmlSerializer(typeof(List<Student>));
            using var stream = new FileStream(System.IO.Path.Combine(Path, Filename), FileMode.Create);
            serializer.Serialize(stream, students);
        }
    }
}
