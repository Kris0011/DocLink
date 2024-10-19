using System.Collections;
using System.Collections.Generic;
using DocLink.Models;

namespace DocLink.ViewModel;

public class DoctorsViewModel
{
    public IEnumerable<Doctor> _doctors { get; set; }
    public Hospital _hospital { get; set; }
}