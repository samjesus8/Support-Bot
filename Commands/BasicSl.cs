using DiscordBotTemplate.MessageLogger;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotTemplate.Commands
{
    public class BasicSl : ApplicationCommandModule
    {
        [SlashCommand("transcribe", "Generates a log of messages sent in a channel")]
        public async Task MessageLogGenerator(InteractionContext ctx, [Option("channel-id", "The channel ID where you want to log the messages")] string channelID,
                                                                      [Option("start-date", "The date where you want to start logging DD/MM/YYY")] string fromDate,
                                                                      [Option("end-date", "The date where you want to log to")] string toDate)
        {
            await ctx.DeferAsync();

            ulong channelId = ulong.Parse(channelID);
            DateTime startDate = DateTime.Parse(fromDate);
            DateTime endDate = DateTime.Parse(toDate);

            List<DMessage> messagesInRange = Program.logger.messages.Where(message => message.channelID == channelId && message.SendDate >= startDate && message.SendDate <= endDate).ToList();

            using (StreamWriter wr = new StreamWriter("output.txt"))
            {
                foreach (var message in messagesInRange)
                {
                    string stringToWrite = $"============================= \n" +
                                           $"User: {message.username} \n" +
                                           $"Content: {message.content} \n" +
                                           $"Date {message.SendDate} \n" +
                                           $"============================= \n";

                    wr.WriteLine(stringToWrite);
                }
            }

            FileStream file = new FileStream(@"C:\Users\samue\Source\Repos\Support-Bot\bin\Debug\output.txt", FileMode.Open, FileAccess.Read);
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddFile(file));
        }
    }
}
