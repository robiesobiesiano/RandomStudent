using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace Losowo.Models
{


    public partial class StudentModel : ObservableObject
    {
        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string secondName;

        
        [ObservableProperty]
        public string asignedClass;

        public StudentModel(string name, string secondName, string asignedClass)
        {
            Name = name;
            SecondName = secondName;
            AsignedClass = asignedClass;
        }
    }
}

