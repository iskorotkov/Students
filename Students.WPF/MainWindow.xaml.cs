using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Students.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Student> _students = new List<Student>();
        private Student _selectedStudent;
        private StudentSerializer _serializer = new StudentSerializer();
        
        public MainWindow()
        {
            InitializeComponent();
            FirstNameBox.TextChanged += UpdateFirstName;
            SecondNameBox.TextChanged += UpdateSecondName;
            FacultyBox.TextChanged += UpdateFaculty;
            SetFormEnabled(false);
        }

        private void NewList_OnClick(object sender, RoutedEventArgs e)
        {
            _students.Clear();
            SetFormEnabled(false);
            ClearForm();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            _selectedStudent = new Student();
            _students.Add(_selectedStudent);
            SetFormEnabled(true);
            UpdateForm();
        }

        private void UpdateFaculty(object sender, TextChangedEventArgs e)
        {
            _selectedStudent.Faculty = FacultyBox.Text;
        }

        private void UpdateSecondName(object sender, TextChangedEventArgs e)
        {
            _selectedStudent.SecondName = SecondNameBox.Text;
        }

        private void UpdateFirstName(object sender, TextChangedEventArgs e)
        {
            _selectedStudent.FirstName = FirstNameBox.Text;
        }

        private void SetFormEnabled(bool enabled)
        {
            FirstNameBox.IsEnabled = enabled;
            SecondNameBox.IsEnabled = enabled;
            FacultyBox.IsEnabled = enabled;
        }

        private void ClearForm()
        {
            FirstNameBox.Text = string.Empty;
            SecondNameBox.Text = string.Empty;
            FacultyBox.Text = string.Empty;
        }

        private void UpdateForm()
        {
            FirstNameBox.Text = _selectedStudent.FirstName;
            SecondNameBox.Text = _selectedStudent.SecondName;
            FacultyBox.Text = _selectedStudent.Faculty;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            _serializer.Serialize(_students);
        }
    }
}
