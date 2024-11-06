﻿using System.Collections.ObjectModel;
using System.ComponentModel;

namespace syncfusion.gridcontroldemos.wpf
{
    public class EmployeesSource
    {
        private ObservableCollection<Employee> employees;

        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
        }

        public EmployeesSource()
        {
            employees = new ObservableCollection<Employee>();
            for (int n = 0; n < 50; n++)
            {
                employees.Add(new Employee("Matt", "Program Manager"));
                employees.Add(new Employee("Joan", "Developer"));
                employees.Add(new Employee("Mark", "Programming Writer"));
                employees.Add(new Employee("Mary", "Test Lead"));
                employees.Add(new Employee("Karen", "Developer"));
                employees.Add(new Employee("George", "Programming Writer"));
                employees.Add(new Employee("Peter", "Program Manager"));
            }
        }
    }
    public class Employee : INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }


        public Employee(string name, string title)
        {
            this.name = name;
            this.title = title;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
