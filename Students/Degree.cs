using System;

namespace Students
{
    [Serializable]
    public class Degree
    {
        public DateTime? GraduationDate { get; set; }
        public string Domain { get; set; }
    }
}
