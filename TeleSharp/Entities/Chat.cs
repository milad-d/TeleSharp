namespace TeleSharp.Entities
{
    public class Chat : MessageSender
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool AllMembersAreAdministrators { get; set; }
    }
}
