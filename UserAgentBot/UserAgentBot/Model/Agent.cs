using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;

namespace UserAgentBot.Model
{
    public class Agent : Human
    {
        public enmAvailability Availability { get; set; }

        public static void AddAgent(Activity activity)
        {
            Agent agent = new Agent()
            {
                From = activity.From,
                Conversation = activity.Conversation,
                Availability = enmAvailability.Available,
                ServiceURL = activity.ServiceUrl,
                ChannelId = activity.ChannelId
            };
            Common.Agents.Add(agent);
           
        }
    }

    public enum enmAvailability
    {
        Available,
        Busy,
        Offline
    }
}