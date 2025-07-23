namespace TelegramClownBot.Models;


/// <summary>
///     PERMANENT TG Authorization dataset
/// </summary>
public record AuthorizationData(string ApiId, string ApiHash, string PhoneNumber, string Password);