using KursProjectDepartament.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace KursProjectDepartament.View
{
    /// <summary>
    /// Interaction logic for EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditEmployeeWindow : Window
    {
        private Employee _employee;

        public EditEmployeeWindow(Employee employee)
        {
            InitializeComponent();
            _employee = employee;
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            // Загрузка данных сотрудника в элементы управления
            LastNameTextBox.Text = _employee.LastName;
            FirstNameTextBox.Text = _employee.FirstName;
            MiddleNameTextBox.Text = _employee.MiddleName;
            PositionTextBox.Text = _employee.Position;
            PhoneNumberTextBox.Text = _employee.PhoneNumber;
            MaritalStatusTextBox.Text = _employee.MaritalStatus;
            ChildrenTextBox.Text = _employee.Children.ToString();


            BirthDateDatePicker.SelectedDate = DateTime.Parse(_employee.BirthDate);


            AddressTextBox.Text = _employee.Address;
            HireDateDatePicker.SelectedDate = DateTime.Parse(_employee.HireDate);
            EmailTextBox.Text = _employee.Email;
            
        }

      

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
                // Применяем изменения из элементов управления к объекту сотрудника
                _employee.LastName = LastNameTextBox.Text;
                _employee.FirstName = FirstNameTextBox.Text;
                _employee.MiddleName = MiddleNameTextBox.Text;
                _employee.Position = PositionTextBox.Text;
                _employee.BirthDate = BirthDateDatePicker.ToString();
                _employee.PhoneNumber = PhoneNumberTextBox.Text;

                

                // Сохраняем изменения в базе данных или другом хранилище
                using (var dbContext = new HumanDepartmentDbContext())
                {
                    dbContext.Employees.Update(_employee);
                    dbContext.SaveChanges();
                }

                // Закрываем окно редактирования
                this.Close();
            }
        }
    }


