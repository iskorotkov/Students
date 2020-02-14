using System.IO;
using System.Xml.Serialization;

namespace Students.CommandLine
{
    static class Program
    {
        private static void Main()
        {
            const string path = @"C:\Projects\Students";
            const string filename = "students.xml";

            var s1 = new Student {FirstName = "Ivan", SecondName = "Korotkov", Faculty = "Math"};
            var serializer = new XmlSerializer(typeof(Student));
            using (var stream = new FileStream(Path.Combine(path, filename), FileMode.Create))
            {
                serializer.Serialize(stream, s1);
            }
        }
    }
}
