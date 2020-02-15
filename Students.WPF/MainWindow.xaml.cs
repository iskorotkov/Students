using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace Students.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Student _selectedStudent;

        public MainWindow()
        {
            InitializeComponent();
            FirstNameBox.TextChanged += (sender, args) => UpdateStudentFirstName();
            SecondNameBox.TextChanged += (sender, args) => UpdateStudentSecondName();
            FacultyBox.TextChanged += (sender, args) => UpdateStudentFaculty();

            StudentSelected += s => SetFormEnabled(true);
            StudentSelected += s => UpdateFormFields();

            NoStudentsSelected += ClearFormFields;
            NoStudentsSelected += () => SetFormEnabled(false);

            SelectedStudent = null;
        }

        private StudentsSerializer Serializer { get; } = new StudentsSerializer();
        private List<Student> Students { get; set; } = new List<Student>();

        private Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                if (value == null)
                    NoStudentsSelected?.Invoke();
                else
                    StudentSelected?.Invoke(value);
            }
        }

        private int? SelectedStudentIndex { get; set; } = null;

        public event Action<Student> StudentSelected;
        public event Action NoStudentsSelected;

        private void NewList_OnClick(object sender, RoutedEventArgs e)
        {
            Students.Clear();
            SelectedStudent = null;
            SelectedStudentIndex = null;
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedStudent = new Student();
            Students.Add(SelectedStudent);
            SelectedStudentIndex = Students.Count - 1;
        }

        private void UpdateStudentFaculty()
        {
            if (SelectedStudent == null)
                return;
            SelectedStudent.Faculty = FacultyBox.Text;
        }

        private void UpdateStudentSecondName()
        {
            if (SelectedStudent == null)
                return;
            SelectedStudent.SecondName = SecondNameBox.Text;
        }

        private void UpdateStudentFirstName()
        {
            if (SelectedStudent == null)
                return;
            SelectedStudent.FirstName = FirstNameBox.Text;
        }

        private void SetFormEnabled(bool enabled)
        {
            FirstNameBox.IsEnabled = enabled;
            SecondNameBox.IsEnabled = enabled;
            FacultyBox.IsEnabled = enabled;
        }

        private void ClearFormFields()
        {
            FirstNameBox.Text = string.Empty;
            SecondNameBox.Text = string.Empty;
            FacultyBox.Text = string.Empty;
        }

        private void UpdateFormFields()
        {
            FirstNameBox.Text = SelectedStudent.FirstName;
            SecondNameBox.Text = SelectedStudent.SecondName;
            FacultyBox.Text = SelectedStudent.Faculty;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
            };
            var result = dialog.ShowDialog();
            if (result == null || !result.Value)
                return;
            var file = dialog.FileName;
            Serializer.Serialize(file, Students);
        }

        private void Load_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
            };
            var result = dialog.ShowDialog();
            if (result == null || !result.Value)
                return;
            var file = dialog.FileName;
            Students = Serializer.Deserialize(file);
            if (!Students.Any())
                return;
            SelectedStudent = Students[0];
            SelectedStudentIndex = 0;
        }

        private void Next_OnClick(object sender, RoutedEventArgs e)
        {
            if (!SelectedStudentIndex.HasValue) return;
            if (SelectedStudentIndex >= Students.Count - 1) return;
            SelectedStudentIndex++;
            SelectedStudent = Students[SelectedStudentIndex.Value];
        }

        private void Previous_OnClick(object sender, RoutedEventArgs e)
        {
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (!SelectedStudentIndex.HasValue) return;
            if (SelectedStudentIndex == 0) return;
            SelectedStudentIndex--;
            SelectedStudent = Students[SelectedStudentIndex.Value];
        }
    }
}
