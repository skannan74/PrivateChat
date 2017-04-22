using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Connector;

namespace UserAgentBot.Model
{
    public class Helper
    {


        public static Agent GetAvailableAgent()
        {
            var availableAgent =
                Common.Agents.FirstOrDefault(available => available.Availability == enmAvailability.Available);
            if (availableAgent != null)
            {
                availableAgent.Availability = enmAvailability.Busy;
            }
            return availableAgent;
        }

        public static Member GetUser(string conversationid)
        {
            var user =
                Common.UsersInConversation.FirstOrDefault(u => u.Conversation.Id == conversationid);
            return user;
        }


        public static async Task SendMessage(IActivity sender, Human receiver, string message, Human currentUser = null)
        {
            //var senderactivity = sender as Activity;
            IMessageActivity newMessage = Activity.CreateMessageActivity();
            newMessage.Type = ActivityTypes.Message;
            newMessage.From = sender.Recipient;
            newMessage.Conversation = receiver.Conversation;
            newMessage.Recipient = receiver.From;
            newMessage.Text = message;
            var connector = new ConnectorClient(new Uri(receiver.ServiceURL));
            var activity = (Activity)newMessage;

            if (currentUser != null)
            {
                StateClient sc = activity.GetStateClient();
                BotData userData = sc.BotState.GetConversationData(currentUser.ChannelId, activity.Conversation.Id);
                userData.SetProperty("Human", currentUser);
                userData.SetProperty("TalkingWith", "User");
            }
            await connector.Conversations.SendToConversationAsync(activity);
        }

        public static async Task SendMessage(IActivity sender,bool isAgent)
        {
            var receiver = isAgent ? UserAgentMap.GetMember(sender) : UserAgentMap.GetAgent(sender);
            await SendMessage(sender,receiver,(sender as Activity).Text);
        }
    }
}