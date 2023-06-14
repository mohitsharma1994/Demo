namespace Demo.Infrastructure.Entity
{
    public class UserLog
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public bool IsSuccess { get; set; } 

        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }

        public virtual User User { get; set; }
    }
}
