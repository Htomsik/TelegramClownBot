namespace TelegramClownBot.Models;

/// <summary>
///     Settings of application
/// </summary>
public class AppSettings
{
    /// <summary>
    ///     "Document" sticker set name
    /// </summary>
    public string PingStickerSetName { get; set; }
    
    /// <summary>
    ///     Emoji in "Document" sticker set name
    /// </summary>
    public string PingStickerEmoji{ get; set; }
    
    /// <summary>
    ///     Simple emoji for reply
    /// </summary>
    public string ReactionEmoji { get; set; }
    
    public AppSettings(string pingStickerSetName, 
        string pingStickerEmoji,
        string reactionEmoji
      )
    {
        PingStickerSetName = pingStickerSetName;
        ReactionEmoji = reactionEmoji;
        PingStickerEmoji = pingStickerEmoji;
    }
}


