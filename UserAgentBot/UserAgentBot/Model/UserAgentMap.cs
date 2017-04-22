using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Web;
using Microsoft.Bot.Connector;

namespace UserAgentBot.Model
{
    public  class UserAgentMap 
    {
        static Dictionary<Member, Agent> map = new Dictionary<Member, Agent>();
        public static void AddMap(Member member, Agent agent)
        {
            map.Add(member,agent);
        }

        internal static Human GetMember(IActivity sender)
        {
            var conversationid = sender.Conversation.Id;
           KeyValuePair<Member, Agent> memberagent = map.FirstOrDefault(cid => cid.Value.Conversation.Id == conversationid);
            return memberagent.Key;
        }

        internal static Human GetAgent(IActivity sender)
        {
            var conversationid = sender.Conversation.Id;
            KeyValuePair<Member, Agent> memberagent = map.FirstOrDefault(cid => cid.Key.Conversation.Id == conversationid);
            return memberagent.Value;
        }
    }
}