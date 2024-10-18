using System.Collections.Generic;

namespace DocLink.ViewModel
{
    public class ScheduleAppointmentViewModel
    {
        public int HospitalId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public List<DoctorViewModel> DoctorList { get; set; } = new List<DoctorViewModel>(); 
    }

    public class DoctorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HospitalName { get; set; } 
    }
}
