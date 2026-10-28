namespace webdammay.dammay.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string UserName { get; private set; }
        public string Gmail { get; private set; }
        public string PasswordHash {  get; private set; }
        public DateTime CreatedAt { get; private set; }
        public User( string username,string gmail,string passwordHash)
        {
            if(string.IsNullOrWhiteSpace(username)) { throw new ArgumentNullException("Username không được trống."); }
            if (string.IsNullOrWhiteSpace(gmail)) { throw new ArgumentNullException("Gmail không được trống."); }
            if (string.IsNullOrWhiteSpace(passwordHash)) { throw new ArgumentNullException("PasswordHash không được trống."); }
            Id= Guid.NewGuid();
            UserName= username;
            Gmail= gmail;
            PasswordHash= passwordHash;
            CreatedAt = DateTime.UtcNow;
        }
        private User() { }
    }
}
