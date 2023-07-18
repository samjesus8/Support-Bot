using System;

namespace DiscordBotTemplate.MessageLogger
{
    public class DMessage
    {
        public string username { get; set; }
        public ulong channelID { get; set; }
        public string content { get; set; }
        public DateTime SendDate { get; set; }
    }
}
