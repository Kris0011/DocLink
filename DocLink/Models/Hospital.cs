using System.Collections.Generic;

namespace DocLink.Models
{
    public class Hospital
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Address { get; set; }
        
        public string Phone { get; set; }
        
        public IList<Doctor> Doctors { get; set; }
    }
}