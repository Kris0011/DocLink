using System.Collections.Generic;
using DocLink.Models;

namespace DocLink.ViewModel;

public class DoctorDashboardViewModel
{
    public Doctor Doctor { get; set; }
    public List<Appointment> Appointments { get; set; }
    
    public DoctorDashboardViewModel(Doctor doctor, List<Appointment> appointments)
    {
        Doctor = doctor;
        Appointments = appointments;
    }
}