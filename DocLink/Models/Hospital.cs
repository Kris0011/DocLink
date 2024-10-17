using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DocLink.Models
{
    public class Hospital
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }   

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string  Email { get; set; }

        [Required]
        public string Password { get; set; }
        

        public IList<Doctor> Doctors { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}