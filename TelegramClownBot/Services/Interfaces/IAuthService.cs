namespace TelegramClownBot.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> AuthorizeAsync();
    }
} 