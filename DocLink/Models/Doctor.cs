using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocLink.Models
{
    public class Doctor
    {
        public int Id { get; set; } 

        [Required]
        [MaxLength(100)] 
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)] 
        public string Specialization { get; set; }

        [Required]
        public string Email {  get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [ForeignKey("Hospital")] 
        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; } 

        public ICollection<Appointment> Appointments { get; set; } 
    }
}
