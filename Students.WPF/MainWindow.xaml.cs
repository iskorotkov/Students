using System.Windows;
using Microsoft.Win32;

namespace Students.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FirstNameBox.TextChanged += (sender, args) => UpdateStudentFirstName();
            SecondNameBox.TextChanged += (sender, args) => UpdateStudentSecondName();
            FacultyBox.TextChanged += (sender, args) => UpdateStudentFaculty();

            Iterator.StudentSelected += s => SetFormEnabled(true);
            Iterator.StudentSelected += s => UpdateFormFields();

            Iterator.NoStudentSelected += ClearFormFields;
            Iterator.NoStudentSelected += () => SetFormEnabled(false);
        }

        private StudentsSerializer Serializer { get; } = new StudentsSerializer();
        private StudentsIterator Iterator { get; } = new StudentsIterator();

        private void NewList_OnClick(object sender, RoutedEventArgs e)
        {
            Iterator.Clear();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            Iterator.New();
        }

        private void UpdateStudentFaculty()
        {
            if (Iterator.IsSelected)
                Iterator.Selected.Faculty = FacultyBox.Text;
        }

        private void UpdateStudentSecondName()
        {
            if (Iterator.IsSelected)
                Iterator.Selected.SecondName = SecondNameBox.Text;
        }

        private void UpdateStudentFirstName()
        {
            if (Iterator.IsSelected)
                Iterator.Selected.FirstName = FirstNameBox.Text;
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
            FirstNameBox.Text = Iterator.Selected.FirstName;
            SecondNameBox.Text = Iterator.Selected.SecondName;
            FacultyBox.Text = Iterator.Selected.Faculty;
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
            Serializer.Serialize(file, Iterator.Students);
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
            Iterator.Students = Serializer.Deserialize(file);
        }

        private void Next_OnClick(object sender, RoutedEventArgs e)
        {
            Iterator.Next();
        }

        private void Previous_OnClick(object sender, RoutedEventArgs e)
        {
            Iterator.Previous();
        }
    }
}
