using LiteTUI.Commands;
using LiteTUI.Controls.Menu;
using LiteTUI.Core;
using TelegramClownBot.Commands;
using TelegramClownBot.Core;
using TelegramClownBot.Models;
using TelegramClownBot.Services;
using TelegramClownBot.Services.Interfaces;
using WTelegram;

namespace TelegramClownBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create global application context
            var context = new BotContext();
            
            // Disable WTelegram library logging
            Helpers.Log = (lvl, str) => { };
            
            // Create services directly
            IAuthService authService = new AuthService(context, new TextInputCommand(context));
            IMessageService messageService = new MessageService(context);
            IUserService userService = new UserService(context);
            
            // Create main menu
            var mainMenu = new Menu("Telegram Clown Bot");
            
            // Add authorization command
            mainMenu.Items.Add(new MenuItem("Authorize in Telegram", 
                new AuthorizeCommand(context, authService, userService, messageService)));
            
            // Create clown user selection menu
            var clownsMenu = new MenuSelection<TelegramUser>(
                context,
                context.ClownSelectionService,
                "Select users for clown reaction",
                user => user.DisplayName,  // function to get item text
                new ChangeControlCommand(context, mainMenu)  // back command
            );
            
            // Add command to navigate to user management menu
            mainMenu.Items.Add(new MenuItem("Clowns reaction user list", 
                new ChangeControlCommand(context, clownsMenu)));
            
            // Add exit command
            mainMenu.Items.Add(new MenuItem("Exit", new ExitCommand(context)));
            
            // Create and run application
            var application = new ApplicationRunner(context, mainMenu);
            await application.RunAsync();
        }
    }
}