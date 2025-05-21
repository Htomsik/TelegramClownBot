using TL;

namespace TelegramClownBot.Services.Interfaces
{
    public interface IMessageService
    {
        Task SendMessageAsync(long userId, string message);
        Task ProcessUpdatesAsync(UpdatesBase updates);
    }
} 