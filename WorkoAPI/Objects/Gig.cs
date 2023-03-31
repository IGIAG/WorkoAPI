namespace WorkoAPI.Objects
{
    public class Gig
    {
        public string id {get;set;}

        public string title {get;set;}

        public string description {get;set;}

        public string authorUserId {get;set;}

        public int rewardPoints {get;set;}

        public bool active {get;set;}

        public IEnumerable<string> solutions {get;set;}

        public Gig(string id, string title, string description, string authorUserId, int rewardPoints, bool active)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.authorUserId = authorUserId;
            this.rewardPoints = rewardPoints;
            this.active = active;
            solutions = new List<string>();
        }
    }
}
