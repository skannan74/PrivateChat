using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;

namespace UserAgentBot.Model
{
    [Serializable]
    public class Human
    {
        public ChannelAccount From { get; set; }
        public ConversationAccount Conversation { get; set; }
        public string ServiceURL { get; set; }

        public string ChannelId { get; set; }
    }
}