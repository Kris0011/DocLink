using System;
using System.ComponentModel.DataAnnotations;

namespace DocLink.Models
{
    public class Appointment
    {
        public int Id { get; set; } 

        [Required]
        public string Subject { get; set; }

        [Required] 
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [Required] 
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } 

        [Required] 
        public DateTime Date { get; set; }

        [Required] 
        public string Status { get; set; }

        public string RescheduleMessage { get; set; } 
    }
}
