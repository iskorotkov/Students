using System;
using System.Collections.Generic;
using System.Linq;

namespace Students.WPF
{
    public class StudentsIterator
    {
        private List<Student> _students = new List<Student>();
        private List<Student>? UnfilteredStudents { get; set; }

        public List<Student> Students
        {
            get => _students;
            set
            {
                _students = value;
                SelectedStudent = _students.Count > 0 ? _students[0] : null;
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

        public bool IsSelected => SelectedStudent != null;

        private Student? _selectedStudent;
        public Student? SelectedStudent
        {
            get => _selectedStudent;
            private set
            {
                _selectedStudent = value;
                if (value != null)
                    StudentSelected?.Invoke(_selectedStudent);
                else
                    NoStudentSelected?.Invoke();
            }
        }

        public bool CanSelectNext => SelectedStudent != null && !ReferenceEquals(SelectedStudent, Students.Last());
        public bool CanSelectPrevious => SelectedStudent != null && !ReferenceEquals(SelectedStudent, Students.First());

        public event Action<Student>? StudentSelected;
        public event Action? NoStudentSelected;

        public void New()
        {
            var s = new Student();
            Students.Add(s);
            SelectedStudent = Students.Last();
        }

        public void Next()
        {
            if (SelectedStudent == null)
                return;
            if (ReferenceEquals(SelectedStudent, Students.Last()))
                return;
            var index = Students.IndexOf(SelectedStudent);
            SelectedStudent = Students[index + 1];
        }

        public void Remove()
        {
            if (SelectedStudent == null)
                return;
            var index = Students.IndexOf(SelectedStudent);
            Students.Remove(SelectedStudent);
            UnfilteredStudents?.Remove(SelectedStudent);
            if (Students.Count == 0)
                SelectedStudent = null;
            else
                SelectedStudent = index >= Students.Count ? Students.Last() : Students[index];
        }

        public void Previous()
        {
            if (SelectedStudent == null)
                return;
            if (ReferenceEquals(SelectedStudent, Students.First()))
                return;
            var index = Students.IndexOf(SelectedStudent);
            SelectedStudent = Students[index - 1];
        }

        public void Clear()
        {
            Students.Clear();
            UnfilteredStudents = null;
            SelectedStudent = null;
        }
    }
}
