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
        /// <summary>
        ///     PERMANENT TG Authorization dataset
        /// </summary>
        public AuthorizationData AuthorizationData { get; set; }
        
        /// <summary>
        ///     TEMPORARY application setiings
        /// </summary>
        public AppSettings Settings { get; }
        
        
        /// <summary>
        ///     Api client for TG operations
        /// </summary>
        public Client? TelegramClient { get; set; }
        
        /// <summary>
        ///     CurrentUser
        /// </summary>
        public User? Me { get; set; }
        
        /// <summary>
        ///     Contact list
        /// </summary>
        public List<TelegramUser> Users { get; set; } = new();
        
        /// <summary>
        ///     Clown contact list
        /// </summary>
        public SelectionService<TelegramUser> ClownSelectionService { get; set; }  = new SelectionService<TelegramUser>();
        
        public BotContext(AppSettings settings, AuthorizationData authorizationData)
        {
            Settings = settings;
            AuthorizationData = authorizationData;
        }
    }
} 