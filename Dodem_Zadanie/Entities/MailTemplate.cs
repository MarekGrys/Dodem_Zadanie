using System.ComponentModel.DataAnnotations;

namespace Dodem_Zadanie.Entities
{
    public class MailTemplate
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
