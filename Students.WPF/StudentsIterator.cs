using System;
using System.Collections.Generic;
using System.Linq;

namespace Students.WPF
{
    public class StudentsIterator
    {
        private int? _index;
        private List<Student> _students = new List<Student>();

        private List<Student>? UnfilteredStudents { get; set; }

        public List<Student> Students
        {
            get => _students;
            set
            {
                _students = value;
                Index = _students.Any() ? 0 : (int?)null;
            }
        }

        public void ApplyFilter(Predicate<Student> predicate)
        {
            UnfilteredStudents ??= Students;
            Students = UnfilteredStudents.Where(s => predicate(s)).ToList();
        }

        public void ClearFilter()
        {
            Students = UnfilteredStudents ?? Students;
            UnfilteredStudents = null;
        }

        public bool IsSelected => Index != null;

        public Student? Selected => !Index.HasValue ? null : Students[Index.Value];

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

        public bool CanSelectNext => Index != null && Index.Value < Students.Count - 1;
        public bool CanSelectPrevious => Index != null && Index.Value > 0;

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
            if (!Index.HasValue)
                return;
            if (Index >= Students.Count - 1)
                return;
            Index++;
        }

        public void Remove()
        {
            if (!Index.HasValue)
                return;
            Students.RemoveAt(Index.Value);
            if (!Students.Any())
                Index = null;
            else
                Index = Index >= Students.Count ? Students.Count - 1 : Index;
        }

        public void Previous()
        {
            if (!Index.HasValue)
                return;
            if (Index <= 0)
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
