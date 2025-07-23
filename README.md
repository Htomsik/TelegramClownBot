# TelegramClownBot

A simple Telegram bot that automatically reacts with emoji (🤡 by default) to messages

## Features

- Interactive TUI interface for "best friends" selection
- Automatic emoji reactions and send "advanced" sticker to messages from "best friends" users
- Console notifications about new messages from tracked users
- Ability to send text/"advanced" sticker messages to specified users (doesn't used in manual)

## Setup

1. Register new Telegram app and get your `api_id` and `api_hash` from [my.telegram.org](https://my.telegram.org/)
2. Specify your phone `number`, `password`, `api_id`, `api_hash` in the `AuthorizationData` record in `Program.cs`
3. Configure sticker and reaction settings `AppSettings` in `Program.cs`
4. Add usernames to the `Clowns` list (in runtime)

On first launch, you'll need to enter the verification code sent to your Telegram account.

## PS
This is a small rofl project without any normal architecture

**Use it with responsibly and in accordance with Telegram's rules.**

