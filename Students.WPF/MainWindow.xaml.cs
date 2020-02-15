using System;
using System.Windows;
using System.Windows.Controls;
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
            UpdateNavButtonsState();
            UpdateRemoveButtonState();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            Iterator.New();
            UpdateNavButtonsState();
            UpdateRemoveButtonState();
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
            UpdateNavButtonsState();
            UpdateRemoveButtonState();
        }

        private void Next_OnClick(object sender, RoutedEventArgs e)
        {
            Iterator.Next();
            UpdateNavButtonsState();
        }

        private void Previous_OnClick(object sender, RoutedEventArgs e)
        {
            Iterator.Previous();
            UpdateNavButtonsState();
        }

        private void UpdateNavButtonsState()
        {
            NextStudentButton.IsEnabled = Iterator.CanSelectNext;
            NextStudentMenuButton.IsEnabled = Iterator.CanSelectNext;
            PreviousStudentButton.IsEnabled = Iterator.CanSelectPrevious;
            PreviousStudentMenuButton.IsEnabled = Iterator.CanSelectPrevious;
        }

        private void Remove_OnClick(object sender, RoutedEventArgs e)
        {
            Iterator.Remove();
            UpdateNavButtonsState();
            UpdateRemoveButtonState();
        }

        private void UpdateRemoveButtonState()
        {
            RemoveMenuButton.IsEnabled = Iterator.IsSelected;
        }

        private void SearchConditionBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var query = SearchConditionBox.Text.Trim();
            if (query.Length == 0)
            {
                Iterator.ClearFilter();
            }
            else
            {
                var pred = FilterComboBox.SelectedIndex switch
                {
                    0 => new Predicate<Student>(s => s.FirstName == query),
                    1 => new Predicate<Student>(s => s.SecondName == query),
                    2 => new Predicate<Student>(s => s.Faculty == query),
                    _ => throw new InvalidOperationException()
                };
                Iterator.ApplyFilter(pred);
            }
            UpdateNavButtonsState();
            UpdateRemoveButtonState();
        }
    }
}
