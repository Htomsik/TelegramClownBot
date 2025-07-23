using System;
using System.Threading.Tasks;
using LiteTUI.Commands.Base;
using TelegramClownBot.Core;
using TelegramClownBot.Services.Interfaces;

namespace TelegramClownBot.Commands
{
    public class AuthorizeCommand : CommandBase<bool>
    {
        private readonly BotContext _botContext;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        
        public AuthorizeCommand(
            BotContext context, 
            IAuthService authService, 
            IUserService userService,
            IMessageService messageService) : base(context)
        {
            _botContext = context;
            _authService = authService;
            _userService = userService;
            _messageService = messageService;
        }
        
        public override async Task<bool> ExecuteAsync()
        {
            // Reset status before starting
            State = "Connecting...";
            
            try
            {
                bool isAuthorized = await _authService.AuthorizeAsync();
                
                if (!isAuthorized)
                {
                    State = "Authorization error";
                    return true;
                }
                
                // Successful authorization
                State = $"Authorized as {_botContext.Me.first_name}";
                
                // Connect event handler after successful authorization
                _botContext.TelegramClient.OnUpdates += _messageService.ProcessUpdatesAsync;
                
                // Load user list
                State = "Loading users...";
                await _userService.LoadUsersAsync();
                State = $"{_botContext.Me.username}. {_botContext.Users.Count} users";
            }
            catch (Exception ex)
            {
                State = $"Error: {ex.Message}";
            }
            
            return true;
        }
    }
} 