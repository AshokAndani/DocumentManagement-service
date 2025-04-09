
namespace DocumentManagement.Entities
{
    /// <summary>
    /// Db Entity referring to user role.
    /// </summary>
    public class Role
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? CreateTimestamp { get; set; }

        public DateTime? ModifyTimestamp { get; set; }
    }
}
