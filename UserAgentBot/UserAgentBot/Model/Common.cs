using System.Collections.Generic;

namespace UserAgentBot.Model
{
    public class Common
    {
        public static List<Member> UsersInConversation { get; set; }
        public static List<Agent> Agents { get; set; }

        static Common()
        {
            UsersInConversation = new List<Member>();
            Agents  = new List<Agent>();

        }
    }
}