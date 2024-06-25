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

    public BotLogic(ITelegramBotClient botClient, HumanDepartmentDbContext dbContext)
    {
        _botClient = botClient;
        _dbContext = dbContext;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
            return;

        // Only process text messages
        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;

        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        // Обработка команд
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
                await HandleEmployeePromotionsCommandAsync(chatId);
                break;
            case "/all_children":
                var allChildren = _dbContext.GetAllChildren();
                await SendEmployeesListAsync(chatId, allChildren.Cast<object>().ToList());
                break;
            case "/employees_not_in_city":
                var employeesNotInCity = _dbContext.GetEmployeesNotLivingInCity("Moscow"); // Замените "Moscow" на нужный город
                await SendEmployeesListAsync(chatId, employeesNotInCity.Cast<object>().ToList());
                break;
            case "/department_employees":
                var departmentEmployees = _dbContext.GetDepartmentEmployees(1); // Замените 1 на нужный departmentId
                await SendEmployeesListAsync(chatId, departmentEmployees.Cast<object>().ToList());
                break;
            case "/employees_on_leave":
                var startDate = DateTime.Now.AddMonths(-1); // Пример: начало периода за последний месяц
                var endDate = DateTime.Now; // Пример: конец периода - текущая дата
                var employeesOnLeave = _dbContext.GetEmployeesOnLeave(startDate, endDate);
                await SendEmployeesListAsync(chatId, employeesOnLeave.Cast<object>().ToList());
                break;
            default:
                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Неизвестная команда. Используйте доступные команды."
                );
                break;
        }
    }
    private async Task HandleEmployeePromotionsCommandAsync(long chatId)
    {
        // Для примера реализации обработчика команды /employee_promotions
        // Получаем информацию о сотруднике, возможно, используя другие команды
        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Введите идентификатор сотрудника для получения списка его приказов и распоряжений:"
        );
    }

        private async Task SendEmployeesListAsync<T>(long chatId, List<T> employees)
    {
        if (employees.Any())
        {
            foreach (var employee in employees)
            {
                // Пример отправки сообщения с информацией о сотруднике
                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: employee.ToString()
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
