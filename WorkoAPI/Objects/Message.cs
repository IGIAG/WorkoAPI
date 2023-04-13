namespace WorkoAPI.Objects
{
    public class Message
    {
        public string authorId { get;set; }
        public string targetId { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public int unixTimestamp { get; set; }

        public Message(string authorId, string targetId, string title, string content, int unixTimestamp)
        {
            this.authorId = authorId;
            this.targetId = targetId;
            this.title = title;
            this.content = content;
            this.unixTimestamp = unixTimestamp;
        }
    }
}
