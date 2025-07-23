using TL;

namespace TelegramClownBot.Services.Interfaces
{
    public interface IMessageService
    {
        /// <summary>
        ///     Send text message to user
        /// </summary>
        Task SendMessageAsync(User targetUser, string message);

        /// <summary>
        ///     Send sticker to user (NOT EMOJI)
        /// </summary>
        Task SendStickerMessageAsync(User targetUser, string stickerSetName, string stickerEmotion);
        
        /// <summary>
        ///     Send emoji reaction to message
        /// </summary>
        Task SendMessageReactionAsync(int messageId, string reactionEmotion);
        
        /// <summary>
        ///     Background message worker
        /// </summary>
        Task ProcessUpdatesAsync(UpdatesBase updates);
    }
} 