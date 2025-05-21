using System.Linq;
using System.Threading.Tasks;
using TelegramClownBot.Core;
using TelegramClownBot.Models;
using TelegramClownBot.Services.Interfaces;
using TL;

namespace TelegramClownBot.Services
{
    public class UserService : IUserService
    {
        private readonly BotContext _context;
        
        public UserService(BotContext context)
        {
            _context = context;
        }
        
        public async Task LoadUsersAsync()
        {
            var dialogs = await _context.TelegramClient.Messages_GetAllDialogs();
            
            // Save IDs of currently selected users
            var selectedUsers = _context.ClownSelectionService.SelectedItems;
            
            // Clear users list and selection
            _context.Users.Clear();
            _context.ClownSelectionService.SelectedItems.Clear();
            
            // Add all users from dialogs
            foreach (var dialog in dialogs.dialogs)
            {
                if (dialog.Peer is not PeerUser peerUser)
                    continue;
                
                var user = dialogs.users[peerUser.user_id];
                var telegramUser = new TelegramUser(user);
                _context.Users.Add(telegramUser);
                
                // Restore selection if user was selected before
                if (selectedUsers.Contains(telegramUser))
                {
                    _context.ClownSelectionService.ToggleSelection(telegramUser);
                }
            }
        }
    }
} 