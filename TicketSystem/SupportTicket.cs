using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotTemplate.TicketSystem
{
    public class SupportTicket
    {
        public string username { get; set; }
        public string issue { get; set; }
        public int ticketNo { get; set; }
        public ulong ticketId { get; set; }
    }
}
