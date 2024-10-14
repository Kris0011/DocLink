using System.ComponentModel.DataAnnotations;

namespace DocLink.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required]
        public int FullName { get; set; }

        [Required]
        public int Specialization { get; set; }

        [Required]
        public Hospital Hospital { get; set; }
    }
}