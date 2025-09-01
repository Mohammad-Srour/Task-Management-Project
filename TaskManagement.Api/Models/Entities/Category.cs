namespace TaskManagement.Api.Models.Entities
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }


        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    }
}
