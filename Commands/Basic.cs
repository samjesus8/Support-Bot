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

        [Command("manage")]
        public async Task ManageSystem(CommandContext ctx)
        {
            var viewButton = new DiscordButtonComponent(ButtonStyle.Primary, "viewButton", "View Tickets");
            var editButton = new DiscordButtonComponent(ButtonStyle.Primary, "editButton", "Edit Ticket");
            var deleteButton = new DiscordButtonComponent(ButtonStyle.Primary, "deleteButton", "Delete Ticket");

            var dashboard = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Azure)
                    .WithTitle("Ticket Dashboard")
                    .WithDescription("1) View Tickets \n" +
                                     "2) Edit Ticket \n" +
                                     "3) Delete Ticket"))
                .AddComponents(viewButton, editButton, deleteButton);

            await ctx.Channel.SendMessageAsync(dashboard);
        }
    }
}
