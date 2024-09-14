using System;

namespace DocLink.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        
        public string Subject { get; set; }
        
        public Patient Patient { get; set; }
        
        public Doctor Doctor { get; set; }
        
        public DateTime Date { get; set; }
        
        
        public string Status { get; set; }
        
    }
}