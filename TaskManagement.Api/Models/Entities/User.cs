namespace TaskManagement.Api.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TaskItem>Tasks { get; set; }=new List<TaskItem>();
        public ICollection<Category> Categories { get; set; }


    }
}
