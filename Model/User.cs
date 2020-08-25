namespace Model
{
    public class User
    {
        public string _id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Birthday { get; set; }
        public string Phone { get; set; }
        public string[] Role { get; set; }
        public string CreateAt { get; set; }
        public string updateAt { get; set; }
        public string LastLogin { get; set; }
        public string Status { get; set; }
    }
}
