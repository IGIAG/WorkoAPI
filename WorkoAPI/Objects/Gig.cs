namespace WorkoAPI.Objects
{
    public class Gig
    {
        public string id;

        public string title;

        public string description;

        public string authorUserId;

        public int rewardPoints;

        public bool active;

        public Gig(string id, string title, string description, string authorUserId, int rewardPoints, bool active)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.authorUserId = authorUserId;
            this.rewardPoints = rewardPoints;
            this.active = active;
        }
    }
}
