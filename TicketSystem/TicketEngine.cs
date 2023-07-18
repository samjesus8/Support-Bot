using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

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

        public bool DeleteTicket(ulong ticketID)
        {
            try
            {
                string path = @"C:\Users\samue\Source\Repos\Support-Bot\bin\Debug\tickets.json";

                var json = File.ReadAllText(path);
                var jsonObj = JObject.Parse(json);

                var tickets = jsonObj["tickets"].ToObject<List<SupportTicket>>();

                bool isDeleted = true;
                while (isDeleted == true)
                {
                    for (int i = 0; i < tickets.Count; i++)
                    {
                        if (tickets[i].ticketId == ticketID)
                        {
                            tickets.Remove(tickets[i]);
                            isDeleted = false;
                        }
                    }
                }

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

        public List<SupportTicket> GetTickets()
        {
            List<SupportTicket> tickets = new List<SupportTicket>();

            using (StreamReader sr = new StreamReader("tickets.json"))
            {
                string json = sr.ReadToEnd();
                TicketJSON ticketObj = JsonConvert.DeserializeObject<TicketJSON>(json);

                foreach (var ticket in ticketObj.tickets)
                {
                    tickets.Add(ticket);
                }
            }

            return tickets;
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
