using System;

namespace Students
{
    [Serializable]
    public class Master : Student
    {
        public Degree Degree { get; set; }
    }
}
