using System;
using System.Threading.Tasks;
using TelegramClownBot.Core;
using TelegramClownBot.Services.Interfaces;
using TL;
using WTelegram;

namespace TelegramClownBot.Services
{
    public class AuthService : IAuthService
    {
        private readonly BotContext _context;
        
        public AuthService(BotContext context)
        {
            _context = context;
        }
        
        public async Task<bool> AuthorizeAsync()
        {
            try
            {
                // Create client if needed
                if (_context.TelegramClient == null)
                    _context.TelegramClient = new Client(Config);
                
                // Connect to Telegram servers
                await _context.TelegramClient.ConnectAsync();
                
                // Authorize
                _context.Me = await _context.TelegramClient.LoginUserIfNeeded();
                
                if (_context.Me == null)
                    return false;
                
                // Successful authorization
                _context.IsAuthorized = true;
                
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        // Configuration for Telegram authorization
        private static string Config(string what)
        {
            switch (what)
            {
                case "api_id": return "";
                case "api_hash": return "";
                case "phone_number": return "";
                case "verification_code": Console.Write("Code: "); return Console.ReadLine();
                case "password": return ""; 
                default: return null;  
            }
        }
    }
} 