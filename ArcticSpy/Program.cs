using FortniteDotNet;
using FortniteDotNet.Enums.Accounts;
using FortniteDotNet.Enums.Party;
using FortniteDotNet.Models.AccountService;
using FortniteDotNet.Models.EventsService;
using FortniteDotNet.Models.FortniteService;
using FortniteDotNet.Models.PartyService;
using FortniteDotNet.XMPP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArcticSpy
{
    class Program
    {
        private static FortniteApi _api;

        // We use fields so we can access these in the Xmpp method.
        private static XMPPClient _xmppClient;
        private static OAuthSession _authSession;

        private static async Task Main()
        {
            _api = new FortniteApi();

            // Creates an OAuth session for the iOS client using the device_auth grant type.
            _authSession = await _api.AccountService.GenerateOAuthSession(GrantType.RefreshToken, AuthClient.PC, new()
            {

                { "refresh_token", "13de0ba76cb346648eda112cb9f1d233" },
            });

            Console.WriteLine("New: " + _authSession.RefreshToken);
            Console.WriteLine("Current: " + _authSession.AccessToken);

            Console.WriteLine(JsonConvert.SerializeObject(await _authSession.GetAccountInfo()));


            return;

            McpResponse res = await _authSession.ClientQuestLogin(FortniteDotNet.Enums.Fortnite.Profile.Athena);
            //res.ProfileChanges.First().

            EventData response = await _authSession.GetEventData(FortniteDotNet.Enums.Events.Region.EU, FortniteDotNet.Enums.Events.Platform.Windows);

            File.WriteAllText("GetEventData", JsonConvert.SerializeObject(response));

            return;

            // Starts XMPP-related operations on a new thread.
            new Thread(Xmpp).Start();

            // Creates a party for the XMPP client using the OAuth session we generated earlier.
            await _authSession.InitParty(_xmppClient);

            // Updates the XMPP client's current party privacy to public using the OAuth session we generated earlier.
            await _xmppClient.CurrentParty.UpdatePrivacy(_authSession, new PartyPrivacy(Privacy.Private));
        }

        private static void Xmpp()
        {
            // Sets up the XMPP client.
            _xmppClient = new XMPPClient(_authSession);

            // We're using the OnGroupChatReceived event handler to listen for any messages sent in the chat of the client's current party.
            _xmppClient.OnGroupChatReceived += async (_, chat) =>
            {
                // If the body doesn't start with !, return.
                if (!chat.Body.StartsWith("!"))
                    return;

                // Get the arguments by removing the ! and splitting the body by a space.
                var args = chat.Body.Remove(0, 1).Split(" ");

                // Get the command (the first argument)
                var command = args.FirstOrDefault();

                // Get the content (every string after the argument)
                var content = string.Join(" ", args.Skip(1));

                // Get the client from the list of its current party's members.
                var me = _xmppClient.CurrentParty.Members.FirstOrDefault(x => x.Id == _authSession.AccountId);

                // Use a switch case on the command (this is better practice than if, else etc.)
                switch (command)
                {
                    // If the command is emote...
                    case "emote":
                        {
                            // Set the client's emote to the content variable. Example: "!emote floss" would set the client's emote to Floss.
                            await me.SetEmote(_xmppClient, content);
                            break;
                        }
                    case "outfit":
                        {
                            // If the content contains :, this implies the user wants to apply a variant.
                            if (content.Contains(":"))
                            {
                                // The SetOutfit method has an optional parameter, which we're using here. Example: "!outfit Skull Trooper:Purple Glow" would set the client's outfit to Skull Trooper, and the outfit's active variant to the Purple Glow variant.
                                await me.SetOutfit(_xmppClient, content.Split(":")[0], content.Split(":")[1]);
                                break;
                            }

                            // Otherwise, just set the outfit without a variant.
                            await me.SetOutfit(_xmppClient, content);
                            break;
                        }
                    case "banner":
                        {
                            // Set the client's banner to the content variables. Example: "!banner BRSeason01:DefaultColor02" would set the client's banner icon to the Battle Bus banner, and the client's banner color to red.
                            await me.SetBanner(_xmppClient, content.Split(":")[0], content.Split(":")[1]);
                            break;
                        }
                }
            };

            // Initialize the XMPP client. This method connects us to Epic Games' XMPP services and starts listening for messages.
            _xmppClient.Initialize().GetAwaiter().GetResult();
        }
    }
}
