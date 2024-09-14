namespace DocLink.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        
        public int FullName { get; set; }
        
        public int Specialization { get; set; }
        
        public Hospital Hospital { get; set; }
    }
}