using Newtonsoft.Json;
using System;

namespace Send
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Message
    {
        public string Text { get; set; }

        public DateTime CreateTime { get; }

        [JsonIgnore]
        public Keys ContentType { get; }

        [JsonIgnore]
        private static readonly Random random = new Random();

        public Message()
        {
            Text = Guid.NewGuid().ToString("N");

            int ind = random.Next(0, 4);
            ContentType = ind switch
            {
                0 => Keys.error,
                1 => Keys.info,
                _ => Keys.stuff,
            };

            CreateTime = DateTime.Now;
        }
    }
}

