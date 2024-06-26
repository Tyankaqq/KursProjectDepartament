using KursProjectDepartament.Model;
using Microsoft.EntityFrameworkCore;
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
            LoadEmployeeDetails();
            LoadDepartments();

        }
        private void LoadDepartments()
        {
            using (var dbContext = new HumanDepartmentDbContext())
            {
                var departments = dbContext.Departments.ToList();
                DepartmentComboBox.ItemsSource = departments;
            }
        }
        private void LoadEmployeeDetails()
        {
            using (var dbContext = new HumanDepartmentDbContext())
            {
                var employees = dbContext.Employees.ToList();
                foreach (var employee in employees)
                {
                    var card = CreateEmployeeCard(employee);
                    EmployeeWrapPanel.Children.Add(card);
                }
            }
        }

        private Border CreateEmployeeCard(Employee employee)
        {
            var card = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                CornerRadius = new CornerRadius(5),
                Background = Brushes.LightGray,
                Child = new StackPanel
                {
                    Children =
                    {

                        new TextBlock { Text = $"{employee.LastName} {employee.FirstName} {employee.MiddleName}", FontSize = 16, FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 0, 0, 5) },
                        new TextBlock { Text = employee.Position, FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 0, 0, 5) },
                        new TextBlock { Text = $"Телефон: {employee.PhoneNumber}", FontSize = 12, Margin = new Thickness(0, 0, 0, 5) },
                        new TextBlock { Text = $"Количество Детей: {employee.Children}", FontSize = 12, Margin = new Thickness(0, 0, 0, 5) },
                        new TextBlock { Text = $"Семейное положение: {employee.MaritalStatus}", FontSize = 12, Margin = new Thickness(0, 0, 0, 5) },
                        new TextBlock { Text = $"Дата рождения: {employee.BirthDate}", FontSize = 12, Margin = new Thickness(0, 0, 0, 5) },
                        new TextBlock { Text = $"Пол: {employee.Gender}", FontSize = 12, Margin = new Thickness(0, 0, 0, 5) },
                        new TextBlock { Text = $"Адрес проживания: {employee.Address}", FontSize = 12, Margin = new Thickness(0, 0, 0, 5) },
                        new TextBlock { Text = $"Дата найма: {employee.HireDate}", FontSize = 12, Margin = new Thickness(0, 0, 0, 5) },
                        new TextBlock { Text = $"Email: {employee.Email}", FontSize = 12, Margin = new Thickness(0, 0, 0, 5) },
                    }
                }
            };
            var contextMenu = new ContextMenu();

            var editMenuItem = new MenuItem { Header = "Редактировать" };
            editMenuItem.Click += (s, e) => EditEmployee(employee);

            var deleteMenuItem = new MenuItem { Header = "Удалить" };
            deleteMenuItem.Click += (s, e) => DeleteEmployee(employee);

            contextMenu.Items.Add(editMenuItem);
            contextMenu.Items.Add(deleteMenuItem);

            card.ContextMenu = contextMenu;

            return card;


        }
        private void EmployeesWithoutHigherEducation_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new HumanDepartmentDbContext())
            {
                var educations = dbContext.GetEmployeesWithoutHigherEducation();
                RefreshEmployeeCards(educations);
            }
        }

        private void EmployeesWithMismatchedEducation_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new HumanDepartmentDbContext())
            {
                var employees = dbContext.GetEducationsWithMismatchedFieldOfStudy();
                RefreshEmployeeCards(employees);
            }
        }
        private void CreateOrderCards(List<Order> orders)
        {
            EmployeeWrapPanel.Children.Clear();

            foreach (var order in orders)
            {

                var card = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(10),
                    Padding = new Thickness(10),
                    Width = 400,
                    Height = 100,
                    Background = Brushes.LightGray
                };


                var stackPanel = new StackPanel();


                var textBlock1 = new TextBlock
                {
                    Text = $"Дата: {order.OrderDate}",
                    Margin = new Thickness(0, 0, 0, 5),
                    FontWeight = FontWeights.Bold
                };
                stackPanel.Children.Add(textBlock1);

                var textBlock2 = new TextBlock
                {
                    Text = $"Тип: {order.OrderType}",
                    Margin = new Thickness(0, 0, 0, 5)
                };
                stackPanel.Children.Add(textBlock2);

                var textBlock3 = new TextBlock
                {
                    Text = $"Описание: {order.OrderDetails}",
                    Margin = new Thickness(0, 0, 0, 5)
                };
                stackPanel.Children.Add(textBlock3);


                card.Child = stackPanel;


                EmployeeWrapPanel.Children.Add(card);
            }
        }
        private void EmployeePromotions_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Введите корректные имя и фамилию сотрудника!");
                return;
            }

            // Получаем приказы и распоряжения для заданного сотрудника по имени и фамилии
            var orders = GetEmployeeOrders(firstName, lastName);

            if (orders == null || !orders.Any())
            {
                MessageBox.Show("Приказы и распоряжения не найдены для данного сотрудника");
                return;
            }

            CreateOrderCards(orders); // Используем метод для создания карточек Order
        }

        private List<Order> GetEmployeeOrders(string firstName, string lastName)
        {
            using (var context = new HumanDepartmentDbContext())
            {
                // Получение EmployeeId по имени и фамилии
                var employee = context.Employees.FirstOrDefault(e => e.FirstName == firstName && e.LastName == lastName);

                if (employee == null)
                {
                    // Если сотрудник не найден, возвращаем пустой список
                    return new List<Order>();
                }

                // Возвращаем все приказы и распоряжения для найденного сотрудника
                return context.Orders.Where(o => o.EmployeeId == employee.EmployeeId).ToList();
            }
        }


        private void AllChildren_Click(object sender, RoutedEventArgs e)
        {
            int countChildren = 0;
            using (var dbContext = new HumanDepartmentDbContext())
            {

                var employees = dbContext.GetAllChildren();
                EmployeeWrapPanel.Children.Clear();
                foreach (var employee in employees)
                {
                    var card = CreateEmployeeCard(employee);
                    EmployeeWrapPanel.Children.Add(card);
                    countChildren += employee.Children;
                }
                MessageBox.Show("Количество детей:" + countChildren.ToString());
            }
        }

        private void EmployeesNotLivingInCity_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new HumanDepartmentDbContext())
            {
                var employees = dbContext.GetEmployeesNotLivingInCity(Adress.Text);
                if (string.IsNullOrWhiteSpace(Adress.Text))
                {
                    MessageBox.Show("Введите город!");
                    return;
                }
                RefreshEmployeeCards(employees);
            }
        }

        private void DepartmentEmployees_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new HumanDepartmentDbContext())
            {
                var selectedDepartment = (Department)DepartmentComboBox.SelectedItem;
                if (selectedDepartment == null)
                {
                    MessageBox.Show("Выберите Отдел!!!!!!!!!");
                    return;
                }
                var employees = dbContext.GetDepartmentEmployees(selectedDepartment.DepartmentId);
                RefreshEmployeeCards(employees);
            }
        }

        private void EmployeesOnLeave_Click(object sender, RoutedEventArgs e)
        {
            if (DateStart.SelectedDate == null || DateEnd.SelectedDate == null)
            {
                MessageBox.Show("Выберите Дату начала и конца!!!!!!!!");
                return;
            }

            var startDate = DateStart.SelectedDate.Value;
            var endDate = DateEnd.SelectedDate.Value;

            using (var dbContext = new HumanDepartmentDbContext())
            {
                var employees = dbContext.GetEmployeesOnLeave(startDate, endDate);
                RefreshEmployeeCards(employees);
            }
        }
        private void RefreshEmployeeCards(List<Employee> employees)
        {
            EmployeeWrapPanel.Children.Clear();
            foreach (var employee in employees)
            {
                var card = CreateEmployeeCard(employee);
                EmployeeWrapPanel.Children.Add(card);
            }
        }
        private void RefreshEmployeeCards(List<Education> educations)
        {
            EmployeeWrapPanel.Children.Clear();

            foreach (var education in educations)
            {
                var employee = education.Employee;

                var card = CreateEmployeeCard(employee);
                EmployeeWrapPanel.Children.Add(card);
            }
        }
        private void EditEmployee(Employee employee)
        {

            var editWindow = new EditEmployeeWindow(employee);
            if (editWindow.ShowDialog() == false)
            {
                EmployeeWrapPanel.Children.Clear();
                LoadEmployeeDetails();
            }
        }

        private void DeleteEmployee(Employee employee)
        {
            var result = MessageBox.Show($"Вы уверены, что хотите удалить сотрудника {employee.FirstName} {employee.LastName}?", "Удаление сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                using (var dbContext = new HumanDepartmentDbContext())
                {
                    // 1. Удаление записей из связанных таблиц
                    // Удаление из таблицы Education
                    var educations = dbContext.Educations.Where(e => e.EmployeeId == employee.EmployeeId).ToList();
                    dbContext.Educations.RemoveRange(educations);

                    // Удаление из таблицы Order
                    var orders = dbContext.Orders.Where(o => o.EmployeeId == employee.EmployeeId).ToList();
                    dbContext.Orders.RemoveRange(orders);

                    // Удаление из таблицы Promotion
                    var promotions = dbContext.Promotions.Where(p => p.EmployeeId == employee.EmployeeId).ToList();
                    dbContext.Promotions.RemoveRange(promotions);

                    // Удаление из таблицы SickLeaf
                    var sickLeaves = dbContext.SickLeaves.Where(s => s.EmployeeId == employee.EmployeeId).ToList();
                    dbContext.SickLeaves.RemoveRange(sickLeaves);

                    // Удаление из таблицы Vacation
                    var vacations = dbContext.Vacations.Where(v => v.EmployeeId == employee.EmployeeId).ToList();
                    dbContext.Vacations.RemoveRange(vacations);

                    // Удаление из таблицы WorkHistory
                    var workHistories = dbContext.WorkHistories.Where(w => w.EmployeeId == employee.EmployeeId).ToList();
                    dbContext.WorkHistories.RemoveRange(workHistories);

                    // 2. Удаление сотрудника из таблицы Employees
                    dbContext.Employees.Remove(employee);
                    dbContext.SaveChanges();
                    EmployeeWrapPanel.Children.Clear();
                    LoadEmployeeDetails();
                }
            }
        }
    }
}