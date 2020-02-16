using System;

namespace Students
{
    [Serializable]
    public class Bachelor : Student
    {
        public Master MakeMaster()
        {
            return new Master
            {
                FirstName = FirstName,
                SecondName = SecondName,
                Faculty = Faculty,
                Degree = new Degree
                {
                    Domain = Faculty,
                    Year = DateTime.Today.Year
                }
            };
        }
    }
}
