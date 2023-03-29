namespace WorkoAPI.Objects
{
    public class Solution
    {
        public string id;

        public string authorId;

        public string gigId;

        public string content;

        public bool isBest;

        public Solution(string id, string authorId, string gigId, string content)
        {
            this.id = id;
            this.authorId = authorId;
            this.gigId = gigId;
            this.content = content;
            this.isBest = false;
        }
    }
}
