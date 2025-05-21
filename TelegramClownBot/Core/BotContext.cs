using System.Collections.Generic;
using LiteTUI.Core;
using LiteTUI.Services;
using TelegramClownBot.Models;
using TL;
using WTelegram;

namespace TelegramClownBot.Core
{
    public class BotContext : ApplicationContext
    {
        public Client? TelegramClient { get; set; }
        public User? Me { get; set; }
        public List<TelegramUser> Users { get; set; } = new();
        public SelectionService<TelegramUser> ClownSelectionService { get; set; }
        public bool IsAuthorized { get; set; } = false;
        
        public BotContext()
        {
            ClownSelectionService = new SelectionService<TelegramUser>(Users);
        }
    }
} 