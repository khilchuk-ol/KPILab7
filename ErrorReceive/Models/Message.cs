using Newtonsoft.Json;
using System;

namespace ErrorReceive
{
    public class Message
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ReceiveTime { get; set; }

        [JsonConstructor]
        public Message(string text, DateTime createTime)
        {
            Text = text;
            CreateTime = createTime;
            ReceiveTime = DateTime.Now;
        }

        public Message() { }
    }
}
