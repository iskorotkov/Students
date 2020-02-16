using System;
using System.Xml.Serialization;

namespace Students
{
    [Serializable, XmlInclude(typeof(Bachelor)), XmlInclude(typeof(Master))]
    public class Student
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Faculty { get; set; }
    }
}
