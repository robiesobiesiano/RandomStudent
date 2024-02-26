using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Losowo.Models
{
    public partial class ClassModel : ObservableObject
    {
        [ObservableProperty]
        public string className;

        [ObservableProperty]
        public ObservableCollection<StudentModel> studentList;

        public ClassModel(string className)
        {
            ClassName = className;
            StudentList = new ObservableCollection<StudentModel>();
        }
    }
}
