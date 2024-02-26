using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Losowo.Models;


namespace Losowo.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string newClassName;

        [ObservableProperty]
        private string newStudentName;

        [ObservableProperty]
        private string newStudentSecondName;

        [ObservableProperty]
        private ClassModel selectedClass;

        [ObservableProperty]
        private StudentModel selectedStudent;

        [ObservableProperty]
        private StudentModel randomStudent;

        public ObservableCollection<ClassModel> Classes { get; set; } = new ObservableCollection<ClassModel>();

        public MainViewModel()
        {
            LoadClassesAndStudents();
        }

        [RelayCommand]
        public async Task AddClass()
        {
            if (!string.IsNullOrWhiteSpace(NewClassName))
            {
                Classes.Add(new ClassModel(NewClassName));
                NewClassName = string.Empty;
                await SaveClasses();
            }
        }

        [RelayCommand]
        public async Task AddStudent()
        {
            if (SelectedClass != null && !string.IsNullOrWhiteSpace(NewStudentName) && !string.IsNullOrWhiteSpace(NewStudentSecondName))
            {

                SelectedClass.StudentList.Add(new StudentModel(NewStudentName, NewStudentSecondName, SelectedClass.ClassName));
                NewStudentName = string.Empty;
                NewStudentSecondName = string.Empty;
                await SaveClasses();
            }
        }


        [RelayCommand]
        public void RandomizeStudent()
        {
            if (SelectedClass != null && SelectedClass.StudentList.Any())
            {
                var random = new Random();
                var students = SelectedClass.StudentList;
                RandomStudent = students[random.Next(students.Count)];
            }
        }


        private async Task SaveClasses()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "classes_with_students.txt");
            using var writer = new StreamWriter(filePath, false);
            foreach (var classModel in Classes)
            {

                await writer.WriteLineAsync($"Klasa: {classModel.ClassName}");

                foreach (var student in classModel.StudentList)
                {
                    await writer.WriteLineAsync($"\tUczeń: {student.Name} {student.SecondName}, Przypisana klasa: {classModel.ClassName}");
                }
            }


        }

        private async Task LoadClassesAndStudents()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "classes_with_students.txt");
            if (File.Exists(filePath))
            {
                using var reader = new StreamReader(filePath);
                string line;
                ClassModel currentClass = null;

                while ((line = await reader.ReadLineAsync()) != null)
                {

                    if (line.StartsWith("Klasa: "))
                    {
                        var className = line.Substring("Klasa: ".Length);
                        currentClass = new ClassModel(className);
                        Classes.Add(currentClass);
                    }

                    else if (line.Trim().StartsWith("Uczeń: ") && currentClass != null)
                    {
                        var studentInfo = line.Trim().Substring("Uczeń: ".Length);
                        var studentDetails = studentInfo.Split(',');
                        if (studentDetails.Length == 2)
                        {
                            var nameParts = studentDetails[0].Trim().Split(' ');
                            if (nameParts.Length >= 2)
                            {
                                var name = nameParts[0];
                                var secondName = string.Join(" ", nameParts, 1, nameParts.Length - 1);
                                currentClass.StudentList.Add(new StudentModel(name, secondName, currentClass.ClassName));
                            }
                        }
                    }
                }
            }

        }
        [RelayCommand]
        public async Task EditStudent(StudentModel student)
        {
            if (student == null) return;

            // Znajdź klasę, do której należy uczeń
            var studentClass = Classes.FirstOrDefault(c => c.ClassName == student.AsignedClass);
            if (studentClass != null)
            {
                // Znajdź ucznia w tej klasie
                var existingStudent = studentClass.StudentList.FirstOrDefault(s => s.Name == student.Name && s.SecondName == student.SecondName);
                if (existingStudent != null)
                {
                    // Zaktualizuj dane ucznia
                    existingStudent.Name = student.Name; // Załóżmy, że to są nowe, zaktualizowane dane
                    existingStudent.SecondName = student.SecondName; // Załóżmy, że to są nowe, zaktualizowane dane
                }
            }

            await SaveClasses();
        }
        [RelayCommand]
        public async Task DeleteStudent(StudentModel student)
        {
            if (student == null) return;

            // Znajdź klasę, do której należy uczeń
            var studentClass = Classes.FirstOrDefault(c => c.ClassName == student.AsignedClass);
            if (studentClass != null)
            {
                // Usuń ucznia z listy
                var existingStudent = studentClass.StudentList.FirstOrDefault(s => s.Name == student.Name && s.SecondName == student.SecondName);
                if (existingStudent != null)
                {
                    studentClass.StudentList.Remove(existingStudent);
                }
            }

            await SaveClasses();
        }
    }
}

