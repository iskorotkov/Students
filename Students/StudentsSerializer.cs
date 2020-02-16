using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Students
{
    public class StudentsSerializer
    {
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<Student>),
            new[]
            {
                typeof(Student),
                typeof(Bachelor),
                typeof(Master)
            });

        public void Serialize(string filepath, List<Student> students)
        {
            using var stream = new FileStream(filepath, FileMode.Create);
            _serializer.Serialize(stream, students);
        }

        public List<Student> Deserialize(string filepath)
        {
            using var stream = new FileStream(filepath, FileMode.Open);
            return (List<Student>) _serializer.Deserialize(stream);
        }
    }
}
