using KursProjectDepartament.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Xml.Linq;

namespace KursProjectDepartament.View
{
    /// <summary>
    /// Логика взаимодействия для HumanPage.xaml
    /// </summary>
    public partial class HumanPage : Page
    {
        public HumanPage()
        {
            InitializeComponent();
            LoadEmployeeCards();
        }
        private void LoadEmployeeCards()
        {
            using (var dbContext = new HumanDepartmentDbContext())
            {
                var employee = dbContext.Employees.FirstOrDefault();
                if (employee != null)
                {
                    FIO.Text = $"{employee.LastName} {employee.FirstName} {employee.MiddleName}";
                    Position.Text = employee.Position;
                    Phone.Text = $"Телефон: {employee.PhoneNumber}";
                    Children.Text = $"Количество Детей: {employee.Children}";
                    Marital_status.Text = $"Семейное положение: {employee.MaritalStatus}";
                    BirthDate.Text = $"Дата рождения: {employee.BirthDate.ToShortDateString()}";
                    Gender.Text = $"Пол: {employee.Gender}";
                    Adress.Text = $"Адрес проживания: {employee.Address}";
                    hire_date.Text = $"Дата найма: {employee.HireDate.ToShortDateString()}";
                    Email.Text = $"Email: {employee.Email}";
                }

            }
        }
    }
}
