namespace DocumentManagement.Models
{
    /// <summary>
    /// Request Model for Register Model.
    /// </summary>
    public class RegisterModel
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public  required string Role { get; set; }
    }
}
