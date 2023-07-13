using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace DiscordBotTemplate.Commands
{
    public class Basic : BaseCommandModule
    {
        [Command("display")]
        public async Task TestCommand(CommandContext ctx) 
        {
            var submitButton = new DiscordButtonComponent(ButtonStyle.Primary, "submitButton", "Submit Ticket");

            var supportEmbed = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Azure)
                    .WithTitle("Support System")
                    .WithDescription("Please click the below button to submit an issue"))
                .AddComponents(submitButton);

            await ctx.Channel.SendMessageAsync(supportEmbed);
        }
    }
}
