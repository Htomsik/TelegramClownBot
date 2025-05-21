using TL;

namespace TelegramClownBot.Models
{
    public record TelegramUser
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string FullName => $"{FirstName} {LastName}".Trim();
        
        public string DisplayName => string.IsNullOrEmpty(Username) 
            ? FullName 
            : $"{FullName} (@{Username})";
            
        public TelegramUser(User user)
        {
            Id = user.id;
            Username = user.username ?? string.Empty;
            FirstName = user.first_name ?? string.Empty;
            LastName = user.last_name ?? string.Empty;
        }
    }
} 