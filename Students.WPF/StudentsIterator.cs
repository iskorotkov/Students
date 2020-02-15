using System;
using System.Collections.Generic;
using System.Linq;

namespace Students.WPF
{
    public class StudentsIterator
    {
        private int? _index;
        private List<Student> _students = new List<Student>();

        public List<Student> Students
        {
            get => _students;
            set
            {
                _students = value;
                Index = _students.Any() ? 0 : (int?) null;
            }
        } 

        public bool IsSelected => Index != null;
        public Student? Selected
        {
            get
            {
                if (!Index.HasValue || !Students.Any())
                    return null;
                return Students[Index.Value];
            }
        }

        private int? Index
        {
            get => _index;
            set
            {
                _index = value;
                if (value != null)
                    StudentSelected?.Invoke(Selected);
                else
                    NoStudentSelected?.Invoke();
            }
        }

        public event Action<Student> StudentSelected;
        public event Action NoStudentSelected;

        public void New()
        {
            var s = new Student();
            Students.Add(s);
            Index = Students.Count - 1;
        }

        public void Next()
        {
            if (Index >= Students.Count - 1)
                return;
            if (!Index.HasValue)
                return;
            Index++;
        }

        public void Previous()
        {
            if (Index <= 0)
                return;
            if (!Index.HasValue)
                return;
            Index--;
        }

        public void Clear()
        {
            Students.Clear();
            Index = null;
        }
    }
}
