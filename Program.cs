using DiscordBotTemplate.Commands;
using DiscordBotTemplate.Config;
using DiscordBotTemplate.TicketSystem;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotTemplate
{
    public sealed class Program
    {
        public static DiscordClient Client { get; private set; }
        public static CommandsNextExtension Commands { get; private set; }
        static async Task Main(string[] args)
        {
            //1. Get the details of your config.json file by deserialising it
            var configJsonFile = new JSONReader();
            await configJsonFile.ReadJSON();

            //2. Setting up the Bot Configuration
            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = configJsonFile.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            //3. Apply this config to our DiscordClient
            Client = new DiscordClient(discordConfig);

            //4. Set the default timeout for Commands that use interactivity
            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            //5. Set up the Task Handler Ready event
            Client.Ready += OnClientReady;
            Client.ComponentInteractionCreated += ComponentEventHandler;
            Client.ModalSubmitted += ModalEventHandler;

            //6. Set up the Commands Configuration
            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { configJsonFile.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false,
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            //7. Register your commands

            Commands.RegisterCommands<Basic>();

            //8. Connect to get the Bot online
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task ModalEventHandler(DiscordClient sender, ModalSubmitEventArgs e)
        {
            var modalValues = e.Values;
            if (e.Interaction.Type == InteractionType.ModalSubmit && e.Interaction.Data.CustomId == "supportFormModal")
            {
                var random = new Random();
                var ticketEngine = new TicketEngine();

                ulong minValue = 1000000000000000000;
                ulong maxValue = 9999999999999999999;

                ulong randomNumber = (ulong)random.Next((int)(minValue >> 32), int.MaxValue) << 32 | (ulong)random.Next();
                ulong result = randomNumber % (maxValue - minValue + 1) + minValue;

                var supportTicket = new SupportTicket()
                {
                    username = e.Interaction.User.Username,
                    issue = modalValues.Values.First(),
                    ticketNo = ticketEngine.GetTotalTickets() + 1,
                    ticketId = result
                };
                var issueChannel = await e.Interaction.Guild.CreateChannelAsync($"{e.Interaction.User.Username}'s Issue {supportTicket.ticketNo}", ChannelType.Text, e.Interaction.Channel.Parent);
                ticketEngine.StoreTicket(supportTicket);

                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Succesfully created ticket for {e.Interaction.User.Username}"));

                var issueEmbed = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Lilac,
                    Title = $"{e.Interaction.User.Username} has opened an issue!!!",
                    Description = $"Issue: {modalValues.Values.First()} \n\n" +
                                  $"Ticket ID: {supportTicket.ticketId}"
                };

                await issueChannel.SendMessageAsync(embed: issueEmbed);
            }
        }

        private static async Task ComponentEventHandler(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {
            var ticketEngine = new TicketEngine();
            switch (e.Interaction.Data.CustomId)
            {
                case "submitButton":
                    var supportModal = new DiscordInteractionResponseBuilder()
                        .WithCustomId("supportFormModal")
                        .WithTitle("Create your Support Ticket")
                        .AddComponents(new TextInputComponent("Type your issue here...", "supportTextBox"));

                    await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, supportModal);
                    break;

                case "viewButton":
                    var tickets = ticketEngine.GetTickets();
                    int count = 0;
                    string[] tempList = new string[tickets.Count];

                    foreach (var ticket in tickets)
                    {
                        tempList[count] = $"TicketNo: **{ticket.ticketNo}**, TicketId: **{ticket.ticketId}**, Author: **{ticket.username}**";
                        count++;
                    }

                    var ticketViewEmbed = new DiscordEmbedBuilder()
                    {
                        Color = DiscordColor.Azure,
                        Title = "Ticket Viewer",
                        Description = string.Join("\n", tempList)
                    };

                    await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(ticketViewEmbed));
                    break;

                case "deleteButton":
                    var idEmbed = new DiscordEmbedBuilder()
                    {
                        Color = DiscordColor.Azure,
                        Title = "Please enter the ID of the ticket you want to delete"
                    };

                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(idEmbed));
                    var ID = await e.Channel.GetNextMessageAsync(TimeSpan.FromMinutes(3));

                    var isDeleted = ticketEngine.DeleteTicket(ulong.Parse(ID.Result.Content));

                    if (isDeleted == true)
                    {
                        var success = new DiscordEmbedBuilder()
                        {
                            Color = DiscordColor.Green,
                            Title = "Successfully deleted the ticket!!!"
                        };
                        await e.Channel.SendMessageAsync(embed: success);
                    }
                    else
                    {
                        var failure = new DiscordEmbedBuilder()
                        {
                            Color = DiscordColor.Red,
                            Title = "Something went wrong!!!"
                        };
                        await e.Channel.SendMessageAsync(embed: failure);
                    }
                    break;

                case "editButton":
                    break;
            }
        }

        private static Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
