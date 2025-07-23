using System.Linq;
using System.Threading.Tasks;
using TelegramClownBot.Core;
using TelegramClownBot.Models;
using TelegramClownBot.Services.Interfaces;
using TL;
using WTelegram;

namespace TelegramClownBot.Services
{
    public class MessageService : IMessageService
    {
        private readonly BotContext _context;
        
        public MessageService(BotContext context)
        {
            _context = context;
        }
        
        public async Task SendMessageAsync(User?  targetUser, string message)
        {
            if (_context.TelegramClient is null 
                || targetUser is null  
                || targetUser.ID == 0)
                return;
            
            await _context.TelegramClient.SendMessageAsync(targetUser, message);
        }
        
        public async Task SendStickerMessageAsync(User? targetUser, string stickerSetName, string stickerEmotion)
        {
            if (_context.TelegramClient is null 
                || string.IsNullOrWhiteSpace(stickerSetName)
                || string.IsNullOrWhiteSpace(stickerEmotion)
                || targetUser is null  
                || targetUser.ID == 0)
                return;
            
            // TG have strange sticker API
            // Pack combine of two arrays 
            // One array with dock (body of sticker) + id (mainArray)
            // Second array with emotionName + id (idArray)
            
            // Get needed sticker idArray
            var stickerSets = await _context.TelegramClient.Messages_GetStickerSet(stickerSetName);

            // Find needed sticker in idArray
            var sticker = stickerSets.packs.FirstOrDefault(sticker => sticker.emoticon == stickerEmotion);
            if (sticker == null || sticker.documents.Length == 0)
                return;
            
            // Get sticker Id from idArray
            var stickerId = stickerSets.packs.First(p => p.emoticon == stickerEmotion).documents[0];
            
            // Get dock by id from mainArray
            var stickerDock = stickerSets.documents.First(d => d.ID == stickerId);
            if (stickerDock == null)
                return;
            
            await _context.TelegramClient.SendMessageAsync(targetUser, null, stickerDock);
        }

        public async Task SendMessageReactionAsync(int messageId, string reactionEmotion)
        {
            await _context.TelegramClient.Messages_SendReaction(
                peer: InputPeer.Self,
                msg_id: messageId,
                reaction: new Reaction[] { new ReactionEmoji { emoticon = reactionEmotion } }
            );
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
            if (unm.message.From == null)
                return;
            
            // Find user in our list
            var user = _context.Users.FirstOrDefault(u => u.Id == unm.message.From.ID);
            
            // Check if the user is selected in the selection service
            if (user == null ||
                !_context.ClownSelectionService.IsSelected(user, userSelect => user.Id == userSelect.Id))
                return;
            
            var stickerTask = SendStickerMessageAsync(user.OriginalUser, _context.Settings.PingStickerSetName, _context.Settings.PingStickerEmoji);
            var reactionTask = SendMessageReactionAsync(unm.message.ID, _context.Settings.ReactionEmoji);
            
            await Task.WhenAll(stickerTask, reactionTask);
        }
    }
} 