using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UserAgentBot.Model;

namespace UserAgentBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                
                StateClient sc = activity.GetStateClient();

                BotData userData = sc.BotState.GetConversationData(activity.ChannelId, activity.Conversation.Id);

                var TalkingWith = userData.GetProperty<enmTalkingWith>("TalkingWith");
                var isAgent = userData.GetProperty<bool>("isAgent");
                if (!isAgent && (TalkingWith == null || TalkingWith == enmTalkingWith.Bot))
                {
                    await Conversation.SendAsync(activity, () => new Dialogs.DecisionMakingDialog());
                }
                else
                {
                    //  Human agent = userData.GetProperty<Human>("Human");
                  //  var isAgent = userData.GetProperty<bool>("isAgent");
                    await Helper.SendMessage(activity, isAgent); // user -> agent
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            switch (message.Type)
            {
                case ActivityTypes.DeleteUserData:
                    break;
                case ActivityTypes.ConversationUpdate:
                case ActivityTypes.ContactRelationUpdate: // is user is member
                    Member.AddUser(message);
                    break;
                case ActivityTypes.Event: // if user is agent
                    Agent.AddAgent(message);
                    StateClient sc = message.GetStateClient();
                    BotData userData = sc.BotState.GetConversationData(message.ChannelId, message.Conversation.Id);
                    userData.SetProperty("isAgent", true);
                    // var TalkingWith = userData.GetProperty<enmTalkingWith>("TalkingWith");
                    sc.BotState.SetConversationData(message.ChannelId, message.Conversation.Id, userData);

                    break;
                case ActivityTypes.Typing:
                    break;
            }

            return null;
        }
    }
}