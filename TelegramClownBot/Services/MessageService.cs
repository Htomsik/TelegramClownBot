using System.Linq;
using System.Threading.Tasks;
using TelegramClownBot.Core;
using TelegramClownBot.Services.Interfaces;
using TL;

namespace TelegramClownBot.Services
{
    public class MessageService : IMessageService
    {
        private readonly BotContext _context;
        
        public MessageService(BotContext context)
        {
            _context = context;
        }
        
        public async Task SendMessageAsync(long userId, string message)
        {
            var dialogs = await _context.TelegramClient.Messages_GetAllDialogs();
            
            if(!dialogs.users.TryGetValue(userId, out User? target))
                return;
            
            await _context.TelegramClient.SendMessageAsync(target, message);
        }
        
        public async Task ProcessUpdatesAsync(UpdatesBase updates)
        {
            if (updates.UpdateList == null)
                return;
            
            foreach (var update in updates.UpdateList)
            {
                if (update is UpdateNewMessage unm)
                {
                    await ProcessMessageAsync(unm);
                }
            }
        }
        
        private async Task ProcessMessageAsync(UpdateNewMessage unm)
        {
            var message = unm.message;
            
            if (message.From == null)
                return;
            
            // Find user in our list
            var user = _context.Users.FirstOrDefault(u => u.Id == message.From.ID);
            
            // Check if the user is selected in the selection service
            if (user == null || _context.ClownSelectionService.IsSelected(user))
                    return;
            
            // Send clown reaction
            await _context.TelegramClient.Messages_SendReaction(
                peer: InputPeer.Self,
                msg_id: message.ID,
                reaction: new[] { new ReactionEmoji { emoticon = "🤡" } }
            );
        }
    }
} 