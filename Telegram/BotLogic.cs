using KursProjectDepartament.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

public class BotLogic
{
    private readonly ITelegramBotClient _botClient;
    private readonly HumanDepartmentDbContext _dbContext;
    private string userInput;
    private readonly Dictionary<long, string> _currentCommands = new Dictionary<long, string>();

    public BotLogic(ITelegramBotClient botClient, HumanDepartmentDbContext dbContext)
    {
        _botClient = botClient;
        _dbContext = dbContext;
    }
    public async Task HandleCommandListAsync(long chatId)
    {
        var commandList = "Доступные команды:\n" +
                          "/employees_without_higher_education - Список сотрудников без высшего образования\n" +
                          "/educations_mismatched - Список сотрудников, образование которых не соответствует должности\n" +
                          "/employee_promotions - Список приказов и распоряжений для заданного сотрудника\n" +
                          "/all_children - Список всех детей сотрудников (в зависимости от семейного положения)\n" +
                          "/employees_not_in_city - Список сотрудников, не проживающих в заданном городе\n" +
                          "/department_employees - Список сотрудников отдела\n" +
                          "/employees_on_leave - Список сотрудников на больничном или в отпуске в заданный период\n" +
                          "/help - Показать список команд";

        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: commandList
        );
    }
    public async Task HandleUpdateAsync(Update update)
    {
        if (update.Message is not { } message || message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;

        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        if (_currentCommands.TryGetValue(chatId, out var currentCommand))
        {
            switch (currentCommand)
            {
                case "/employee_promotions":
                    await HandleEmployeePromotionsCommandAsync(chatId, messageText);
                    _currentCommands.Remove(chatId);
                    return;
                default:
                    _currentCommands.Remove(chatId);
                    break;
            }
        }

        switch (messageText.ToLower())
        {
            case "/employees_without_higher_education":
                var employeesWithoutHigherEducation = _dbContext.GetEmployeesWithoutHigherEducation();
                await SendEmployeesListAsync(chatId, employeesWithoutHigherEducation.Cast<object>().ToList());
                break;
            case "/educations_mismatched":
                var employeesMismatched = _dbContext.GetEducationsWithMismatchedFieldOfStudy();
                await SendEmployeesListAsync(chatId, employeesMismatched.Cast<object>().ToList());
                break;
            case "/employee_promotions":
                _currentCommands[chatId] = "/employee_promotions";
                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Введите имя и фамилию сотрудника для получения списка его приказов и распоряжений:"
                );
                break;
            case "/all_children":
                var allChildren = _dbContext.GetAllChildren();
                await SendEmployeesListAsync(chatId, allChildren.Cast<object>().ToList());
                break;
            case "/employees_not_in_city":
                var employeesNotInCity = _dbContext.GetEmployeesNotLivingInCity("Гурьевск");
                await SendEmployeesListAsync(chatId, employeesNotInCity.Cast<object>().ToList());
                break;
            case "/department_employees":
                var departmentEmployees = _dbContext.GetDepartmentEmployees(1);
                await SendEmployeesListAsync(chatId, departmentEmployees.Cast<object>().ToList());
                break;
            case "/employees_on_leave":
                var startDate = DateTime.Now.AddMonths(-1);
                var endDate = DateTime.Now;
                var employeesOnLeave = _dbContext.GetEmployeesOnLeave(startDate, endDate);
                await SendEmployeesListAsync(chatId, employeesOnLeave.Cast<object>().ToList());
                break;
            case "/help":
                await HandleCommandListAsync(chatId);
                break;
            default:
                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Неизвестная команда. Используйте доступные команды."
                );
                break;
        }
    }


    private async Task HandleEmployeePromotionsCommandAsync(long chatId, string userInput)
    {

        var parts = userInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {

            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Введите имя и фамилию сотрудника для получения списка его приказов и распоряжений в формате 'Имя Фамилия'."
            );
            return;
        }


        var firstName = parts[0];
        var lastName = parts[1];


        var employee = _dbContext.Employees.FirstOrDefault(e => e.FirstName == firstName && e.LastName == lastName);

        if (employee != null)
        {

            var orders = _dbContext.Orders
                                 .Where(o => o.EmployeeId == employee.EmployeeId)
                                 .ToList();

            if (orders.Any())
            {

                var ordersInfo = $"Список приказов или распоряжений для сотрудника {firstName} {lastName}:\n";
                foreach (var order in orders)
                {
                    ordersInfo += $"- Тип: {order.OrderType}, Дата: {order.OrderDate}, Детали: {order.OrderDetails ?? "нет информации"}\n";
                }

                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: ordersInfo
                );
            }
            else
            {
                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"Для сотрудника {firstName} {lastName} нет приказов или распоряжений."
                );
            }
        }
        else
        {
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Сотрудник {firstName} {lastName} не найден."
            );
        }
    }

    private string FormatEmployee(Employee employee)
    {
        return $"ID: {employee.EmployeeId}, Фамилия: {employee.LastName},: Имя {employee.FirstName}, Отчество: {employee.MiddleName} Должность: {employee.Position}, Телефон: {employee.PhoneNumber}, Город: {employee.Address}, Детей: {employee.Children}";

    }

    private string FormatEducation(Education education)
    {
        return $"ID: {education.EducationId}, Уровень образования: {education.Degree}, Направление: {education.FieldOfStudy}";
    }

    private async Task SendEmployeesListAsync(long chatId, List<object> employees)
    {
        if (employees.Any())
        {
            foreach (var employee in employees)
            {
                string message;
                switch (employee)
                {
                    case Employee e:
                        message = FormatEmployee(e);
                        break;
                    case Education ed:
                        message = FormatEducation(ed);
                        break;
                    default:
                        message = employee.ToString();
                        break;
                }

                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: message
                );
            }
        }
        else
        {
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Список сотрудников пуст."
            );
        }
    }
}