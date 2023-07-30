﻿namespace Telegram.Framework.TelegramBotApplication.Exceptions
{
    public class MessageMayBeTooOld : Exception
    {
        public MessageMayBeTooOld() 
            : base("[UpdateType.CallbackQuery] The message may be too old")
        {
        }
    }
}