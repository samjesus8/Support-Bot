using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotTemplate.TicketSystem
{
    public class TicketEngine
    {
        public bool StoreTicket(SupportTicket ticket)
        {
            try
            {
                string path = @"C:\Users\samue\Source\Repos\Support-Bot\bin\Debug\tickets.json";

                var json = File.ReadAllText(path);
                var jsonObj = JObject.Parse(json);

                var tickets = jsonObj["tickets"].ToObject<List<SupportTicket>>();
                tickets.Add(ticket);

                jsonObj["tickets"] = JArray.FromObject(tickets);
                File.WriteAllText(path, jsonObj.ToString());

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public int GetTotalTickets()
        {
            int count = 0;
            using (StreamReader sr = new StreamReader("tickets.json"))
            {
                string json = sr.ReadToEnd();
                TicketJSON ticketObj = JsonConvert.DeserializeObject<TicketJSON>(json);

                foreach(var ticket in ticketObj.tickets)
                {
                    count++;
                }
            }

            return count;
        }
    }

    internal sealed class TicketJSON
    {
        public SupportTicket[] tickets { get; set; }
    }
}
