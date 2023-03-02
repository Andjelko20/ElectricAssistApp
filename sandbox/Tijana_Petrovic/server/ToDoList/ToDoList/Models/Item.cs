using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Item
    {
        public int Id { get; set; }
        public String Name { get; set; } = String.Empty;
    }
}
