class Program
{
    static async Task Main(string[] args)
    {
        string botToken = "7073034781:AAHe3mNDNDN2a214jxRVByLvaakYEYLXA78"; // Замените на ваш токен бота
        var telegramBot = new TelegramBot(botToken);

        await telegramBot.StartAsync();
    }
}
