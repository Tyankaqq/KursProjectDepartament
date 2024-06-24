using KursProjectDepartament.Model;
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
using System.Windows.Shapes;

namespace KursProjectDepartament.View
{
    /// <summary>
    /// Логика взаимодействия для AddEditHuman.xaml
    /// </summary>
    public partial class AddEditHuman : Page
    {
        public string Surname { get; private set; }
        public string Name { get; private set; }
        public string MiddleName { get; private set; }
        public string Address { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string MaritalStatus { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string HireDate { get; private set; }
        public string Gender { get; private set; }
        public int ChildrenCount { get; private set; }

        private readonly HumanDepartmentDbContext dbContext = new HumanDepartmentDbContext();

        public AddEditHuman()
        {
            InitializeComponent();
            dbContext = new HumanDepartmentDbContext();
            LoadDepartments();
        }
        private void LoadDepartments()
        {
            var departments = dbContext.Departments.ToList();
            DepartmentComboBox.ItemsSource = departments;

        }

        public async void Button_Click(object sender, RoutedEventArgs e)
        {
            string surname = SurnameTextBox.Text;
            string name = NameTextBox.Text;
            string middleName = MiddleNameTextBox.Text;
            string address = AddressTextBox.Text;
            DateTime birthDate = BirthDayDatePicker.SelectedDate ?? DateTime.MinValue;
            string maritalStatus = MaritalStatusTextBox.Text;
            string phoneNumber = PhoneNumberTextBox.Text;
            string email = EmailTextBox.Text;
            string hireDate = HireDateTextBox.Text;
            string gender = GenderTextBox.Text;
            int childrenCount = Convert.ToInt32(ChildrenTextBox.Text);
            string position = PositionTextBox.Text;
            var selectedDepartment = (Department)DepartmentComboBox.SelectedItem;
            if (selectedDepartment != null)
            {
                // Создаем нового сотрудника и сохраняем его в базе данных
                var newEmployee = new Employee
                {
                    LastName = surname,
                    FirstName = name,
                    MiddleName = middleName,
                    Address = address,
                    BirthDate = birthDate.ToString(),
                    MaritalStatus = maritalStatus,
                    PhoneNumber = phoneNumber,
                    Email = email,
                    HireDate = DateTime.Parse(hireDate).ToString(),
                    Gender = gender,
                    children = childrenCount,
                    Department = selectedDepartment
                };

                dbContext.Employees.Add(newEmployee);
                dbContext.SaveChanges();

            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите отдел.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            MainWindow.frame.Navigate(new HumanPage());

            MessageBox.Show("Данные сохранены успешно!");
        }
    }
}
