using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KursProjectDepartament.Model
{
    class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null!)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object? parameter)
        {
            return this.canExecute == null || this.canExecute(parameter!);
        }
        public void Execute(object? parameter)
        {
            this.execute(parameter!);
            var command = new RelayCommand(ExecuteCommand);

            // Для примера, здесь мы вызываем метод ExecuteCommand при выполнении команды
            command.Execute(null);
        }

        public static void ExecuteCommand(object parameter)
        {
            var childrenCountByMaritalStatus = GetChildrenCountByMaritalStatus();

            foreach (var kvp in childrenCountByMaritalStatus)
            {
                Console.WriteLine($"Семейное положение: {kvp.Key}, Количество детей: {kvp.Value}");
            }
        }

        public static Dictionary<string?, int> GetChildrenCountByMaritalStatus()
        {
            var employees = new List<Employee>
    {
        new Employee { MaritalStatus = "Женат/Замужем", Children = 2 },
        new Employee { MaritalStatus = "Не женат/Не замужем", Children = 0 },
        new Employee { MaritalStatus = "Разведен/Разведена", Children = 1 },
        new Employee { MaritalStatus = "Женат/Замужем", Children = 3 },
        new Employee { MaritalStatus = null, Children = 0 }
    };

            var childrenCountByMaritalStatus = new Dictionary<string?, int>();

            foreach (var employee in employees)
            {
                if (employee.MaritalStatus != null)
                {
                    string? status = employee.MaritalStatus; // Temporary assignment to avoid nullability issue
                    if (!childrenCountByMaritalStatus.ContainsKey(status))
                    {
                        childrenCountByMaritalStatus[status] = 0;
                    }
                    childrenCountByMaritalStatus[status] += employee.Children;
                }
            }

            int notSpecifiedCount = employees.Count(e => e.MaritalStatus == null);
            childrenCountByMaritalStatus.Add("Не указано", notSpecifiedCount);

            return childrenCountByMaritalStatus;
        }
    }
}


