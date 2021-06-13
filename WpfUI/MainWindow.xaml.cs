using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataLibrary;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            List<Employee> listOfEmployees = DataAccess.LoadEmployees();

            ObservableCollection<Employee> collectionOfEmployees = new ObservableCollection<Employee>(listOfEmployees);

            InitializeComponent();

            this.employeesGrid.ItemsSource = collectionOfEmployees;

        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Employee> searchQuery = new ObservableCollection<Employee>();
            foreach (Employee person in employeesGrid.ItemsSource)
            {
                if (person.DepartmentName.Contains(searchPromptTextbox.Text))
                {
                    searchQuery.Add(person);
                }
            }
            employeesGrid.ItemsSource = searchQuery;
            saveButton.IsEnabled = false;
        }

        private void searchCancelButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Employee> searchQuery = new ObservableCollection<Employee>(DataAccess.LoadEmployees());
            employeesGrid.ItemsSource = searchQuery;
            saveButton.IsEnabled = true;
        }

        private void employeesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {                     
                Employee selectedEmployee = (Employee)employeesGrid.SelectedItem;

                infoLabel.Content = "Название отдела: " + selectedEmployee.DepartmentName + ";";

                if ((selectedEmployee.Position == "Рабочий") || (selectedEmployee.Position == "Контролёр"))
                {
                    infoLabel.Content += " Имя начальника: ";

                    foreach (Employee emp in DataAccess.LoadEmployees())
                    {
                        if ((emp.DepartmentName == selectedEmployee.DepartmentName) && (emp.Position == "Глава"))
                        {
                            infoLabel.Content += emp.Name;
                            break;
                        }
                    }
                }                 
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            File.Delete("Employees.xml");
            File.Create("Employees.xml").Close();

            List<Employee> newList = new List<Employee>();
            foreach (var item in employeesGrid.ItemsSource)
            {
                newList.Add((Employee)item);
            }
            
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Employee>));
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("Employees.xml"))
            {
                writer.Serialize(file, newList);
            }
        }

        private void newWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Employee> currentGrid = (ObservableCollection<Employee>)employeesGrid.ItemsSource;

            if ((nameBox.Text!= "")&&(dobBox.Text!="")&&(genderBox.Text!="")&&(positionBox.Text!="")&&(departmentBox.Text!=""))
            {
                currentGrid.Add(new Employee { DateOfBirth = dobBox.Text, Name = nameBox.Text, Gender = genderBox.Text, Position = positionBox.Text, DepartmentName = departmentBox.Text });
            }
            employeesGrid.ItemsSource = currentGrid;
        }
    }
}
