using BestMatchDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using UserAgentBot.Model;

namespace UserAgentBot.Dialogs
{
    [Serializable]
    public class DecisionMakingDialog : BestMatchDialog<object>
    {
        public override async Task NoMatchHandler(IDialogContext context, string messageText)
        {
            await context.PostAsync("I didn't understand you.");
            await context.PostAsync("You can contact our cusomer care representative by typing \"Connect me with customer care\"");
            context.Done<object>(null);
        }


        [BestMatch(new string[] { "1","i want to talk to a human" , "transfer to agent" , "contact human agent", "contact agent","contact human", "transfer to human agent" ,
            "transfer to agent" , "transfer to human", "i want to talk to customer care representative" ,"transfer to customer care","contact a human","contact customer care"},
            threshold: 0.5, ignoreCase: true, ignoreNonAlphaNumericCharacters: true)]
        public async Task AgentTransfer(IDialogContext context, string messageText)
        {
            await context.PostAsync("Please wait for your request to be accepted.");
            Agent availableAgent = Helper.GetAvailableAgent();
            if (availableAgent == null)
            {
                await context.PostAsync("All our customer care representatives are busy at the moment. Please try after some time.");
            }
            else
            {
                await context.PostAsync($"You are connected to {availableAgent.From.Name}");
                var activity = ((Activity) context.Activity);
                var currentuser = Helper.GetUser(activity.Conversation.Id);
                await Helper.SendMessage(context.Activity, availableAgent,$"You are connected to {context.Activity.From.Name}");
                
                UserAgentMap.AddMap(currentuser,availableAgent);
                
                //var activity = ((Activity) context.Activity);
                //StateClient sc =activity.GetStateClient();
                //BotData userData = sc.BotState.SetConversationData(activity.ChannelId, activity.Conversation.Id,);
                //userData.SetProperty("TalkingWith",enmTalkingWith.Agent);
                context.ConversationData.SetValue("TalkingWith", enmTalkingWith.Agent);
                //var TalkingWith = userData.GetProperty<enmTalkingWith>("TalkingWith");
                //context.ConversationData.SetValue("TalkingWith", enmTalkingWith.Agent);
               //  context.ConversationData.SetValue("Human", availableAgent);
                //context.UserData.SetValue("AgentName", availableAgent.From.Name);
                //context.UserData.SetValue("AgentServiceUrl", availableAgent.ServiceURL);
                //context.UserData.SetValue("AgentConversation", availableAgent.Conversation);


            }
            
            //var activity = await message;
            //var agent = await _userToAgent.IntitiateConversationWithAgentAsync(activity as Activity, default(CancellationToken));
            //if (agent == null)
            //    await context.PostAsync("All our customer care representatives are busy at the moment. Please try after some time.");
            context.Done<object>(null);
        }
    }
}