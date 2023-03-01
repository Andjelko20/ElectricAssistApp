using System.ComponentModel.DataAnnotations;

namespace Server.Model
{
    public class Note
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
