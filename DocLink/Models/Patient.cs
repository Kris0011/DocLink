using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DocLink.Models
{


    public class Patient
    {
        public int Id { get; set; } 

        [Required]
        [MaxLength(100)] 
        public string FullName { get; set; }

        [EmailAddress]
        [Required]
        [MaxLength(100)] 
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [MaxLength(100)] 
        public string Address { get; set; }

        [Required]
        [MaxLength(10)] 
        public string PhoneNumber { get; set; }

        [MaxLength(10)] 
        public string Gender { get; set; }

        [Required]
        public int Age { get; set; }


        public ICollection<Appointment> Appointments { get; set; }
    }
}
