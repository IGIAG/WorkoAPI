namespace WorkoAPI.Objects
{
    public class PublicUserData
    {
        public string id { get; set; }
        public string name { get; set; }

        public PublicUserData(string id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
