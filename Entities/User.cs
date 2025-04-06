
namespace DocumentManagement.Entities
{
    /// <summary>
    /// Db Entity referring to a physical application user.
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public int RoleId { get; set; }

        public  Role? Role { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? CreateTimestamp { get; set; }

        public DateTime? ModifyTimestamp { get; set; }
    }

}
