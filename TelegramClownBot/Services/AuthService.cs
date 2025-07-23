using System;
using System.Threading.Tasks;
using LiteTUI.Commands;
using TelegramClownBot.Core;
using TelegramClownBot.Services.Interfaces;
using TL;
using WTelegram;

namespace TelegramClownBot.Services
{
    public class AuthService : IAuthService
    {
        private readonly BotContext _context;
        
        private readonly TextInputCommand _textInputCommand;

        public AuthService(BotContext context, TextInputCommand textInputCommand)
        {
            _context = context;
            _textInputCommand = textInputCommand;
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
        private string? Config(string what)
        {
            switch (what)
            {
                case "api_id": return "";
                case "api_hash": return "";
                case "phone_number": return "";
                case "verification_code": return GetData("Verification сode").Result;
                case "password": return "";
                default: return null;  
            }
        }
        
        private async Task<string?> GetData(string what)
        {
            _textInputCommand.Title = what;
            return await _textInputCommand.ExecuteAsync();
        }
    }
} 