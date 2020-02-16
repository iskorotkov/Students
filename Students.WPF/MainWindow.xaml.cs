using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            Iterator.StudentSelected += _ => SetFormEnabled(true);
            Iterator.StudentSelected += UpdateFormFields;
            Iterator.StudentSelected += SetBachelorFormState;
            Iterator.StudentSelected += SetMasterFormState;

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
            SearchConditionBox.Clear();
        }

        private void AddBachelor_OnClick(object sender, RoutedEventArgs e)
        {
            Iterator.Add(new Bachelor());
            UpdateNavButtonsState();
            UpdateRemoveButtonState();
        }

        private void AddMaster_OnClick(object sender, RoutedEventArgs e)
        {
            Iterator.Add(new Master());
            UpdateNavButtonsState();
            UpdateRemoveButtonState();
        }

        private void UpdateStudentFaculty(object sender, TextChangedEventArgs e)
        {
            if (Iterator.SelectedStudent != null)
                Iterator.SelectedStudent.Faculty = FacultyBox.Text;
        }

        private void UpdateStudentSecondName(object sender, TextChangedEventArgs e)
        {
            if (Iterator.SelectedStudent != null)
                Iterator.SelectedStudent.SecondName = SecondNameBox.Text;
        }

        private void UpdateStudentFirstName(object sender, TextChangedEventArgs e)
        {
            if (Iterator.SelectedStudent != null)
                Iterator.SelectedStudent.FirstName = FirstNameBox.Text;
        }

        private void SetFormEnabled(bool enabled)
        {
            FirstNameBox.IsEnabled = enabled;
            SecondNameBox.IsEnabled = enabled;
            FacultyBox.IsEnabled = enabled;
        }

        private void ClearFormFields()
        {
            FirstNameBox.Clear();
            SecondNameBox.Clear();
            FacultyBox.Clear();
            DegreeYear.Clear();
            DegreeDomain.Clear();
            DegreeForm.Visibility = Visibility.Collapsed;
            MakeMasterButton.Visibility = Visibility.Collapsed;
        }

        private void UpdateFormFields(Student student)
        {
            FirstNameBox.Text = student.FirstName;
            SecondNameBox.Text = student.SecondName;
            FacultyBox.Text = student.Faculty;
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
            SearchConditionBox.Clear();
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

        private void SearchCondition_OnChanged(object sender, EventArgs e)
        {
            if (SearchConditionBox == null)
                return;
            var query = SearchConditionBox.Text.Trim();
            if (query.Length == 0)
            {
                Iterator.ClearFilter();
            }
            else
            {
                query = query.ToLower();
                var pred = FilterComboBox.SelectedIndex switch
                {
                    0 => new Predicate<Student>(s => s.FirstName.ToLower().Contains(query)),
                    1 => new Predicate<Student>(s => s.SecondName.ToLower().Contains(query)),
                    2 => new Predicate<Student>(s => s.Faculty.ToLower().Contains(query)),
                    _ => throw new InvalidOperationException()
                };
                Iterator.ApplyFilter(pred);
            }

            UpdateNavButtonsState();
            UpdateRemoveButtonState();
        }

        private void MakeMasterButton_OnClick(object sender, RoutedEventArgs e)
        {
            var bachelor = (Bachelor) Iterator.SelectedStudent;
            Iterator.ReplaceSelected(bachelor.MakeMaster());
        }

        private void DegreeYear_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Iterator.SelectedStudent is Master master)
                master.Degree.Year = int.Parse(DegreeYear.Text);
        }

        private void DegreeDomain_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Iterator.SelectedStudent is Master master)
                master.Degree.Domain = DegreeDomain.Text;
        }

        private void SetBachelorFormState(Student student)
        {
            MakeMasterButton.Visibility = student is Bachelor
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void SetMasterFormState(Student student)
        {
            var master = student as Master;
            DegreeForm.Visibility = master != null
                ? Visibility.Visible
                : Visibility.Collapsed;

            DegreeYear.Text = master?.Degree.Year.ToString() ?? "";
            DegreeDomain.Text = master?.Degree.Domain ?? "";
        }

        private void DegreeYear_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = int.TryParse(DegreeYear.Text, out _);
        }
    }
}
