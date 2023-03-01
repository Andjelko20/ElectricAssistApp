namespace FullStack_Demo_API.Models
{
    public class Zaposlen
    {
        public Guid id { get; set; } // global uniqe identificator
        public string ime { get; set; }
        public string email { get; set; }
        public string telefon { get; set; }
        public long plata { get; set; }
        public string odeljenje { get; set; }
    }
}
