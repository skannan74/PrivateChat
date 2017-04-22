using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;

namespace UserAgentBot.Model
{
    public class Member : Human
    {
        public enmTalkingWith TalkingWith { get; set; }
        public static void AddUser(Activity activity)
        {
            Member user = new Member()
            {
                From = activity.From,
                Conversation = activity.Conversation,
                TalkingWith = enmTalkingWith.Bot,
                ServiceURL = activity.ServiceUrl,
                ChannelId = activity.ChannelId
            };
            Common.UsersInConversation.Add(user);
        }
    }

    public enum enmTalkingWith
    {
        Bot,
        Agent
    }
   
}