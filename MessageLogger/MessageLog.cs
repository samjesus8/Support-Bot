using System.Collections.Generic;

namespace DiscordBotTemplate.MessageLogger
{
    public class MessageLog
    {
        public List<DMessage> messages = new List<DMessage>();
        public static readonly MessageLog instance = new MessageLog();

        static MessageLog() { }
    }
}
