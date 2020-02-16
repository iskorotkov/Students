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

        public void Add(Student student)
        {
            Students.Add(student);
            SelectedStudent = Students.Last();
        }

        public void Next()
        {
            if (SelectedStudent == null)
                return;
            if (ReferenceEquals(SelectedStudent, Students.Last()))
                throw new InvalidOperationException("Can't go past last student in list");
            var index = Students.IndexOf(SelectedStudent);
            SelectedStudent = Students[index + 1];
        }

        public void Remove()
        {
            if (SelectedStudent == null)
                throw new InvalidOperationException("Can't delete users if it wasn't selected");
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
                throw new InvalidOperationException("Can't go before first student in list");
            var index = Students.IndexOf(SelectedStudent);
            SelectedStudent = Students[index - 1];
        }

        public void Clear()
        {
            Students.Clear();
            UnfilteredStudents = null;
            SelectedStudent = null;
        }

        public void ReplaceSelected(Student newStudent)
        {
            var index = Students.IndexOf(SelectedStudent);
            Students[index] = newStudent;
            SelectedStudent = newStudent;
        }
    }
}
