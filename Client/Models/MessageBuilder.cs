using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client.Models {
    public class MessageBuilder {
        public Message ParseMessage(string message) {
            Message m = new Message();
            var json = JObject.Parse(message);
            foreach (var item in json) {
                if (item.Value.Type.ToString().Equals("Integer")) {
                    m.AddParameter(new IntParameter(item.Key, (int)item.Value));
                } else {
                    m.AddParameter(new StringParameter(item.Key, item.Value.ToString()));
                }
            }
            return m;
        }

        public Message PrepareRegisterBotMessage(string name, GameModeType gamemode) {
            Message m = new Message();
            m.AddParameter(new StringParameter("type", "register"));
            m.AddParameter(new StringParameter("clientType", "bot"));
            m.AddParameter(new StringParameter("name", name));
            m.AddParameter(new StringParameter("game", String.Join(" ", Regex.Split(gamemode.ToString(), @"(?<!^)(?=[A-Z])"))));
            return m;
        }
    }
}
