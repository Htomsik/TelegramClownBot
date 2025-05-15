using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTelegram;
using TL;

class Program
{
    private static Client? _client;

    private static User? _me;
    
    private static readonly List<string> ClownsNickName = new List<string>(){};
    
    private static readonly Dictionary<string, long> ClownsNickNameToId = new Dictionary<string, long>();
    
    static async Task Main(string[] args)
    {
        // Create client
        _client = new Client(Config);
        
        // Authorize
        var isAuthorize = await Authorize();
        if(!isAuthorize)
            return;

        // Get all clowns IDs
        await GetAllClownsIds();
        
        // Add event handler for receiving updates
        _client.OnUpdates += UpdateReceive;
        
        // Wait until user terminates the program
        await Task.Delay(-1); // infinite waiting
    }
    
    private static async Task<bool> Authorize()
    {
        // Connect to Telegram servers
        await _client.ConnectAsync();
        _me = await _client.LoginUserIfNeeded();
        
        if(_me is null)
            return false;
        
        Console.WriteLine($"Successfully authorized! {_me.first_name}!");
        return true;
    }
    
    private static async Task GetAllClownsIds()
    {
        var dialogs = await _client.Messages_GetAllDialogs();
        foreach (var dialog in dialogs.dialogs)
        {
            if (dialog.Peer is not PeerUser peerUser)
                continue;
            
            var user = dialogs.users[peerUser.user_id];
            
            if(ClownsNickName.Contains(user.username))
                ClownsNickNameToId.Add(user.username, user.id);
        }
    }
    
    // Telegram event handler
    private static async Task UpdateReceive(UpdatesBase updates)
    {
        // Process all updates in the list
        if (updates.UpdateList == null)
                return;
        
        foreach (var update in updates.UpdateList)
        {

            if (update is UpdateNewMessage unm)
            {
                await MessageReceive(unm);
                continue;
            }
        }
    }

    private static async Task MessageReceive(UpdateNewMessage unm)
    {
        // Get the message
        var message = unm.message;
        if (message.From is null || !ClownsNickNameToId.ContainsValue(message.From.ID))
        {
            return;
        }
            
        Console.WriteLine($"[{DateTime.Now}] New message from {ClownsNickNameToId.FirstOrDefault(x => x.Value == message.From.ID).Key ?? "unknown user"}");
                
        // Send reaction to message
        await _client.Messages_SendReaction(
            peer: InputPeer.Self, 
            msg_id: message.ID, 
            reaction: new[] { new ReactionEmoji { emoticon = "🤡" } });
                            
        Console.WriteLine($"Reaction sent to message from {ClownsNickNameToId.FirstOrDefault(x => x.Value == message.From.ID).Key ?? "unknown user"}");
    }
    
    static string Config(string what)
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

    // Simple method to send a message to user by ID
    private static async Task SendMessageToUser(long userId, string message)
    {
        var dialogs = await _client.Messages_GetAllDialogs();
        
        if(!dialogs.users.TryGetValue(userId, out User? target))
            return;
        
        await _client.SendMessageAsync(target, message);
    }
    
    // Spam method
    private static async Task SpamMessageToUser(long userId, string message)
    {
        var dialogs = await _client.Messages_GetAllDialogs();
        
        if(!dialogs.users.TryGetValue(userId, out User? target))
            return;

        for (int i = 0; i < 100; i++)
        {
            await Task.Delay(1000);
            await _client.SendMessageAsync(target, message);
        }
    }
}
